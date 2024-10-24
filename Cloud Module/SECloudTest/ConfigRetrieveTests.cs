/******************************************************************************
* Filename    = ConfigRetrieveTests.cs
*
* Author      = Pranav Guruprasad Rao
*
* Product     = Cloud
* 
* Project     = Unnamed Software Project
*
* Description = Test Class for Config File Retrieval
*****************************************************************************/

using NUnit.Framework;
using Moq;
using Azure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using FA1;

[TestFixture]
public class ConfigRetrieveTests
{
    private Mock<BlobServiceClient> _mockBlobServiceClient;
    private Mock<BlobContainerClient> _mockContainerClient;
    private Mock<BlobClient> _mockBlobClient;
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<ILogger<ConfigRetrieve>> _mockLogger;
    private Mock<FunctionContext> _mockFunctionContext;
    private Mock<HttpRequestData> _mockRequest;

    private ConfigRetrieve _configRetrieve;

    [SetUp]
    public void SetUp()
    {
        // Initialize the mocks
        _mockBlobServiceClient = new Mock<BlobServiceClient>();
        _mockContainerClient = new Mock<BlobContainerClient>();
        _mockBlobClient = new Mock<BlobClient>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<ConfigRetrieve>>();
        _mockFunctionContext = new Mock<FunctionContext>();
        _mockRequest = new Mock<HttpRequestData>(_mockFunctionContext.Object);

        // Mock configuration retrieval for connection string
        _mockConfiguration.Setup(config => config["AzureWebJobsStorage"])
                          .Returns("UseDevelopmentStorage=true");

        // Mock blob container client and blob client creation
        _mockBlobServiceClient.Setup(client => client.GetBlobContainerClient(It.IsAny<string>()))
                              .Returns(_mockContainerClient.Object);
        _mockContainerClient.Setup(container => container.GetBlobClient(It.IsAny<string>()))
                            .Returns(_mockBlobClient.Object);

        // Initialize the ConfigRetrieve function class with mocked dependencies
        _configRetrieve = new ConfigRetrieve(_mockBlobServiceClient.Object, _mockLogger.Object, _mockConfiguration.Object);
    }

    [Test]
    public async Task GetConfigSetting_ConfigFileExists_ReturnsSettingValue()
    {
        // Arrange
        string testSetting = "testSetting";
        string configJson = "{\"testSetting\":\"testValue\"}";
        var content = BinaryData.FromString(configJson);

        _mockBlobClient.Setup(blob => blob.DownloadContentAsync())
                       .ReturnsAsync(Response.FromValue(BlobsModelFactory.BlobDownloadResult(content), Mock.Of<Response>()));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.OK))
                    .Returns(new Mock<HttpResponseData>(_mockFunctionContext.Object).Object);

        // Act
        HttpResponseData response = await _configRetrieve.GetConfigSetting(_mockRequest.Object, "team1", testSetting);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _mockLogger.Verify(l => l.LogInformation(It.Is<string>(s => s.Contains("Setting found"))), Times.Once);
    }

    /*[Test]
    public async Task GetConfigSetting_ConfigFileDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockBlobClient.Setup(blob => blob.DownloadContentAsync())
                       .ThrowsAsync(new RequestFailedException("Blob not found", (int)HttpStatusCode.NotFound));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.NotFound))
                    .Returns(new Mock<HttpResponseData>(_mockFunctionContext.Object).Object);

        // Act
        HttpResponseData response = await _configRetrieve.GetConfigSetting(_mockRequest.Object, "team1", "testSetting");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        _mockLogger.Verify(l => l.LogWarning(It.Is<string>(s => s.Contains("Config file not found"))), Times.Once);
    }*/

    [Test]
    public async Task GetConfigSetting_SettingNotFoundInConfig_ReturnsNotFound()
    {
        // Arrange
        string configJson = "{\"someOtherSetting\":\"someValue\"}";
        var content = BinaryData.FromString(configJson);

        _mockBlobClient.Setup(blob => blob.DownloadContentAsync())
                       .ReturnsAsync(Response.FromValue(BlobsModelFactory.BlobDownloadResult(content), Mock.Of<Response>()));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.NotFound))
                    .Returns(new Mock<HttpResponseData>(_mockFunctionContext.Object).Object);

        // Act
        HttpResponseData response = await _configRetrieve.GetConfigSetting(_mockRequest.Object, "team1", "nonexistentSetting");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        _mockLogger.Verify(l => l.LogWarning(It.Is<string>(s => s.Contains("Setting 'nonexistentSetting' not found"))), Times.Once);
    }

    [Test]
    public async Task GetConfigSetting_ExceptionThrown_ReturnsServerError()
    {
        // Arrange
        _mockBlobClient.Setup(blob => blob.DownloadContentAsync())
                       .ThrowsAsync(new Exception("Blob storage error"));

        _mockRequest.Setup(req => req.CreateResponse(HttpStatusCode.InternalServerError))
                    .Returns(new Mock<HttpResponseData>(_mockFunctionContext.Object).Object);

        // Act
        HttpResponseData response = await _configRetrieve.GetConfigSetting(_mockRequest.Object, "team1", "testSetting");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        _mockLogger.Verify(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }
}
