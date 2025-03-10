using FireflyHttp;

namespace FireflyTester.ImplementationSamples
{
    public static class BasicSampleUsage
    {
        public static async Task SimpleApiRequestTest()
        {
            // 1. GET Request (JSON)
            var getResponse = await Firefly.Get("https://jsonplaceholder.typicode.com/todos/1");
            Console.WriteLine("GET Response: " + getResponse);

            // 2. GET with Headers
            var getWithHeaders = await Firefly.Get("https://jsonplaceholder.typicode.com/posts/1",
                new { Authorization = "Bearer sample_token" });
            Console.WriteLine("GET with Headers Response: \u2705 Success! + getWithHeaders");

            // 3. POST Request (JSON)
            var postResponse = await Firefly.Post("https://jsonplaceholder.typicode.com/posts",
                new { title = "foo", body = "bar", userId = 1 });
            Console.WriteLine("POST Response: " + postResponse);

            // 4. PUT Request (JSON)
            var putResponse = await Firefly.Put("https://jsonplaceholder.typicode.com/posts/1",
                new { id = 1, title = "updated title", body = "updated body", userId = 1 });
            Console.WriteLine("PUT Response: " + putResponse);

            // 5. DELETE Request
            var deleteResponse = await Firefly.Delete("https://jsonplaceholder.typicode.com/posts/1");
            Console.WriteLine("DELETE Response: " + deleteResponse);


            Console.WriteLine("Testing Firefly XML Requests...\n");
            var xmlResponse = await Firefly.GetXml("https://www.w3schools.com/xml/note.xml");
            Console.WriteLine(xmlResponse);


            // 6i. POST XML Request (using C# Object)
            var xmlPostResponse = await Firefly.PostXml("https://httpbin.org/post",
                new SampleXmlRequest { Name = "John Doe", Age = 30 });
            Console.WriteLine("POST XML Response: " + xmlPostResponse);

            // 6ii. POST XML Request (using raw XML string)
            var rawXml = "<ExampleModel><Age>18</Age><Name>Firefly XML</Name></ExampleModel>";
            var response1 = await Firefly.PostXml("https://postman-echo.com/post", rawXml);
            Console.WriteLine("Raw XML Response: " + response1);

            // 7. PUT XML Request
            var xmlPutResponse = await Firefly.PutXml("https://httpbin.org/put",
                new SampleXmlRequest { Name = "Jane Doe", Age = 25 });
            Console.WriteLine("PUT XML Response: " + xmlPutResponse);

            // 8. DELETE XML Request
            var xmlDeleteResponse = await Firefly.DeleteXml("https://httpbin.org/delete",
                new SampleXmlRequest { Name = "John Doe", Age = 30 });
            Console.WriteLine("DELETE XML Response: " + xmlDeleteResponse);
        }
    }
}
