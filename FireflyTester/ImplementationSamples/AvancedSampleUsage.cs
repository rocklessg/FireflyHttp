using FireflyHttp;

namespace FireflyTester.ImplementationSamples
{
    public static class AvancedSampleUsage
    {
        public static async Task RunTests()
        {
            var baseUrl = "https://httpbin.org"; // Test server to inspect requests
            var client = new FireflyClient(baseUrl);

            // Set default headers
            client.SetDefaultHeaders(new Dictionary<string, string>
            {
                { "User-Agent", "FireflyHttp-Test" },
                { "X-Custom-Header", "DefaultHeader" }
            });

            await TestGetRequest(client);
            await TestPostJsonWithHeaders(client);
            await TestPostXmlWithHeaders(client);
            await TestPutRequest(client);
            await TestDeleteRequest(client);
        }

        private static async Task TestGetRequest(FireflyClient client)
        {
            Console.WriteLine("\n🔹 Test: GET Request with Custom Headers");
            var headers = new Dictionary<string, string> { { "Authorization", "Bearer test_token" } };
            var response = await client.Get("/get", headers);
            Console.WriteLine(response);
        }

        private static async Task TestPostJsonWithHeaders(FireflyClient client)
        {
            Console.WriteLine("\n🔹 Test: POST Request with JSON and Multiple Headers");
            var data = new { Name = "TestUser", Age = 25 };
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer test_token" },
                { "X-Custom-Header", "PostRequest" }
            };
            var response = await client.Post("/post", data, headers);
            Console.WriteLine(response);
        }

        private static async Task TestPostXmlWithHeaders(FireflyClient client)
        {
            Console.WriteLine("\n🔹 Test: POST Request with XML");
            var xmlData = new SampleXmlRequest { Name = "Test", Age = 10 };
            var headers = new Dictionary<string, string>(); // Content-Type will be set automatically
            var response = await client.PostXml("/post", xmlData, headers);
            Console.WriteLine(response);
        }

        private static async Task TestPutRequest(FireflyClient client)
        {
            Console.WriteLine("\n🔹 Test: PUT Request");
            var data = new { Name = "UpdatedUser", Age = 30 };
            var headers = new Dictionary<string, string> { { "Authorization", "Bearer test_token" } };
            var response = await client.Put("/put", data, headers);
            Console.WriteLine(response);
        }

        private static async Task TestDeleteRequest(FireflyClient client)
        {
            Console.WriteLine("\n🔹 Test: DELETE Request with Headers");
            var headers = new Dictionary<string, string> { { "X-Delete-Header", "DeleteTest" } };
            var response = await client.Delete("/delete", headers);
            Console.WriteLine(response);
        } 
    }
}

