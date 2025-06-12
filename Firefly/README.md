## 📦 **Package Name**
`FireflyHttp`

**FireflyHttp** is a lightweight and easy-to-use C# HTTP request library inspired by Python's `requests`. It simplifies HTTP calls with minimal configuration and supports JSON and XML requests.

## **Installation**

Install via NuGet:


```bash
dotnet add package FireflyHttp
```

## **How to use**

```using FireflyHttp;```

# GET Request

	var response = await Firefly.Get("https://httpbin.org/get");
	Console.WriteLine(response);


# GET Request with Headers

	var response = await Firefly.Get("https://httpbin.org/get", new { Authorization = "Bearer YOUR_TOKEN" });
	Console.WriteLine(response);


# POST Request (JSON)

	var data = new { name = "Firefly", version = "1.0" };
	var response = await Firefly.Post("https://httpbin.org/post", data);
	Console.WriteLine(response);


# PUT Request (JSON)

	var data = new { name = "Firefly", version = "2.0" };
	var response = await Firefly.Put("https://httpbin.org/put", data);
	Console.WriteLine(response);


# DELETE Request

	var response = await Firefly.Delete("https://httpbin.org/delete");
	Console.WriteLine(response);


# POST Request (XML)

	// using raw XML string payload
	var data = "<ExampleModel><Id>1</Id><Name>Firefly XML</Name></ExampleModel>";
	var response = await Firefly.PostXml("https://httpbin.org/post", data);
	Console.WriteLine(response);

	// using object to serialize to XML
	var data = new ExampleModel { Id = 1, Name = "Firefly XML" };
	var response = await Firefly.PostXml("https://httpbin.org/post", data);
	Console.WriteLine(response);

# Simplified JSON Call With Custome Response Type
	
	var client = new FireflyClient("https://api.example.com");
    var result = await client.SendJson<YourResponseType>(
    HttpMethod.Post,
    "/some-endpoint",
    new MyRequestType { Property = "value" });

# WebSocket Simple Connection

	var webSocket = await FireflyWebSocket.ConnectAsync("wss://example.com/socket");
    await webSocket.SendAsync("Hello from Firefly!"); // for text based events
	await webSocket.SendBinaryAsync(new byte[] { 1, 2, 3, 4 }); //for binary based events

	// event handling for incoming Text, Binary & Errors
	webSocket.OnMessageReceived += text => Console.WriteLine($"Received: {text}");
	webSocket.OnBinaryReceived += data => Console.WriteLine($"Received: {data}");
    webSocket.OnError += ex => Console.WriteLine($"Error: {ex.Message}");

# gRpc
	using FireflyGrpc;


	// Connect to gRPC Server
	var grpc = await FireflyGrpcClient<Greeter.GreeterClient>.ConnectAsync(
		"https://localhost:5001",
		channel => new Greeter.GreeterClient(channel)
	);

	// Unary Call (Simple Request-Response)
	var reply = await grpc.Client.SayHelloAsync(new HelloRequest { Name = "Firefly" });
	Console.WriteLine(reply.Message);

	// Server Streaming
	using var call = grpc.Client.StreamMessages(new StreamRequest { Name = "Firefly" });
	await foreach (var message in call.ResponseStream.ReadAllAsync())
	{
		Console.WriteLine($"Message: {message.Message}");
	}

	// Client Streaming
	using var call = grpc.Client.ClientStreamMessages();
	for (int i = 1; i <= 3; i++)
	{
		await call.RequestStream.WriteAsync(new ClientRequest { Message = $"Hello {i}" });
	}
	await call.RequestStream.CompleteAsync();

	var response = await call.ResponseAsync;
	Console.WriteLine($"Confirmation: {response.Confirmation}");

	// Bidirectional Streaming (Duplex) Chat functionality
	using var chat = grpc.Client.Chat();
	var sendingTask = Task.Run(async () =>
	{
		for (int i = 0; i < 3; i++)
		{
			await chat.RequestStream.WriteAsync(new ChatMessage { User = "Client", Message = $"Ping {i}" });
			await Task.Delay(1000);
		}
		await chat.RequestStream.CompleteAsync();
	});

	var receivingTask = Task.Run(async () =>
	{
		await foreach (var message in chat.ResponseStream.ReadAllAsync())
		{
			Console.WriteLine($"{message.User}: {message.Message}");
		}
	});

	await Task.WhenAll(sendingTask, receivingTask);




## **Features**

✔️ Simple and clean API for making HTTP requests.
✔️ Supports JSON and XML payloads.
✔️ Custom headers support.
✔️ Configurable request timeout.
✔️ Logging support.
✔️ WebSocket real-time events communications.
✔️ gRpc support.


## **License**

MIT License


## Changelog

### v1.1.0
- Support BaseAddress so users can configure it once instead of repeating full URLs.
- Support default headers to avoid setting them manually for every request.
- Change headers input parameter to `Dictionary<string, string>` for better usability.

### v1.2.0
- Added support for `MultipartFormDataContent`.
- Improved error handling.

### v1.3.0
- Added `DownloadFileAsStream` for handling stream response.
- Support automatic response deserialization (JSON & XML)
- Improved request logging.
- Optimized request handling for performance improvements.

### v2.0.0
- Added `FireflyWebSocket` for real-time (bi-directional) event communication with automatic reconnection.
- Handles text and binary events.
- Added optional logging support.

### v2.1.0
- Introduces update to Serializes your request object as JSON
- Sets Content-Type: application/json
- Deserializes the API response to `YourResponseType`
- Json and XML support `SendRequestXml<T>()` was introduced in v1.3.0 and still fully supported.

### v2.2.0
- Introduced `FireflyGrpcClient<TClient>` for gRPC client support.
- Connect to gRPC services with simplified API and support for strongly-typed service contracts.
- Supports unary and streaming methods using generated gRPC stubs.
- Minimal configuration, seamless integration alongside HTTP and WebSocket APIs.