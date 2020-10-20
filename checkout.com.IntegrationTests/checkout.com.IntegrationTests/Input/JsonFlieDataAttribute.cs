namespace Checkout.com.IntegrationTests.Input
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Xunit.Sdk;

    public class JsonFileDataAttribute : DataAttribute
    {
        private readonly string filePath;


        private const string inputString = "Input";

        private const string outputString = "ExpectedOutput";

        public JsonFileDataAttribute(string filePath)
        {
            this.filePath = filePath;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            var path = Path.IsPathRooted(filePath)
                ? filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            var fileData = File.ReadAllText(filePath);
            return GetData(fileData);
        }

        private IEnumerable<object[]> GetData(string jsonData)
        {
            var allData = JObject.Parse(jsonData);
            var inputDataString = allData[inputString].ToString();
            var outputDataString = allData[outputString].ToString();
            var inputData = JsonConvert.DeserializeObject(inputDataString, typeof(object));
            var outputData = JsonConvert.DeserializeObject(outputDataString, typeof(object));
            return new List<object[]> { new object[] { inputData,outputData } };
        }
    }
}