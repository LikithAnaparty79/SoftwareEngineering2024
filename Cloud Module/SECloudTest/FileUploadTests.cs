// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;

namespace FA1.Tests
{
    [TestClass]
    public class FileUploadTests
    {
        private const string BaseConnectionString = "UseDevelopmentStorage=true";
        private const string TestContainer = "testteam";
        private BlobContainerClient _containerClient;
        private ILogger<FileUpload> _logger;

        [TestInitialize]
        public async Task Initialize()
        {
            // Setup container client using Azurite connection string
            _containerClient = new BlobContainerClient(BaseConnectionString, TestContainer);
            await _containerClient.CreateIfNotExistsAsync();

            // Setup logger
            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<FileUpload>();

            // Set environment variable for the function
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", BaseConnectionString);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            // Delete test container after each test
            await _containerClient.DeleteIfExistsAsync();
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", null);
        }

        [TestMethod]
        public async Task TestSuccessfulFileUpload()
        {
            // Arrange
            var fileName = "test.txt";
            var fileContent = "Hello, World!";
            var context = CreateMockFunctionContext();
            var request = CreateMultipartFormDataRequest(fileName, fileContent);

            // Act
            var response = await FileUpload.UploadFile(request, TestContainer, context);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Verify file was uploaded to blob storage
            var blobClient = _containerClient.GetBlobClient(fileName);
            Assert.IsTrue(await blobClient.ExistsAsync());

            // Verify content
            var downloadedContent = await DownloadBlobContent(blobClient);
            Assert.AreEqual(fileContent, downloadedContent);
        }

        [TestMethod]
        public async Task TestInvalidContentType()
        {
            // Arrange
            var context = CreateMockFunctionContext();
            var request = CreateInvalidRequest();

            // Act
            var response = await FileUpload.UploadFile(request, TestContainer, context);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task TestEmptyFileName()
        {
            // Arrange
            var fileName = "";
            var fileContent = "Test Content";
            var context = CreateMockFunctionContext();
            var request = CreateMultipartFormDataRequest(fileName, fileContent);

            // Act
            var response = await FileUpload.UploadFile(request, TestContainer, context);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private FunctionContext CreateMockFunctionContext()
        {
            // Create a mock FunctionContext that returns our logger
            var context = new MockFunctionContext(_logger);
            return context;
        }

        private HttpRequestData CreateMultipartFormDataRequest(string fileName, string content)
        {
            var boundary = "---WebKitFormBoundary7MA4YWxkTrZu0gW";
            var multipartContent = new StringBuilder();
            multipartContent.AppendLine($"--{boundary}");
            multipartContent.AppendLine($"Content-Disposition: form-data; name=\"file\"; filename=\"{fileName}\"");
            multipartContent.AppendLine("Content-Type: text/plain");
            multipartContent.AppendLine();
            multipartContent.AppendLine(content);
            multipartContent.AppendLine($"--{boundary}--");

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(multipartContent.ToString()));
            var request = new MockHttpRequestData(
                new MockFunctionContext(_logger),
                new Uri("http://localhost/api/upload/testteam"),
                stream);

            request.Headers.Add("Content-Type", $"multipart/form-data; boundary={boundary}");
            return request;
        }

        private HttpRequestData CreateInvalidRequest()
        {
            var request = new MockHttpRequestData(
                new MockFunctionContext(_logger),
                new Uri("http://localhost/api/upload/testteam"),
                new MemoryStream());

            request.Headers.Add("Content-Type", "application/json");
            return request;
        }

        private async Task<string> DownloadBlobContent(BlobClient blobClient)
        {
            using var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0;
            using var reader = new StreamReader(memoryStream);
            return await reader.ReadToEndAsync();
        }
    }

    // Mock classes to support testing
    public class MockFunctionContext : FunctionContext
    {
        private readonly ILogger<FileUpload> _logger;

        public MockFunctionContext(ILogger<FileUpload> logger)
        {
            _logger = logger;
        }

        public override IServiceProvider InstanceServices => new MockServiceProvider(_logger);
        // Implement other required properties/methods...
        public override string InvocationId => throw new NotImplementedException();
        public override string FunctionId => throw new NotImplementedException();
        public override TraceContext TraceContext => throw new NotImplementedException();
        public override BindingContext BindingContext => throw new NotImplementedException();
        public override RetryContext RetryContext => throw new NotImplementedException();
        public override Features.IFunctionBindingsFeature BindingSourceFeature { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Features.IFunctionBindingsFeature BindingDefaultsFeature { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class MockServiceProvider : IServiceProvider
    {
        private readonly ILogger<FileUpload> _logger;

        public MockServiceProvider(ILogger<FileUpload> logger)
        {
            _logger = logger;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ILogger<FileUpload>))
                return _logger;
            return null;
        }
    }

    public class MockHttpRequestData : HttpRequestData
    {
        private readonly Stream _body;

        public MockHttpRequestData(FunctionContext functionContext, Uri url, Stream body)
            : base(functionContext)
        {
            Url = url;
            _body = body;
            Headers = new HttpHeadersCollection();
        }

        public override Stream Body => _body;
        public override HttpHeadersCollection Headers { get; }
        public override IReadOnlyCollection<IHttpCookie> Cookies => new List<IHttpCookie>();
        public override Uri Url { get; }
        public override IEnumerable<KeyValuePair<string, string>> Query => new List<KeyValuePair<string, string>>();

        public override string Method => "POST";

        public override HttpResponseData CreateResponse()
        {
            return new MockHttpResponseData(FunctionContext);
        }
    }

    public class MockHttpResponseData : HttpResponseData
    {
        private readonly MemoryStream _body = new MemoryStream();

        public MockHttpResponseData(FunctionContext functionContext)
            : base(functionContext)
        {
            Headers = new HttpHeadersCollection();
        }

        public override HttpStatusCode StatusCode { get; set; }
        public override HttpHeadersCollection Headers { get; set; }
        public override Stream Body => _body;
        public override HttpCookies Cookies => new MockHttpCookies();
    }

    public class MockHttpCookies : HttpCookies
    {
        public override void Append(string name, string value)
        {
            // Mock implementation
        }
    }
}
