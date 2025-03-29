using FireflyHttp;

namespace FireflyTester.WebSocketImplementationSamples
{
    public class ChatClient
    {
        private FireflyWebSocket _webSocket;

        public async Task StartAsync(string wsUrl)
        {
            _webSocket = await FireflyWebSocket.ConnectAsync(wsUrl);

            // Handle incoming messages
            _webSocket.OnMessageReceived += message =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Server]: {message}");
                Console.ResetColor();
            };

            _webSocket.OnError += ex =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            };

            Console.WriteLine("Connected to chat. Type your message and press Enter.");
            await ListenForUserInput();
        }

        private async Task ListenForUserInput()
        {
            while (true)
            {
                var message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message)) continue;

                if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Closing chat...");
                    await _webSocket.CloseAsync();
                    break;
                }

                await _webSocket.SendAsync(message);
            }

        }
    }
}
