/******************************************************************************
* Filename    = FileDownloadTests.cs
*
* Author      = Pranav Guruprasad Rao
*
* Product     = Cloud
* 
* Project     = Unnamed Software Project
*
* Description = Test Class for File Download
*****************************************************************************/

using NUnit.Framework;
using Moq;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FA1;
using Azure.Storage.Blobs.Models;

[TestFixture]
public class FileDownloadTests
{
    private Mock<FunctionContext> _mockContext;
    private Mock<HttpRequestData> _mockRequest;
    private Mock<BlobClient> _mockBlobClient;
    private Mock<HttpResponseData> _mockResponse;
    private Mock<ILogger<FileDownload>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<FunctionContext>();
        _mockRequest = new Mock<HttpRequestData>(_mockContext.Object);
        _mockBlobClient = new Mock<BlobClient>();
        _mockResponse = new Mock<HttpResponseData>(_mockContext.Object);
        _mockLogger = new Mock<ILogger<FileDownload>>();
    }

    [Test]
    public async Task DownloadFile_FileExists_ReturnsOkWithFile()
    {
        // Arrange
        var fileDownload = new FileDownload();

        _mockBlobClient
            .Setup(b => b.DownloadAsync())
            .ReturnsAsync(Response.FromValue(
                BlobsModelFactory.BlobDownloadInfo(
                    content: new MemoryStream(),
                    contentType: "application/octed-stream", /*new BlobHttpHeaders { ContentType = "application/octet-stream" },*/
                    contentLength: 1024,
                    contentHash: null,
                    lastModified: DateTimeOffset.UtcNow,
                    blobType: BlobType.Block,
                    leaseState: LeaseState.Available,
                    leaseStatus: LeaseStatus.Unlocked,
                    metadata: new System.Collections.Generic.Dictionary<string, string>()
                ),
                Mock.Of<Response>()));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.OK))
                    .Returns(new Mock<HttpResponseData>(_mockContext.Object).Object);

        // Act
        HttpResponseData response = await FileDownload.DownloadFile(_mockRequest.Object, "team1", "Schedule.txt", _mockContext.Object);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DownloadFile_FileDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var fileDownload = new FileDownload();

        _mockBlobClient
            .Setup(b => b.DownloadAsync())
            .ThrowsAsync(new RequestFailedException("Blob not found"));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.NotFound))
                    .Returns(new Mock<HttpResponseData>(_mockContext.Object).Object);

        // Act
        HttpResponseData response = await FileDownload.DownloadFile(_mockRequest.Object, "team1", "hello.txt", _mockContext.Object);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DownloadFile_ExceptionDuringDownload_ReturnsInternalServerError()
    {
        // Arrange
        var fileDownload = new FileDownload();

        _mockBlobClient
            .Setup(b => b.DownloadAsync())
            .ThrowsAsync(new Exception("Download error"));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.InternalServerError))
                    .Returns(new Mock<HttpResponseData>(_mockContext.Object).Object);

        // Act
        HttpResponseData response = await FileDownload.DownloadFile(_mockRequest.Object, "team1", "file1.txt", _mockContext.Object);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task EmptyConnectionStringTest_ReturnsInternalServerError()
    {
        // Arrange
        var fileDownload = new FileDownload();

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.InternalServerError))
                    .Returns(new Mock<HttpResponseData>(_mockContext.Object).Object);

        // Act
        HttpResponseData response = await FileDownload.DownloadFile(_mockRequest.Object, "team1", "Schedule.txt", _mockContext.Object);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}
