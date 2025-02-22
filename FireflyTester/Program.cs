// See https://aka.ms/new-console-template for more information

using FireflyTester;


Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Testing Firefly HTTP Requests...\n");

await FireflyHttp_v1_1_0_Test.RunTests();