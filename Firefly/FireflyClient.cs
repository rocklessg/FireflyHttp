using FireflyHttp.Dtos;
using System.Net.Http.Headers;

namespace FireflyHttp
{
    public class FireflyClient
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, string> _defaultHeaders = new();

        public FireflyClient(string baseAddress)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public void SetDefaultHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                _defaultHeaders[header.Key] = header.Value;
            }
        }

        public Dictionary<string, string> MergeHeaders(Dictionary<string, string>? headers)
        {
            var mergedHeaders = new Dictionary<string, string>(_defaultHeaders);
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    mergedHeaders[header.Key] = header.Value;
                }
            }
            return mergedHeaders;
        }

        #region Json requests

        public async Task<string> Get(string url, Dictionary<string, string>? headers = null) =>
            await Firefly.SendRequest<object>(HttpMethod.Get, url, MergeHeaders(headers), default, false, _client);

        public async Task<string> Post<T>(string url, T data, Dictionary<string, string>? headers = null, bool isXml = false) =>
            await Firefly.SendRequest(HttpMethod.Post, url, MergeHeaders(headers), data, isXml, _client);

        public async Task<string> Put<T>(string url, T data, Dictionary<string, string>? headers = null, bool isXml = false) =>
            await Firefly.SendRequest(HttpMethod.Put, url, MergeHeaders(headers), data, isXml, _client);

        public async Task<string> Delete(string url, Dictionary<string, string>? headers = null) =>
            await Firefly.SendRequest<object>(HttpMethod.Delete, url, MergeHeaders(headers), default, false, _client);

        #endregion

        #region Xml requests

        public async Task<string> GetXml(string url, Dictionary<string, string>? headers = null) =>
            await Firefly.SendRequest<object>(HttpMethod.Get, url, MergeHeaders(headers), default, true, _client);

        public async Task<string> PostXml<T>(string url, T data, Dictionary<string, string>? headers = null, bool isXml = true) =>
            await Firefly.SendRequest(HttpMethod.Post, url, MergeHeaders(headers), data, isXml, _client);

        public async Task<string> PutXml<T>(string url, T data, Dictionary<string, string>? headers = null, bool isXml = true) =>
            await Firefly.SendRequest(HttpMethod.Put, url, MergeHeaders(headers), data, isXml, _client);

        public async Task<string> DeleteXml(string url, Dictionary<string, string>? headers = null) =>
            await Firefly.SendRequest<object>(HttpMethod.Delete, url, MergeHeaders(headers), default, true, _client);

        #endregion


        /// <summary>
        /// Uploads files.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> PostFiles(PostFilesRequest request)
        {
            using var multipartContent = new MultipartFormDataContent();

            // Add files
            foreach (var file in request.Files)
            {
                using var fileContent = new StreamContent(file.FileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                multipartContent.Add(fileContent, file.FieldName, file.FileName);
            }

            // Add additional form fields if provided
            foreach (var field in request.AdditionalFormFields ?? [])
            {
                multipartContent.Add(new StringContent(field.Value), field.Key);
            }

            // Merge headers
            var mergedHeaders = MergeHeaders(request.Headers);

            return await Firefly.SendRequest(
                HttpMethod.Post,
                request.Url,
                mergedHeaders,
                multipartContent,
                false,
                _client
            );
        }

        /// <summary>
        /// Sends an HTTP request and returns the raw response stream.
        /// </summary>
        public async Task<Stream?> DownloadFileAsStream(HttpMethod method, string url, Dictionary<string, string>? headers = null, HttpContent? content = null)
        {
            return await Firefly.SendRequestAsStream(method, url, MergeHeaders(headers), content, _client);
        }

        /// <summary>
        /// Sends an HTTP request and returns a deserialized json response object.
        /// </summary>
        public async Task<TResponse?> SendRequest<TResponse>(HttpMethod method, string url, Dictionary<string, string>? headers = null, HttpContent? content = null)
        {
            return await Firefly.SendRequest<TResponse>(method, url, MergeHeaders(headers), content, _client);
        }
        
        /// <summary>
        /// Sends an HTTP request and returns a deserialized Xml response object.
        /// </summary>
        public async Task<TResponse?> SendRequestXml<TResponse>(HttpMethod method, string url, Dictionary<string, string>? headers = null, HttpContent? content = null)
        {
            return await Firefly.SendRequest<TResponse>(method, url, MergeHeaders(headers), content, _client, true);
        }
    }
}
