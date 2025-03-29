using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FireflyHttp
{
    public class FireflyWebSocket
    {
        private ClientWebSocket _webSocket;
        private readonly Uri _uri;
        private CancellationTokenSource _cancellationTokenSource;
        private int _reconnectAttempts;
        private readonly ILogger<FireflyWebSocket>? _logger; // Optional logger

        // Events for received messages and errors
        public event Action<string>? OnMessageReceived;
        public event Action<byte[]>? OnBinaryReceived;
        public event Action<Exception>? OnError;

        /// <summary>
        /// Private constructor to enforce factory usage.
        /// </summary>
        private FireflyWebSocket(string url, ILogger<FireflyWebSocket>? logger = null)
        {
            _webSocket = new ClientWebSocket();
            _uri = new Uri(url);
            _logger = logger;
        }

        /// <summary>
        /// Creates and connects a WebSocket client.
        /// </summary>
        /// <param name="url">WebSocket server URL.</param>
        /// <param name="logger">Optional logger instance.</param>
        /// <returns>A connected FireflyWebSocket instance.</returns>
        public static async Task<FireflyWebSocket> ConnectAsync(string url, ILogger<FireflyWebSocket>? logger = null)
        {
            var client = new FireflyWebSocket(url, logger);
            await client.ConnectInternalAsync();
            return client;
        }

        /// <summary>
        /// Establishes a WebSocket connection.
        /// </summary>
        private async Task ConnectInternalAsync()
        {
            try
            {
                _logger?.LogInformation("Connecting to WebSocket: {Url}", _uri);
                _cancellationTokenSource = new CancellationTokenSource();

                await _webSocket.ConnectAsync(_uri, _cancellationTokenSource.Token);
                _reconnectAttempts = 0; // Reset reconnection attempts

                _logger?.LogInformation("Connected to WebSocket: {Url}", _uri);
                _ = StartReceiving(); // Start listening for incoming messages
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "WebSocket connection failed: {Url}", _uri);
                OnError?.Invoke(ex);
                await AttemptReconnectAsync();
            }
        }

        /// <summary>
        /// Sends a text message over WebSocket.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public async Task SendAsync(string message)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                _logger?.LogDebug("Sending message: {Message}", message);
                await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Sends binary data over WebSocket.
        /// </summary>
        /// <param name="data">Binary data to send.</param>
        public async Task SendBinaryAsync(byte[] data)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                _logger?.LogDebug("Sending binary data ({Size} bytes)", data.Length);
                await _webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, _cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Listens for incoming WebSocket messages.
        /// </summary>
        private async Task StartReceiving()
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _logger?.LogWarning("WebSocket closed by server: {Url}", _uri);
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", _cancellationTokenSource.Token);
                        await AttemptReconnectAsync();
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        _logger?.LogInformation("Received message: {Message}", message);
                        OnMessageReceived?.Invoke(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        var binaryData = new byte[result.Count];
                        Array.Copy(buffer, binaryData, result.Count);
                        _logger?.LogInformation("Received binary data ({Size} bytes)", result.Count);
                        OnBinaryReceived?.Invoke(binaryData);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "WebSocket receive error: {Url}", _uri);
                    OnError?.Invoke(ex);
                    await AttemptReconnectAsync();
                }
            }
        }

        /// <summary>
        /// Attempts to reconnect with exponential backoff.
        /// </summary>
        private async Task AttemptReconnectAsync()
        {
            if (_reconnectAttempts < 5)
            {
                _reconnectAttempts++;
                int delay = (int)Math.Pow(2, _reconnectAttempts) * 1000; // Exponential backoff
                _logger?.LogWarning("Attempting to reconnect (Attempt {Attempt}): {Url}", _reconnectAttempts, _uri);

                await Task.Delay(delay);
                _webSocket.Dispose();
                _webSocket = new ClientWebSocket();
                await ConnectInternalAsync();
            }
            else
            {
                _logger?.LogError("Max reconnect attempts reached. WebSocket connection failed: {Url}", _uri);
            }
        }

        /// <summary>
        /// Closes the WebSocket connection gracefully.
        /// </summary>
        public async Task CloseAsync()
        {
            _logger?.LogInformation("Closing WebSocket connection: {Url}", _uri);
            _cancellationTokenSource.Cancel();
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", CancellationToken.None);
        }
    }
}
