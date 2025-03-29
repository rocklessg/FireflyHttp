using Microsoft.Extensions.Logging;
using FireflyHttp;
namespace FireflyTester.WebSocketImplementationSamples
{

    public static class WebSocketSampleUsage
    {
        /// <summary>
        /// Initializes FireflyWebSocket with a logger.
        /// </summary>
        public static async Task WithLogger()
        {
            // NOTE: You can add any of your logging provider.
            // For this sample implementation, we use Console logging.
            // From Microsoft.Extensions.Logging.Console package.
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); // Add console logging
            });
            var logger = loggerFactory.CreateLogger<FireflyWebSocket>();

            string wsUrl = "wss://echo-websocket.fly.dev";
            
            var webSocket = await FireflyWebSocket.ConnectAsync(wsUrl, logger);

            SubscribeToEvents(webSocket);

            await webSocket.SendAsync("Hello from Firefly!");
            await webSocket.SendBinaryAsync(new byte[] { 1, 2, 3, 4, 5 });

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            await webSocket.CloseAsync();
        }

        /// <summary>
        /// Initializes FireflyWebSocket without logging. (This is not encouraged)
        /// </summary>
        public static async Task WithoutLogger()
        {
            string wsUrl = "wss://echo-websocket.fly.dev";

            var webSocket = await FireflyWebSocket.ConnectAsync(wsUrl);
           
            SubscribeToEvents(webSocket);

            await webSocket.SendAsync("Hello from Firefly!");
            await webSocket.SendBinaryAsync(new byte[] { 1, 2, 3, 4, 5 });

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            await webSocket.CloseAsync();
        }

        /// <summary>
        /// Subscribes to WebSocket events.
        /// </summary>
        private static void SubscribeToEvents(FireflyWebSocket webSocket)
        {
            webSocket.OnMessageReceived += message => Console.WriteLine($"Message: {message}");
            webSocket.OnBinaryReceived += data => Console.WriteLine($"Binary Data: {data.Length} bytes");
            webSocket.OnError += ex => Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
