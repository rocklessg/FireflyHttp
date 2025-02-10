## 📦 **Package Name**
`FireflyHttp`

**FireflyHttp** is a lightweight and easy-to-use C# HTTP request library inspired by Python's `requests`. It simplifies HTTP calls with minimal configuration and supports JSON and XML requests.

## **Installation**

Install via NuGet:


```bash
dotnet add package FireflyHttp
```

## **Usage**


var response = await Firefly.Get(``"https://your-url.org/get"``);
Console.WriteLine(response);

```using FireflyHttp;```

# GET Request

var response = await Firefly.Get("https://httpbin.org/get");
Console.WriteLine(response);


# GET Request with Headers

var response = await Firefly.Get("https://httpbin.org/get", new { Authorization = "Bearer YOUR_TOKEN" });
Console.WriteLine(response);
POST Request (JSON)
csharp
Copy
Edit
var data = new { name = "Firefly", version = "1.0" };
var response = await Firefly.Post("https://httpbin.org/post", data);
Console.WriteLine(response);
PUT Request (JSON)
csharp
Copy
Edit
var data = new { name = "Firefly", version = "2.0" };
var response = await Firefly.Put("https://httpbin.org/put", data);
Console.WriteLine(response);
DELETE Request
csharp
Copy
Edit
var response = await Firefly.Delete("https://httpbin.org/delete");
Console.WriteLine(response);
POST Request (XML)
csharp
Copy
Edit
var data = new ExampleModel { Id = 1, Name = "Firefly XML" };
var response = await Firefly.PostXml("https://httpbin.org/post", data);
Console.WriteLine(response);
Features