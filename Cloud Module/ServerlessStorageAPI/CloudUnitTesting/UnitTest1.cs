// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System.Net;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http;
using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using SECloud.Services;
using SECloud.Models;
using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace CloudUnitTesting;

public class Tests
{
    private var httpClient;
    private var logger;
    private string team = "testblobcontainer";
    private string sasToken = "";
    private const string BaseUrl = "https://secloudapp-2024.azurewebsites.net/api/";;

    [SetUp]
    public void Setup()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .AddHttpClient()
            .BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<CloudService>>();
        var httpClient = new HttpClient();
    }

    [Test]
    public async void ExceptionFileUploadTest()
    {
        logger.LogMessage("Testing for Internal Server Error in File Upload.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var content = "This is a test file content";
        var fileName = $"test-{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void InvalidContentTypeFileUploadTest()
    {
        logger.LogMessage("Uploading File with Invalid Content Type.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var content = "This is a test file content";
        var fileName = $"test-{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void InvalidFileSectionFileUploadTest()
    {
        logger.LogMessage("Uploading File with Invalid File Section.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var content = "This is a test file content";
        var fileName = $"test-{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void LargeFileUploadTest()
    {
        logger.LogMessage("Uploading Large File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );
        
        var size = 1024 * 1024 * 1024; // 5MB
        var buffer = new byte[size];
        new Random().NextBytes(buffer);
        
        var fileName = $"test-{Guid.NewGuid()}.bin";
        string filePath = "path/to/your/file.dat";
        using var stream = new MemoryStream(buffer);
        var response = await cloudService.UploadAsync(fileName, stream, "application/octet-stream");
        
        Assert.AreEqual(response.Success, true);
    }

    [Test]
    public async void ImageFileUploadTest()
    {
        logger.LogMessage("Uploading Image File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var imagePath = "path/to/your/image.jpg";
        using var stream = File.OpenRead(imagePath);
        var response = await cloudService.UploadAsync(fileName, stream, "image/jpeg");
        
        Assert.AreEqual(response.Success, true);
    }

    [Test]
    public async void EmptyFileUploadTest()
    {
        logger.LogMessage("Uploading Empty File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var content = "";
        var fileName = $"test-{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void EmptyFileNameUploadTest()
    {
        logger.LogMessage("Uploading File with no file name.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var content = "This is a test file content";
        var fileName = "";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void SuccessfulFileUploadTest()
    {
        logger.LogMessage("Uploading Regular File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );
        
        var content = "This is a test file content";
        var fileName = $"test-{Guid.NewGuid()}.txt";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
        
        Assert.AreEqual(response.Success, true);
    }

    [Test]
    public async void ConcurrentFileUploadTest()
    {
        logger.LogMessage("Uploading files concurrently.");
        var tasks = new List<Task<ServiceResponse<string>>>();
        for (int i = 0; i < 5; i++)
        {
            var content = $"Concurrent test file {i}";
            var fileName = $"concurrent-test-{i}-{Guid.NewGuid()}.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            tasks.Add(cloudService.UploadAsync(fileName, stream, "text/plain"));
        }
        var results = await Task.WhenAll(tasks);
        Assert.AreEqual(results.Success, true);
    }

    [Test]
    public async void InvalidFileNameDownloadTest()
    {
        logger.LogMessage("Downloading File with invalid file name.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = $"{Guid.NewGuid()}";
        var downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.AreEqual(downloadResponse.Success, false);
    }

    [Test]
    public async void EmptyConnectionStringFileDownloadTest()
    {
        logger.LogMessage("Downloading File with empty connection string.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = $"{Guid.NewGuid()}.txt";
        var downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.AreEqual(downloadResponse.Success, false);
    }

    [Test]
    public async void ExceptionFileDownloadTest()
    {
        logger.LogMessage("Testing for Internal Server Error in File Download.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = $"{Guid.NewGuid()}.txt";
        var downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.AreEqual(downloadResponse.Success, false);     
    }

    [Test]
    public async void NonExistentFileDownloadTest()
    {
        logger.LogMessage("Downloading Non-existent File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = $"{Guid.NewGuid()}.txt";
        var downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.AreEqual(downloadResponse.Success, false);
    }

    [Test]
    public async void SuccessfulFileDownloadTest()
    {
        logger.LogMessage("Downloading Regular File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = "peer.cu";
        var downloadResponse = await cloudService.DownloadAsync(blobName);
        Assert.AreEqual(downloadResponse.Success, true);
    }

    [Test]
    public async void SuccessfulConfigFileRetrievalTest()
    {
        logger.LogMessage("Getting Correct Config Setting.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string setting = "Theme";
        var response = await cloudService.RetrieveConfigAsync(setting);
        Assert.AreEqual(response.Success, true);
    }

    [Test]
    public async void NonExistentConfigFileRetrievalTest()
    {
        logger.LogMessage("Testing Config Retrieval with non existent config file.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string setting = "Theme";
        var response = await cloudService.RetrieveConfigAsync(setting);
        Assert.AreEqual(response.Success, true);
    }

    [Test]
    public async void NonExistentSettingConfigFileRetrievalTest()
    {
        logger.LogMessage("Testing Config Retrieval with non existent config setting.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string setting = $"{Guid.NewGuid()}";
        var response = await cloudService.RetrieveConfigAsync(setting);
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void ExceptionConfigFileRetrievalTest()
    {
        logger.LogMessage("Testing for Internal Server Error in Config File Retrieval.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string setting = "Theme";
        var response = await cloudService.RetrieveConfigAsync(setting);
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void SuccessfulFileUpdateTest()
    {
        logger.LogMessage("Updating Regular File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );
    }

    [Test]
    public async void SuccessfulFileDeleteTest()
    {
        logger.LogMessage("Deleting Regular File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = "vector_add.cu";
        var response = await cloudService.DeleteAsync(blobName);
        Assert.AreEqual(response.Success, true);
    }

    [Test]
    public async void InvalidFileNameDeleteTest()
    {
        logger.LogMessage("Deleting Regular File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        string blobName = $"{Guid.NewGuid()}";
        var response = await cloudService.DeleteAsync(blobName);
        Assert.AreEqual(response.Success, false);
    }

    [Test]
    public async void ListBlobsTest()
    {
        logger.LogMessage("Updating Regular File.");
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger
        );

        var response = await cloudService.ListBlobsAsync();
        Assert.AreEqual(response.Success, true);
    }
}
