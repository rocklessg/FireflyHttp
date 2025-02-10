## 📦 **Package Name**
`FireflyHttp`

**FireflyHttp** is a lightweight and easy-to-use C# HTTP request library inspired by Python's `requests`. It simplifies HTTP calls with minimal configuration and supports JSON and XML requests.

## **Installation**

Install via NuGet:


```bash
dotnet add package FireflyHttp
```

## **Usage**

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


## **Features**

✔️ Simple and clean API for making HTTP requests.
✔️ Supports JSON and XML payloads.
✔️ Custom headers support.
✔️ Configurable request timeout.
✔️ Logging support.


## **License**

MIT License