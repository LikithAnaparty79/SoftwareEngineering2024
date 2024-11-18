/******************************************************************************
* Filename    = UnitTest.cs
*
* Author      = Pranav Guruprasad Rao
*
* Product     = Cloud
* 
* Project     = Unnamed Software Project
*
* Description = Unit Tests for Cloud
*****************************************************************************/

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http;
using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using SECloud.Services;
using SECloud.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CloudUnitTesting;

public class Tests
{
    public required HttpClient _httpClient;
    public required ILogger<CloudService> _logger;
    public string _team = "testblobcontainer";
    public string _sasToken = "";
    private const string BaseUrl = "https://secloudapp-2024.azurewebsites.net/api/";

    [SetUp]
    public void Setup()
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging(builder => {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .AddHttpClient()
            .BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<CloudService>>();
        _httpClient = new HttpClient();
    }

    [TearDown]
    public void Teardown()
    {
        _httpClient?.Dispose();
    }

    [Test]
    public async Task UploadAsync_ContentIsNull_ReturnsFailure()
    {
        // Arrange
        var cloudService = new CloudService(BaseUrl, _team, _sasToken, _httpClient, _logger);
        string blobName = "test.txt";
        string contentType = "text/plain";

        // Act
        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, null, contentType);

        // Assert
        Assert.That(response.Success, Is.False);
        Assert.That(response.Message, Is.EqualTo("The content stream is empty."));
    }

    [Test]
    public async Task UploadAsync_ContentLengthIsZero_ReturnsFailure()
    {
        // Arrange
        var cloudService = new CloudService(BaseUrl, _team, _sasToken, _httpClient, _logger);
        string blobName = "test.txt";
        string contentType = "text/plain";
        using var emptyStream = new MemoryStream();

        // Act
        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, emptyStream, contentType);

