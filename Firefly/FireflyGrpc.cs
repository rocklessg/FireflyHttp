using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace FireflyHttp
{
    public class FireflyGrpc<TClient> where TClient : class
    {
        private readonly GrpcChannel _channel;
        private readonly ILogger<FireflyGrpc<TClient>>? _logger;

        public TClient Client { get; }

        /// <summary>
        /// Optional default headers to be included in all calls.
        /// </summary>
        public Metadata Headers { get; } = new Metadata();

        private FireflyGrpc(string serverUrl, Func<GrpcChannel, TClient> clientFactory, ILogger<FireflyGrpc<TClient>>? logger = null)
        {
            _logger = logger;
            _channel = GrpcChannel.ForAddress(serverUrl);
            _logger?.LogInformation("gRPC channel created: {Url}", serverUrl);
            Client = clientFactory(_channel);
        }

        public static Task<FireflyGrpc<TClient>> ConnectAsync(
            string serverUrl,
            Func<GrpcChannel, TClient> clientFactory,
            ILogger<FireflyGrpc<TClient>>? logger = null)
        {
            var grpcClient = new FireflyGrpc<TClient>(serverUrl, clientFactory, logger);
            return Task.FromResult(grpcClient);
        }

        public async Task ShutdownAsync()
        {
            _logger?.LogInformation("Shutting down gRPC channel.");
            await _channel.ShutdownAsync();
        }
    }
}

