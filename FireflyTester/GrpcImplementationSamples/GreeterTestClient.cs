using FireflyHttp;
using GrpcGreeter;
using Microsoft.Extensions.Logging;

namespace FireflyTester.GrpcImplementationSamples
{
    public class GreeterTestClient
    {
        private readonly ILogger<FireflyGrpc<Greeter.GreeterClient>>? _logger;

        public GreeterTestClient(ILogger<FireflyGrpc<Greeter.GreeterClient>>? logger = null)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            var grpc = await FireflyGrpc<Greeter.GreeterClient>.ConnectAsync(
                "https://localhost:7056", // Url to your gRpc server
                channel => new Greeter.GreeterClient(channel),
                _logger
            );

            // Without headers
            var request = new HelloRequest { Name = "NoHeaderUser" };
            var response = await grpc.Client.SayHelloAsync(request);
            Console.WriteLine($"[No Header] Server Response: {response.Message}");

            // With headers
            grpc.Headers.Add("Authorization", "Bearer YOUR_TOKEN_HERE");

            var requestWithHeader = new HelloRequest { Name = "WithHeaderUser" };
            var responseWithHeader = await grpc.Client.SayHelloAsync(requestWithHeader, grpc.Headers);
            Console.WriteLine($"[With Header] Server Response: {responseWithHeader.Message}");

            await grpc.ShutdownAsync();
        }
    }
}
