using System.Xml.Serialization;
using FireflyHttp;

namespace FireflyTester.ImplementationSamples
{
    public class SampleDeserializedResponse
    {
        public static async Task RunTests()
        {
            await TestRestfulRequest();
           // await TestXmlRequest();
        }
        private static async Task TestRestfulRequest()
        {
            var client = new FireflyClient("https://jsonplaceholder.typicode.com");

            // Add headers if needed
            client.SetDefaultHeaders(new Dictionary<string, string>
                {
                    { "User-Agent", "FireflyHttp-Test" },
                    { "X-Custom-Header", "DefaultHeader" }
                });

            try
            {
                // Authenticate if the endpoint needed it
                var headers = new Dictionary<string, string> { { "Authorization", "Bearer test_token" } };
                var response = await client.SendRequest<YourJsonResponseObject>(
                    HttpMethod.Get,
                    "/posts/1",
                    headers
                );

                Console.WriteLine($"Response for Restful Api call. Title: {response?.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Error in TestRestfulRequest");
            }
        }

        private static async Task TestXmlRequest()
        {
            var client = new FireflyClient("https://postman-echo.com");
            try
            {
                var xmlRequest = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>" +
                                 "<soapenv:Body>" +
                                 "<GetUserInfo><UserId>123</UserId></GetUserInfo>" +
                                 "</soapenv:Body></soapenv:Envelope>";

                var content = new StringContent(xmlRequest, System.Text.Encoding.UTF8, "application/xml");

                var response = await client.SendRequestXml<YourXmlResponseObject>(
                    HttpMethod.Post,
                    "/post",
                    null,
                    content
                );

                Console.WriteLine($"XML Response: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + " Error in TestXmlRequest");
            }
        }

        // Sample response model
        public class YourJsonResponseObject
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        // Sample XML response model

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class YourXmlResponseObject
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public SoapBody Body { get; set; }
        }

        public class SoapBody
        {
            [XmlElement(ElementName = "GetUserInfo")]
            public GetUserInfo GetUserInfo { get; set; }
        }

        public class GetUserInfo
        {
            [XmlElement(ElementName = "UserId")]
            public int UserId { get; set; }
        }
    }
}
