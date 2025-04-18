﻿## 📦 **Package Name**
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


## **Features**

✔️ Simple and clean API for making HTTP requests.
✔️ Supports JSON and XML payloads.
✔️ Custom headers support.
✔️ Configurable request timeout.
✔️ Logging support.
✔️ WebSocket real-time events communications.


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