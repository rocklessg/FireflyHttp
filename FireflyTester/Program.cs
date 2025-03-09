// See https://aka.ms/new-console-template for more information

using FireflyTester.ImplementationSamples;


Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Testing Firefly HTTP Requests...\n");

await AvancedSampleUsage.RunTests();