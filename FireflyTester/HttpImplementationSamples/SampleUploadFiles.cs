using FireflyHttp;
using FireflyHttp.Dtos;

namespace FireflyTester.HttpImplementationSamples
{
    public static class SampleUploadFiles
    {
        private static string _baseUrl = "https://httpbin.org"; // Test server base 
        public static async Task RunTests()
        {
            await UploadMultipleFiles();
            await UploadFile();
        }


        public static async Task UploadFile()
        {
            Console.WriteLine("POST File Upload - Testing with single file type");

            // Define file path
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string textPath = Path.Combine(desktopPath, "testtext.txt");

            // Create dummy file            
            await File.WriteAllTextAsync(textPath, "This is a dummy TEXT content.");

            // Open file stream
            using var fileStream = File.OpenRead(textPath);


            // Make a post call to upload file
            var request = new PostFilesRequest
            {
                Url = "/post",
                Files = new List<FileData>
                {             
                    new()
                    {
                        FileStream = fileStream,
                        FileName = "test1.txt",
                        ContentType = "text/plain",
                        FieldName = "document1"
                    },
                },
                Headers = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer test-token" }
                },
                AdditionalFormFields = new Dictionary<string, string>
                {
                    { "description", "Uploading multiple file types for testing" },
                    { "category", "testing" }
                }
            };

            var client = new FireflyClient(_baseUrl);
            var response = await client.PostFiles(request);

            Console.WriteLine("Response:");
            Console.WriteLine(response);
        }

        public static async Task UploadMultipleFiles()
        {
            Console.WriteLine("POST File Upload - Testing with multiple file types");

            // Define file paths
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string pdfPath = Path.Combine(desktopPath, "testfile.pdf");
            string imagePath = Path.Combine(desktopPath, "testimage.jpg");
            string audioPath = Path.Combine(desktopPath, "testaudio.mp3");
            string videoPath = Path.Combine(desktopPath, "testvideo.mp4");
            string textPath = Path.Combine(desktopPath, "testtext.txt");

            // Create dummy files
            await File.WriteAllTextAsync(pdfPath, "This is a dummy PDF content.");
            await File.WriteAllTextAsync(imagePath, "This is a dummy JPEG image content.");
            await File.WriteAllTextAsync(audioPath, "This is a dummy MP3 content.");
            await File.WriteAllTextAsync(videoPath, "This is a dummy MP4 content.");
            await File.WriteAllTextAsync(textPath, "This is a dummy TEXT content.");

            // Open file streams
            using var pdfStream = File.OpenRead(pdfPath);
            using var imageStream = File.OpenRead(imagePath);
            using var audioStream = File.OpenRead(audioPath);
            using var videoStream = File.OpenRead(videoPath);
            using var fileStream = File.OpenRead(textPath);


            // Make a post call to upload multiple files
            var request = new PostFilesRequest
            {
                Url = "/post", 
                Files = new List<FileData>
                {
                    new()
                    {
                        FileStream = pdfStream,
                        FileName = "testfile.pdf",
                        ContentType = "application/pdf",
                        FieldName = "pdfDocument" // Custom field name
                    },
                    new()
                    {
                        FileStream = imageStream,
                        FileName = "testimage.jpg",
                        ContentType = "image/jpeg",
                        FieldName = "imageFile"
                    },
                    new()
                    {
                        FileStream = audioStream,
                        FileName = "testaudio.mp3",
                        ContentType = "audio/mpeg",
                        FieldName = "audioFile"
                    },
                    new()
                    {
                        FileStream = videoStream,
                        FileName = "testvideo.mp4",
                        ContentType = "video/mp4",
                        FieldName = "videoFile"
                    },
                    new()
                    {
                        FileStream = fileStream,
                        FileName = "test1.txt",
                        ContentType = "text/plain",
                        FieldName = "document1"
                    },
                },
                Headers = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer test-token" }
                },
                AdditionalFormFields = new Dictionary<string, string>
                {
                    { "description", "Uploading multiple file types for testing" },
                    { "category", "testing" }
                }
            };

            var client = new FireflyClient(_baseUrl);
            var response = await client.PostFiles(request);

            Console.WriteLine("Response:");
            Console.WriteLine(response);
        }
    }
}
