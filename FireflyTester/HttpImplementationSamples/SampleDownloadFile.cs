using FireflyHttp;

namespace FireflyTester.HttpImplementationSamples
{
    public static class SampleDownloadFile
    {
        private static readonly string _baseUrl = "https://www.w3.org";
        private static readonly string _filePath = "downloaded_sample.txt";
        private static readonly FireflyClient _client = new FireflyClient(_baseUrl);
        public static async Task RunTests()
        {
            await TestFileDownload();
        }

        private static async Task TestFileDownload()
        {
            try
            {
                var headers = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer test_token" },
                    { "X-Custom-Header", "PostRequest" }
                };

                using var stream = await _client.DownloadFileAsStream(
                    HttpMethod.Get,
                    "/TR/PNG/iso_8859-1.txt",
                    headers
                );

                if (stream == null)
                {
                    throw new InvalidOperationException("Failed to download file.");
                }

                // Process the returned file stream. E.g save to disk or process in memory etc
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _filePath);
                using var fileStream = File.Create(filePath);
                await stream.CopyToAsync(fileStream);

                Console.WriteLine($"File downloaded successfully: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in TestFileDownload: " + ex.Message);                
            }
        }
    }
}
