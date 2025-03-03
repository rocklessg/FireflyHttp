namespace FireflyHttp.Dtos
{
    public class PostFilesRequest
    {
        public string Url { get; set; } = string.Empty;
        public List<FileData> Files { get; set; } = new();
        public Dictionary<string, string>? Headers { get; set; }
        public Dictionary<string, string>? AdditionalFormFields { get; set; } // Optional form fields e.g { "description": "Profile picture" }
    }

    public class FileData
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FieldName { get; set; } = "file"; // E.g image, video, document, text .
    }
}
