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

        public async Task<string> Get(string url, Dictionary<string, string>? headers = null) =>
            await Firefly.SendRequest<object>(HttpMethod.Get, url, MergeHeaders(headers), default, false, _client);

        public async Task<string> Post<T>(string url, T data, Dictionary<string, string>? headers = null, bool isXml = false) =>
            await Firefly.SendRequest(HttpMethod.Post, url, MergeHeaders(headers), data, isXml, _client);

        public async Task<string> Put<T>(string url, T data, Dictionary<string, string>? headers = null, bool isXml = false) =>
            await Firefly.SendRequest(HttpMethod.Put, url, MergeHeaders(headers), data, isXml, _client);

        public async Task<string> Delete(string url, Dictionary<string, string>? headers = null) =>
            await Firefly.SendRequest<object>(HttpMethod.Delete, url, MergeHeaders(headers), default, false, _client);


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
    }
}
