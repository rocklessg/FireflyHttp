using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace FireflyHttp
{
    /// <summary>
    /// A lightweight and easy-to-use HTTP client for making API requests with minimal configuration.
    /// Inspired by Python's requests library.
    /// </summary>
    public static class Firefly
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        private static ILogger? _logger;

        /// <summary>
        /// Sets the timeout duration for HTTP requests.
        /// </summary>
        public static void SetTimeout(TimeSpan timeout) => _httpClient.Timeout = timeout;

        /// <summary>
        /// Sets the logger instance for logging requests and responses.
        /// </summary>
        public static void SetLogger(ILogger logger) => _logger = logger;

        #region RESTful Service

        /// <summary>
        /// Sends a GET request to the specified URL.
        /// </summary>
        public static async Task<string> Get(string url) =>
            await SendRequest<object>(HttpMethod.Get, url);

        /// <summary>
        /// Sends a GET request with custom headers.
        /// </summary>
        public static async Task<string> Get(string url, object? headers) =>
            await SendRequest<object>(HttpMethod.Get, url, headers);

        /// <summary>
        /// Sends a POST request with a JSON payload.
        /// </summary>
        public static async Task<string> Post<T>(string url, T data) =>
            await SendRequest(HttpMethod.Post, url, null, data);

        /// <summary>
        /// Sends a POST request with a JSON payload and custom headers.
        /// </summary>
        public static async Task<string> Post<T>(string url, T data, object? headers) =>
            await SendRequest(HttpMethod.Post, url, headers, data);

        /// <summary>
        /// Sends a PUT request with a JSON payload.
        /// </summary>
        public static async Task<string> Put<T>(string url, T data) =>
            await SendRequest(HttpMethod.Put, url, null, data);

        /// <summary>
        /// Sends a PUT request with a JSON payload and custom headers.
        /// </summary>
        public static async Task<string> Put<T>(string url, T data, object? headers) =>
            await SendRequest(HttpMethod.Put, url, headers, data);

        /// <summary>
        /// Sends a DELETE request to the specified URL.
        /// </summary>
        public static async Task<string> Delete(string url) =>
            await SendRequest<object>(HttpMethod.Delete, url);

        /// <summary>
        /// Sends a DELETE request with custom headers.
        /// </summary>
        public static async Task<string> Delete(string url, object? headers) =>
            await SendRequest<object>(HttpMethod.Delete, url, headers);

        #endregion       


        #region SOAP Service

        /// <summary>
        /// Sends a GET request with to the specied URL.
        /// </summary>
        public static async Task<string> GetXml(string url) =>
            await SendRequest<object>(HttpMethod.Get, url, isXml: true);

        /// <summary>
        /// Sends a GET request with custom headers..
        /// </summary>
        public static async Task<string> GetXml(string url, object? headers) =>
            await SendRequest<object>(HttpMethod.Get, url, headers, isXml: true);

        /// <summary>
        /// Sends a POST request with an XML payload.
        /// </summary>
        public static async Task<string> PostXml<T>(string url, T data) =>
            await SendRequest(HttpMethod.Post, url, null, data, isXml: true);

        /// <summary>
        /// Sends a POST request with an XML payload and custom headers.
        /// </summary>
        public static async Task<string> PostXml<T>(string url, T data, object? headers) =>
            await SendRequest(HttpMethod.Post, url, headers, data, isXml: true);

        /// <summary>
        /// Sends a PUT request with an XML payload.
        /// </summary>
        public static async Task<string> PutXml<T>(string url, T data) =>
            await SendRequest(HttpMethod.Put, url, null, data, isXml: true);

        /// <summary>
        /// Sends a PUT request with an XML payload and custom headers.
        /// </summary>
        public static async Task<string> PutXml<T>(string url, T data, object? headers) =>
            await SendRequest(HttpMethod.Put, url, headers, data, isXml: true);

        /// <summary>
        /// Sends a DELETE request with an XML payload.
        /// </summary>
        public static async Task<string> DeleteXml<T>(string url, T data) =>
            await SendRequest(HttpMethod.Delete, url, null, data, isXml: true);

        /// <summary>
        /// Sends a DELETE request with an XML payload and custom headers.
        /// </summary>
        public static async Task<string> DeleteXml<T>(string url, T data, object? headers) =>
            await SendRequest(HttpMethod.Delete, url, headers, data, isXml: true);

        #endregion

        /// <summary>
        /// Internal method for sending HTTP requests with optional headers and payloads.
        /// </summary>
        private static async Task<string> SendRequest<T>(HttpMethod method, string url, object? headers = null, T? data = default, bool isXml = false)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url);
                _logger?.LogInformation($"Sending {method} request to {url}");

                if (headers is not null)
                {
                    foreach (var property in headers.GetType().GetProperties())
                    {
                        request.Headers.Add(property.Name, property.GetValue(headers)?.ToString());
                    }
                }

                if (data is not null)
                {
                    string content;
                    string mediaType;

                    if (isXml)
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        using var stringWriter = new StringWriter();
                        serializer.Serialize(stringWriter, data);
                        content = stringWriter.ToString();
                        mediaType = "application/xml";
                    }
                    else
                    {
                        content = JsonSerializer.Serialize(data);
                        mediaType = "application/json";
                    }

                    request.Content = new StringContent(content, Encoding.UTF8, mediaType);
                }

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger?.LogInformation($"Response received from {url}: {responseBody}");
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error occurred while making request to {url}");
                throw;
            }
        }
    }
}