        // Assert
        Assert.That(response.Success, Is.False);
        Assert.That(response.Message, Is.EqualTo("The content stream is empty."));
    }

    [Test]
    public async Task UploadAsync_BlobNameWithoutExtension_ReturnsFailure()
    {
        // Arrange
        var cloudService = new CloudService(BaseUrl, _team, _sasToken, _httpClient, _logger);
        string blobName = "testfile"; // No extension
        string contentType = "text/plain";
        using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes("Sample content"));

        // Act
        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, contentStream, contentType);

        // Assert
        Assert.That(response.Success, Is.False);
    }

    [Test]
    public async Task UploadAsync_BlobNameWithUpperCaseExtension_ReturnsFailure()
    {
        // Arrange
        var cloudService = new CloudService(BaseUrl, _team, _sasToken, _httpClient, _logger);
        string blobName = "test.TXT";
        string contentType = "text/plain";
        using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes("Sample content"));

        // Act
        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, contentStream, contentType);

        Assert.That(response.Success, Is.False);
        // Assert
        //Assert.That(Path.GetExtension(blobName)?.TrimStart('.').ToLowerInvariant(), Is.EqualTo("txt"));
    }

    [Test]
    public async Task UploadAsync_BlobNameWithValidExtension_ReturnsSuccess()
    {
        // Arrange
        var cloudService = new CloudService(BaseUrl, _team, _sasToken, _httpClient, _logger);
        string blobName = "test.txt";
        string contentType = "text/plain";
        using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes("Sample content"));

        // Act
        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, contentStream, contentType);

        // Assert
        Assert.That(response.Success, Is.True); // Ensure additional logic is set up for upload testing
        //Assert.That(Path.GetExtension(blobName)?.TrimStart('.').ToLowerInvariant(), Is.EqualTo("txt"));
    }

    [Test]
    public async Task UploadFailsForUnsupportedContentType()
    {
        _logger.LogInformation("Testing upload with unsupported content type.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string blobName = "unsupported-file.xyz";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Test content"));

        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, stream, "application/unknown");

        Assert.That(response.Success, Is.False);
        Assert.That(response.Message, Is.EqualTo("Unsupported content type: application/unknown"));
    }
    /*
    [Test]
    public async Task ExceptionFileUploadTest()  // API testing
    {
        string dataUri = "data.txt";
        string content = "Here I am.";
        var request = new HttpRequestMessage(
            HttpMethod.Post, BaseUrl + $"/{_team}/{dataUri}") {
            Content = new StringContent(content)
        };
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        //response.EnsureSuccessStatusCode();

        Assert.That(response.IsSuccessStatusCode, Is.EqualTo(false));
    }*/

    [Test]
    public async Task InvalidContentTypeFileUploadTest()
    {
        _logger.LogInformation("Uploading File with Invalid Content Type.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string content = "This is a test file content";
        string fileName = $"test-{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "image/jpeg");
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task InvalidFileSectionFileUploadTest()
    {
        _logger.LogInformation("Uploading File with Invalid File Section.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        // Define the incorrect content type
        var content = new StringContent("This is test content", Encoding.UTF8, "application/json");

        // Send the request using HttpClient.PostAsync()
        HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/upload/{_team}", content);

        // Assert: Ensure the upload was unsuccessful with BadRequest
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        string responseContent = await response.Content.ReadAsStringAsync();
        Assert.That(responseContent, Is.EqualTo("Invalid content type. Expecting multipart/form-data."));

        /*string boundary = "---MyBoundary---";
        string requestBody = $"--{boundary}\r\n" +
                          "Content-Disposition: form-data; name=\"file\"; filename=\"test.txt\"\r\n" +
                          "Content-Type: text/plain\r\n" +
                          "\r\n" +
                          "This is a test file content\r\n" +
                          $"--{boundary}--\r\n" +
                          "invalid section\r\n";

        using var content = new StringContent(requestBody, Encoding.UTF8);
        content.Headers.Remove("Content-Type");
        content.Headers.Add("Content-Type", $"multipart/form-data; boundary={boundary}");
        // Send the request using HttpClient.PostAsync()
        HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/upload/{_team}", content);

        // Assert that the upload was unsuccessful
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));*/
    }

    [Test]
    public async Task UploadFailsWithErrorMessage()
    {
        _logger.LogInformation("Testing upload failure with error message.");

        // Mocking HttpClient to simulate a failed HTTP response
        var httpClientMock = new HttpClient(new HttpMessageHandlerMock((request, cancellationToken) => {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                ReasonPhrase = "Bad Request"
            };
            return Task.FromResult(responseMessage);
        }));

        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            httpClientMock,
            _logger
        );

        string blobName = "test-failure.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Test content"));

        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, stream, "text/plain");
        _logger.LogInformation($"success is {response.Success.ToString()} and message is {response.Message.ToString()}");

        Assert.That(response.Success, Is.False);
        //Assert.That(response.Message, Does.Contain("Upload failed: 400 - Bad Request"));
    }

    [Test]
    public async Task UploadHandlesExceptionGracefully()
    {
        _logger.LogInformation("Testing upload when an exception is thrown.");

        // Mocking HttpClient to throw an exception
        var httpClientMock = new HttpClient(new HttpMessageHandlerMock((request, cancellationToken) => {
            throw new HttpRequestException("Mocked exception");
        }));

        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            httpClientMock,
            _logger
        );

        string blobName = "test-exception.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Test content"));

        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, stream, "text/plain");

        Assert.That(response.Success, Is.False);
        Assert.That(response.Message, Does.Contain("An error occurred: Mocked exception"));
    }

    [Test]
    public async Task LargeFileUploadTest()
    {
        _logger.LogInformation("Uploading Large File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        int size = 5 * 1024 * 1024; // 5MB
        byte[] buffer = new byte[size];
        new Random().NextBytes(buffer);

        string fileName = $"test-{Guid.NewGuid()}.bin";
        //string filePath = "path/to/your/file.dat";
        using var stream = new MemoryStream(buffer);
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "application/octet-stream");

        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task ImageFileUploadTest()
    {
        _logger.LogInformation("Uploading Image File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        // Download the image from the provided URL
        string imageUrl = "https://picsum.photos/id/1/200/300";
        using var httpClient = new HttpClient();
        byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        using var stream = new MemoryStream(imageBytes);
        // Upload the downloaded image
        ServiceResponse<string> response = await cloudService.UploadAsync("image.jpg", stream, "image/jpg");
        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task EmptyFileUploadTest()
    {
        _logger.LogInformation("Uploading Empty File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        // string content = null;
        string fileName = $"test-{Guid.NewGuid()}.txt";
        File.CreateText(fileName).Close();
        //string content = File.ReadAllText(fileName);
        using var stream = new MemoryStream();
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task EmptyFileNameUploadTest()
    {
        _logger.LogInformation("Uploading File with no file name.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string content = "This is a test file content";
        string fileName = "";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "text/plain");

        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task SuccessfulFileUploadTest()
    {
        _logger.LogInformation("Uploading Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string content = "This is a test file content";
        string fileName = $"test-1.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "text/plain");

        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task ConcurrentFileUploadTest()
    {
        _logger.LogInformation("Uploading files concurrently.");
        _logger.LogInformation("Uploading Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        var tasks = new List<Task<ServiceResponse<string>>>();
        for (int i = 0; i < 5; i++)
        {
            string content = $"Concurrent test file {i}";
            string fileName = $"concurrent-test-{i}-{Guid.NewGuid()}.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            tasks.Add(cloudService.UploadAsync(fileName, stream, "text/plain"));
        }
        ServiceResponse<string>[] results = await Task.WhenAll(tasks);

        foreach (ServiceResponse<string> result in results)
        {
            _logger.LogInformation($"test result: {result.Message.ToString()}");
            Assert.That(result.Success, Is.EqualTo(true));
        }

    }

    [Test]
    public async Task InvalidFileNameDownloadTest()
    {
        _logger.LogInformation("Downloading File with invalid file name.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string blobName = $"{Guid.NewGuid()}";
        ServiceResponse<System.IO.Stream> downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.That(downloadResponse.Success, Is.EqualTo(false));
    }

    /*
    [Test]
    public async Task ExceptionFileDownloadTest()
    {
        _logger.LogInformation("Testing for Internal Server Error in File Download.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string blobName = $"{Guid.NewGuid()}.txt";
        ServiceResponse<System.IO.Stream> downloadResponse = await cloudService.DownloadAsync(blobName);
    }*/

    [Test]
    public async Task NonExistentFileDownloadTest()
    {
        _logger.LogInformation("Downloading Non-existent File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string blobName = $"{Guid.NewGuid()}.txt";
        ServiceResponse<System.IO.Stream> downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.That(downloadResponse.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task SuccessfulFileDownloadTest()
    {
        _logger.LogInformation("Downloading Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string content = "This is a test file content";
        string blobName = $"test-1.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ServiceResponse<string> response = await cloudService.UploadAsync(blobName, stream, "text/plain");

        ServiceResponse<System.IO.Stream> downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.That(downloadResponse.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task SuccessfulConfigFileRetrievalTest()
    {
        _logger.LogInformation("Getting Correct Config Setting.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string setting = "Theme";
        ServiceResponse<string> response = await cloudService.RetrieveConfigAsync(setting);
        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task NonExistentConfigFileRetrievalTest()
    {
        _logger.LogInformation("Testing Config Retrieval with non existent config file.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string setting = "Value";
        ServiceResponse<string> response = await cloudService.RetrieveConfigAsync(setting);
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task NonExistentSettingConfigFileRetrievalTest()
    {
        _logger.LogInformation("Testing Config Retrieval with non existent config setting.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string setting = $"{Guid.NewGuid()}";
        ServiceResponse<string> response = await cloudService.RetrieveConfigAsync(setting);
        Assert.That(response.Success, Is.EqualTo(false));
    }

    /*[Test]
    public async Task ExceptionConfigFileRetrievalTest()
    {
        _logger.LogInformation("Testing for Internal Server Error in Config File Retrieval.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string setting = "Theme";
        ServiceResponse<string> response = await cloudService.RetrieveConfigAsync(setting);
        Assert.That(response.Success, Is.EqualTo(false));
    }*/

    [Test]
    public async Task SuccessfulFileUpdateTest()
    {
        _logger.LogInformation("Updating Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string contentType = "text/plain";

        string content = "This is the updated version of the test file content";
        string fileName = $"test-1.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        ServiceResponse<string> response = await cloudService.UpdateAsync(fileName, stream, contentType);
        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task InvalidContainerNameUpdateTest()
    {
        _logger.LogInformation("Updating Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            "wrong_team",
            _sasToken,
            _httpClient,
            _logger
        );

        string contentType = "text/plain";

        string content = "This is the updated version of the test file content";
        string fileName = $"{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        ServiceResponse<string> response = await cloudService.UpdateAsync(fileName, stream, contentType);
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task SuccessfulFileDeleteTest()
    {
        _logger.LogInformation("Deleting Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string content = "TO BE DELETED";
        string fileName = $"test-2.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "text/plain");

        if (response.Success == true)
        {
            ServiceResponse<bool> deleteResponse = await cloudService.DeleteAsync(fileName);
            Assert.That(deleteResponse.Success, Is.EqualTo(true));
        }
    }

    [Test]
    public async Task InvalidFileNameDeleteTest()
    {
        _logger.LogInformation("Deleting Regular File.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        string blobName = $"{Guid.NewGuid()}";
        ServiceResponse<bool> response = await cloudService.DeleteAsync(blobName);
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task ListBlobsTest()
    {
        _logger.LogInformation("Listing Blobs in the container.");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        ServiceResponse<System.Collections.Generic.List<string>> response = await cloudService.ListBlobsAsync();
        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task InvalidContainerListBlobsTest()
    {
        _logger.LogInformation("Listing Blobs in the container.");
        var cloudService = new CloudService(
            BaseUrl,
            "",
            _sasToken,
            _httpClient,
            _logger
        );

        ServiceResponse<System.Collections.Generic.List<string>> response = await cloudService.ListBlobsAsync();
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task ListBlobsAsync_BlobListResponseIsNull_ReturnsEmptyList()
    {
        // Arrange: Mock HttpClient to return a successful response with null content
        var httpClientMock = new HttpClient(new HttpMessageHandlerMock((request, cancellationToken) => {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };
            return Task.FromResult(responseMessage);
        }));

        var cloudService = new CloudService(BaseUrl, _team, _sasToken, httpClientMock, _logger);

        // Act: Call the method
        ServiceResponse<List<string>> response = await cloudService.ListBlobsAsync();

        // Assert
        Assert.That(response.Success, Is.True);
        Assert.That(response.Data, Is.Null); // Null because blobListResponse.Blobs was null
        /*Mock.Get(_logger).Verify(
            x => x.LogInformation("Successfully listed blobs. Found {Count} blobs", 0),
            Times.Once
        );*/
    }

    [Test]
    public async Task EmptyContainerListBlobsTest()
    {
        _logger.LogInformation("Listing Blobs in the container.");
        var cloudService = new CloudService(
            BaseUrl,
            "wrong-container",
            _sasToken,
            _httpClient,
            _logger
        );

        ServiceResponse<System.Collections.Generic.List<string>> response = await cloudService.ListBlobsAsync();
        Assert.That(response.Data, Is.Empty);
    }

    [Test]
    public async Task SuccessfulJsonSearchTest()
    {
        _logger.LogInformation("Search for JSON files in the container");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        WeatherForecast weather = new WeatherForecast {
            Date = DateTime.Now,
            TemperatureCelsius = 32,
            Summary = "Sunny"
        };
        string fileName = "WeatherForecast.json";
        await using (FileStream createStream = File.Create(fileName))
        {
            await JsonSerializer.SerializeAsync(createStream, weather);
        }

        // Open the file for reading to prepare for upload
        await using FileStream uploadStream = File.OpenRead(fileName);

        // Upload the file to the cloud with "application/json" as the content type
        ServiceResponse<string> upload_response = await cloudService.UploadAsync("weather.json", uploadStream, "application/json");

        // Now, search the uploaded JSON file in the cloud
        string key = "TemperatureCelsius";
        string value = "32";
        ServiceResponse<SECloud.Models.JsonSearchResponse> response = await cloudService.SearchJsonFilesAsync(key, value);

        // Assert: Check that the search was successful
        Assert.That(response.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task EmptySearchKeyJsonSearchTest()
    {
        _logger.LogInformation("Search for JSON files in the container");
        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            _httpClient,
            _logger
        );

        /*WeatherForecast weather = new WeatherForecast {
            Date = DateTime.Now,
            TemperatureCelsius = 32,
            Summary = "Sunny"
        };
        string fileName = "WeatherForecast.json";
        await using (FileStream createStream = File.Create(fileName))
        {
            await JsonSerializer.SerializeAsync(createStream, weather);
        */

        // Open the file for reading to prepare for upload
        //await using FileStream uploadStream = File.OpenRead(fileName);

        // Upload the file to the cloud with "application/json" as the content type
        //ServiceResponse<string> upload_response = await cloudService.UploadAsync("weather.json", uploadStream, "application/json");

        // Now, search the uploaded JSON file in the cloud
        string key = "";
        string value = "32";
        ServiceResponse<SECloud.Models.JsonSearchResponse> response = await cloudService.SearchJsonFilesAsync(key, value);

        // Assert: Check that the search was successful
        Assert.That(response.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task InvalidContainerNameJsonSearchTest()
    {
        _logger.LogInformation("Search for JSON files in the container");
        var cloudService = new CloudService(
            BaseUrl,
            "wrong-container1",
            _sasToken,
            _httpClient,
            _logger
        );

        string content = "This is a test file content";
        string fileName = $"test-1.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ServiceResponse<string> response = await cloudService.UploadAsync(fileName, stream, "application/json");

        string content1 = "This is a test file content";
        string fileName1 = $"test-2.txt";
        using var stream1 = new MemoryStream(Encoding.UTF8.GetBytes(content1));
        ServiceResponse<string> response1 = await cloudService.UploadAsync(fileName1, stream1, "application/json");

        string content2 = "This is a test file content";
        string fileName2 = $"test-3.txt";
        using var stream2 = new MemoryStream(Encoding.UTF8.GetBytes(content2));
        ServiceResponse<string> response2 = await cloudService.UploadAsync(fileName2, stream2, "application/json");

        // Now, search the uploaded JSON file in the cloud
        string key = "";
        string value = "32";
        ServiceResponse<JsonSearchResponse> response3 = await cloudService.SearchJsonFilesAsync(key, value);
        _logger.LogInformation(response3.Message.ToString());
        // Assert: Check that the search was successful
        Assert.That(response3.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task SearchJsonFilesAsync_FailsWithErrorMessage()
    {
        _logger.LogInformation("Testing JSON search failure scenario.");

        // Mocking HttpClient to simulate a failed HTTP response
        var httpClientMock = new HttpClient(new HttpMessageHandlerMock((request, cancellationToken) => {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                ReasonPhrase = "Bad Request"
            };
            return Task.FromResult(responseMessage);
        }));

        var cloudService = new CloudService(
            BaseUrl,
            _team,
            _sasToken,
            httpClientMock,
            _logger
        );

        string searchKey = "testKey";
        string searchValue = "testValue";

        // Act: Call the method
        ServiceResponse<JsonSearchResponse> response = await cloudService.SearchJsonFilesAsync(searchKey, searchValue);

        // Assert: Verify the response
        Assert.That(response.Success, Is.False);
        Assert.That(response.Message, Does.Contain("Search failed: Bad Request"));

        // Assert: Verify the log entry (using a logger mock)
        /*var loggerMock = Mock.Get(_logger); // Assuming a mock logger is used
        loggerMock.Verify(
            x => x.LogError("JSON search failed. Status: {StatusCode}, Reason: {Reason}", HttpStatusCode.BadRequest, "Bad Request"),
            Times.Once
        );*/
    }

    [Test]
    public async Task SearchJsonFilesAsync_SearchResponseIsNull_ReturnsSuccessWithZeroMatches()
    {
        // Arrange: Mock HttpClient to return a valid response with null content
        var httpClientMock = new HttpClient(new HttpMessageHandlerMock((request, cancellationToken) => {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };
            return Task.FromResult(responseMessage);
        }));

        var cloudService = new CloudService(BaseUrl, _team, _sasToken, httpClientMock, _logger);

        // Act: Call the method
        ServiceResponse<JsonSearchResponse> response = await cloudService.SearchJsonFilesAsync("key", "value");

        // Assert
        Assert.That(response.Success, Is.True);
        Assert.That(response.Data, Is.Null);
        /*Mock.Get(_logger).Verify(
            x => x.LogInformation("Successfully completed JSON search. Found {MatchCount} matches", 0),
            Times.Once
        );*/
    }

}

public class HttpMessageHandlerMock : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

    public HttpMessageHandlerMock(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _handler(request, cancellationToken);
    }
}

public class WeatherForecast
{
    public DateTimeOffset Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string? Summary { get; set; }
}
