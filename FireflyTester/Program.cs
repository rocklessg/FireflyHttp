// See https://aka.ms/new-console-template for more information

using FireflyTester.HttpImplementationSamples;
using FireflyTester.WebSocketImplementationSamples;


Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Testing Firefly HTTP Requests...\n");

//await AvancedSampleUsage.RunTests();
//await SampleDownloadFile.RunTests();
//await SampleDeserializedResponse.RunTests();

//Websocket flow
await WebSocketSampleUsage.WithLogger();
//await WebSocketSampleUsage.WithoutLogger(); // Uncomment to test without logger
    