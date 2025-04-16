// See https://aka.ms/new-console-template for more information

using FireflyTester.HttpImplementationSamples;
using FireflyTester.WebSocketImplementationSamples;


Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Testing Firefly HTTP Requests...\n");

#region Http

//await AvancedSampleUsage.RunTests();
//await SampleDownloadFile.RunTests();
await SampleDeserializedResponse.RunTests();

#endregion

#region Web Socket

//await WebSocketSampleUsage.WithLogger();
//await WebSocketSampleUsage.WithoutLogger(); // test without logger

string wsUrl = "wss://echo.websocket.org"; // Use a test WebSocket server
var chatClient = new ChatClient();
//await chatClient.StartAsync(wsUrl);

#endregion
