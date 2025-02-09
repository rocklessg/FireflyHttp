namespace FireflyTester
{
    public class SampleXmlRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        // Parameterless constructor is required for XML serialization
        public SampleXmlRequest() { }

        public SampleXmlRequest(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
