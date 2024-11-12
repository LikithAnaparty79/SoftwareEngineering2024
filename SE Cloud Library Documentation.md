# SE Cloud Library Documentation

## Overview
This documentation covers the implementation and usage of seven core functions for interacting with Azure Blob Storage through a custom Cloud Service interface. The service is designed to handle basic blob storage operations with proper logging and error handling.

## Prerequisites
- .NET Core/.NET 5+ SDK
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Http
- Microsoft.Extensions.Logging
- Microsoft.Extensions.Logging.Console
- System.Net.Http


## Base Configuration
All examples use the following base configuration:
```csharp
var baseUrl = "https://secloudapp-2024.azurewebsites.net/api";
var team = "testblobcontainer";  // Your container name
var sasToken = "";  // Your SAS token
```

## 1. Upload Function
### Function Signature
```csharp
public async Task<ServiceResponse<string>> UploadAsync(
    string fileName,
    Stream content,
    string contentType)
```

### Purpose
Uploads files to Azure Blob Storage with support for different file types and sizes.

### Features
- Text file upload
- Image file upload
- Large file upload (5MB+)
- Concurrent multiple file uploads

### Return Type
- Returns `ServiceResponse<string>` where the string contains the URL of the uploaded blob
- Success case returns the blob URL
- Failure case returns error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": "https://storage.blob.core.windows.net/container/example.txt",
    "Message": "Successfully uploaded example.txt"
}

// Failure case
{
    "Success": false,
    "Data": null,
    "Message": "Upload failed: 400 - Invalid file name"
}
```

### Usage Example
```csharp
// Single file upload
var fileName = $"test-{Guid.NewGuid()}.txt";
using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
var response = await cloudService.UploadAsync(fileName, stream, "text/plain");
```

### Test Example
```csharp

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using SECloud.Services;
using SECloud.Models;
using System;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup dependency injection
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            })
            .AddHttpClient()
            .BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<CloudService>>();
        var httpClient = new HttpClient(); // Simplified for testing

        // Configuration values - replace with your actual values
        var baseUrl = "https://secloudapp-2024.azurewebsites.net/api";
        var team = "testblobcontainer";
        var sasToken = "";

        // Create CloudService instance
        var cloudService = new CloudService(
            baseUrl,
            team,
            sasToken,
            httpClient,
            logger);

        try
        {
            // Test 1: Upload a text file
            await TestTextFileUpload(cloudService);

            // Test 2: Upload an image file
            //await TestImageFileUpload(cloudService);

            // Test 3: Upload a large file
            //await TestLargeFileUpload(cloudService);

            // Test 4: Upload multiple files concurrently
            //await TestConcurrentUploads(cloudService);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Tests failed: {ex.Message}");
            logger.LogError(ex, "Error during test execution");
        }
    }

    static async Task TestTextFileUpload(CloudService cloudService)
    {
        Console.WriteLine("\nTesting text file upload...");

        var content = "This is a test file content";
        var fileName = $"test-{Guid.NewGuid()}.txt";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var response = await cloudService.UploadAsync(fileName, stream, "text/plain");

        PrintResult<string>("Text file upload", response);
    }

    static async Task TestImageFileUpload(CloudService cloudService)
    {
        Console.WriteLine("\nTesting image file upload...");

        // Replace with path to an actual test image
        var imagePath = "test-image.jpg";
        if (!File.Exists(imagePath))
        {
            Console.WriteLine($"Test image not found at {imagePath}. Skipping image upload test.");
            return;
        }

        using var stream = File.OpenRead(imagePath);
        var response = await cloudService.UploadAsync(
            Path.GetFileName(imagePath),
            stream,
            "image/jpeg");

        PrintResult<string>("Image file upload", response);
    }

    static async Task TestLargeFileUpload(CloudService cloudService)
    {
        Console.WriteLine("\nTesting large file upload...");

        // Create a 5MB test file
        var size = 5 * 1024 * 1024; // 5MB
        var buffer = new byte[size];
        new Random().NextBytes(buffer);

        var fileName = $"large-file-{Guid.NewGuid()}.bin";
        using var stream = new MemoryStream(buffer);

        var response = await cloudService.UploadAsync(fileName, stream, "application/octet-stream");

        PrintResult<string>("Large file upload", response);
    }

    static async Task TestConcurrentUploads(CloudService cloudService)
    {
        Console.WriteLine("\nTesting concurrent uploads...");

        var tasks = new List<Task<ServiceResponse<string>>>();

        // Create 5 concurrent upload tasks
        for (int i = 0; i < 5; i++)
        {
            var content = $"Concurrent test file {i}";
            var fileName = $"concurrent-test-{i}-{Guid.NewGuid()}.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            tasks.Add(cloudService.UploadAsync(fileName, stream, "text/plain"));
        }

        var results = await Task.WhenAll(tasks);

        for (int i = 0; i < results.Length; i++)
        {
            PrintResult<string>($"Concurrent upload {i}", results[i]);
        }
    }

    static void PrintResult<T>(string testName, ServiceResponse<T> response)
    {
        Console.WriteLine($"{testName} result:");
        Console.WriteLine($"Success: {response.Success}");
        Console.WriteLine($"Message: {response.Message}");
        if (response.Data != null)
        {
            Console.WriteLine($"Data: {response.Data}");
        }
        Console.WriteLine();
    }
}
```

## 2. Download Function
### Function Signature
```csharp
public async Task<ServiceResponse<Stream>> DownloadAsync(
    string blobName)
```

### Purpose
Downloads files from Azure Blob Storage and saves them locally.

### Features
- Stream-based download
- Automatic local file creation
- Error handling for missing blobs

### Return Type
- Returns `ServiceResponse<Stream>` containing the downloaded file content
- Success case returns the file stream
- Failure case returns null stream with error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": <MemoryStream>, // Contains file content
    "Message": null
}

// Failure case
{
    "Success": false,
    "Data": null,
    "Message": "Download failed: Blob not found"
}
```

### Usage Example
```csharp
string blobName = "example.txt";
var downloadResponse = await cloudService.DownloadAsync(blobName);
if (downloadResponse.Success)
{
    var downloadPath = Path.Combine(Directory.GetCurrentDirectory(), blobName);
    using (var fileStream = new FileStream(downloadPath, FileMode.Create))
    {
        await downloadResponse.Data.CopyToAsync(fileStream);
    }
}
```

### Test Example
```csharp

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SECloud.Interfaces;
using SECloud.Services;

namespace SECloudClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up dependency injection for logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => {
                    config.AddConsole();
                    config.SetMinimumLevel(LogLevel.Debug);
                })
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<CloudService>>();

            // Set up HttpClient and CloudService
            var httpClient = new HttpClient();
            var baseUrl = "https://secloudapp-2024.azurewebsites.net/api";
            var team = "testblobcontainer";  // Replace with your actual container name
            var sasToken = ""; // Replace with your actual SAS token
            var cloudService = new CloudService(baseUrl, team, sasToken, httpClient, logger);

            // Blob to download
            string blobName = "peer.cu";

            // Log download initiation
            logger.LogInformation("Starting download for blob: {BlobName}", blobName);

            // Call DownloadAsync method
            var downloadResponse = await cloudService.DownloadAsync(blobName);

            // Check download result
            if (downloadResponse.Success)
            {
                logger.LogInformation("Download succeeded for blob: {BlobName}", blobName);

                // Save downloaded stream to a local file
                var downloadPath = Path.Combine(Directory.GetCurrentDirectory(), blobName);
                using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
                {
                    await downloadResponse.Data.CopyToAsync(fileStream);
                }

                logger.LogInformation("Downloaded blob saved to: {DownloadPath}", downloadPath);
            }
            else
            {
                logger.LogError("Download failed: {Message}", downloadResponse.Message);
                Console.WriteLine("Additional info: Check blob name, container permissions, and network connectivity.");
            }

            Console.WriteLine("Download test complete.");
        }
    }
}
```

## 3. Retrieve Configuration Function
### Function Signature
```csharp
public async Task<ServiceResponse<string>> RetrieveConfigAsync(
    string setting)
```

### Purpose
Retrieves configuration settings from the cloud storage.

### Features
- Key-based configuration retrieval
- Structured response handling

### Return Type
- Returns `ServiceResponse<string>` containing the configuration value
- Success case returns the configuration setting value
- Failure case returns error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": "Dark",  // Configuration value
    "Message": null
}

// Failure case
{
    "Success": false,
    "Data": null,
    "Message": "Config retrieval failed: Setting not found"
}
```

### Usage Example
```csharp
string setting = "Theme";
var response = await cloudService.RetrieveConfigAsync(setting);
```

### Test Example
```csharp

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SECloud.Services;
using SECloud.Models;

namespace SECloudClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<CloudService>>();

            var httpClient = new HttpClient();
            var cloudService = new CloudService("https://secloudapp-2024.azurewebsites.net/api", "testblobcontainer", "", httpClient, logger);

            string setting = "Theme";
            var response = await cloudService.RetrieveConfigAsync(setting);
            if (response.Success)
                logger.LogInformation("Retrieved config: {ConfigData}", response.Data);
            else
                logger.LogError("Config retrieval failed: {Message}", response.Message);
        }
    }
}
```

## 4. Delete Function
### Function Signature
```csharp
public async Task<ServiceResponse<bool>> DeleteAsync(
    string blobName)
```

### Purpose
Removes specified blobs from the storage container.

### Features
- Single blob deletion
- Success/failure status reporting

### Return Type
- Returns `ServiceResponse<bool>` indicating deletion success
- Success case returns true
- Failure case returns false with error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": true,
    "Message": null
}

// Failure case
{
    "Success": false,
    "Data": false,
    "Message": "Delete failed: Blob not found"
}
```

### Usage Example
```csharp
string blobName = "file-to-delete.txt";
var response = await cloudService.DeleteAsync(blobName);
```

### Test Example
```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SECloud.Services;
using SECloud.Models;

namespace SECloudClient;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(config => config.AddConsole().SetMinimumLevel(LogLevel.Debug))
            .BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<CloudService>>();

        var httpClient = new HttpClient();
        var cloudService = new CloudService("https://secloudapp-2024.azurewebsites.net/api", "testblobcontainer", "", httpClient, logger);

        string blobName = "vector_add.cu";
        var response = await cloudService.DeleteAsync(blobName);
        if (response.Success)
            logger.LogInformation("Delete succeeded for blob: {BlobName}", blobName);
        else
            logger.LogError("Delete failed: {Message}", response.Message);
    }
}
```

## 5. List Blobs Function
### Function Signature
```csharp
public async Task<ServiceResponse<IEnumerable<string>>> ListBlobsAsync()
```

### Purpose
Lists all blobs in the specified container.

### Features
- Container content enumeration
- Filtered listing capabilities

### Return Type
- Returns `ServiceResponse<List<string>>` containing blob names
- Success case returns list of blob names
- Failure case returns null or empty list with error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": [
        "document1.txt",
        "image1.jpg",
        "config.json"
    ],
    "Message": null
}

// Failure case
{
    "Success": false,
    "Data": null,
    "Message": "List failed: Container not found"
}
```

### Usage Example
```csharp
var response = await cloudService.ListBlobsAsync();
if (response.Success)
{
    foreach (var blob in response.Data)
    {
        logger.LogInformation($" - {blob}");
    }
}
```

### Test Example
```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SECloud.Services;
using SECloud.Models;

namespace SECloudClient
{
    class ListBlobsProgram
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<CloudService>>();

            var httpClient = new HttpClient();
            var cloudService = new CloudService("https://secloudapp-2024.azurewebsites.net/api", "testblobcontainer", "", httpClient, logger);

            var response = await cloudService.ListBlobsAsync();
            if (response.Success)
            {
                logger.LogInformation("Blobs listed successfully:");
                foreach (var blob in response.Data)
                    logger.LogInformation(" - {BlobName}", blob);
            }
            else
                logger.LogError("Listing blobs failed: {Message}", response.Message);
        }
    }
}
```

## 6. Update Function
### Function Signature
```csharp
public async Task<ServiceResponse<bool>> UpdateAsync(
    string blobName,
    Stream content,
    string contentType)
```

### Purpose
Updates existing blob content or adds new one in the storage.

### Features
- Content type specification
- Stream-based updates
- Local file update support

### Return Type
- Returns `ServiceResponse<string>` containing update confirmation
- Success case returns updated blob URL
- Failure case returns error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": "https://storage.blob.core.windows.net/container/updated.txt",
    "Message": "Successfully updated updated.txt"
}

// Failure case
{
    "Success": false,
    "Data": null,
    "Message": "Update failed: 404 - Blob not found"
}
```

### Usage Example
```csharp
string blobName = "document.txt";
string contentType = "text/plain";
using var contentStream = new FileStream("local-file.txt", FileMode.Open);
var response = await cloudService.UpdateAsync(blobName, contentStream, contentType);
```

### Test Example
```csharp
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SECloud.Services;
using SECloud.Models;

namespace SECloudClient
{
    class UpdateBlobProgram
    {
        static async Task Main(string[] args)
        {
            // Setup dependency injection and logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<CloudService>>();

            var httpClient = new HttpClient();

            // Initialize CloudService with base URL, team/container, and optional SAS token
            var cloudService = new CloudService("https://secloudapp-2024.azurewebsites.net/api", "testblobcontainer", "", httpClient, logger);

            string blobName = "cloud_local_trial.txt";
            string contentType = "text/plain";

            // Open file as stream to use as content
            using var contentStream = new FileStream(@"C:\Users\ARNAV\Desktop\cloud_local_trial.txt", FileMode.Open, FileAccess.Read);

            var response = await cloudService.UpdateAsync(blobName, contentStream, contentType);

            if (response.Success)
            {
                logger.LogInformation("Blob updated successfully: {Message}", response.Message);
            }
            else
            {
                logger.LogError("Updating blob failed: {Message}", response.Message);
            }
        }
    }
}
```

## 7. Search JSON Function
### Function Signature
```csharp
public async Task<ServiceResponse<JsonSearchResult>> SearchJsonFilesAsync(
    string searchKey,
    string searchValue)
```

### Purpose
Searches through JSON files in the storage for specific key-value pairs.

### Features
- Key-value based search
- Formatted JSON results
- Match count reporting

### Return Type
- Returns `ServiceResponse<JsonSearchResponse>` containing search results
- Success case returns match count and matched files
- Failure case returns error message

### Example Response
```csharp
// Success case
{
    "Success": true,
    "Data": {
        "MatchCount": 2,
        "Matches": [
            {
                "FileName": "config1.json",
                "Content": {
                    "Theme": "Dark",
                    "Language": "EN"
                }
            },
            {
                "FileName": "config2.json",
                "Content": {
                    "Theme": "Dark",
                    "Language": "FR"
                }
            }
        ]
    },
    "Message": "Search completed successfully. Found 2 matches."
}

// Failure case
{
    "Success": false,
    "Data": null,
    "Message": "Search failed: Invalid search parameters"
}
```

### Supporting Types
```csharp
public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}

public class JsonSearchResponse
{
    public int MatchCount { get; set; }
    public List<JsonMatch> Matches { get; set; }
}

public class JsonMatch
{
    public string FileName { get; set; }
    public object Content { get; set; }
}

public class BlobListResponse
{
    public List<string> Blobs { get; set; }
}
```

### Usage Example
```csharp
string searchKey = "Theme";
string searchValue = "Dark";
var searchResponse = await cloudService.SearchJsonFilesAsync(searchKey, searchValue);
```

### Test Example
```csharp
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SECloud.Services;
using SECloud.Models;

namespace SECloudClient
{
    class SearchJsonProgram
    {
        static async Task Main(string[] args)
        {
            // Setup dependency injection and logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<CloudService>>();
            var httpClient = new HttpClient();

            // Initialize CloudService with base URL, team/container, and optional SAS token
            var cloudService = new CloudService(
                "https://secloudapp-2024.azurewebsites.net/api",
                "testblobcontainer",
                "",
                httpClient,
                logger
            );

            try
            {
                // Search parameters
                string searchKey = "Theme";  // Example key to search for
                string searchValue = "True";  // Example value to search for

                logger.LogInformation("Starting JSON search for key: {Key}, value: {Value}", searchKey, searchValue);

                var searchResponse = await cloudService.SearchJsonFilesAsync(searchKey, searchValue);

                if (searchResponse.Success)
                {
                    logger.LogInformation("Search completed successfully");
                    logger.LogInformation("Found {Count} matches", searchResponse.Data.MatchCount);

                    // Process and display each match
                    foreach (var match in searchResponse.Data.Matches)
                    {
                        logger.LogInformation("Match found in file: {FileName}", match.FileName);

                        // Pretty print the JSON content
                        var jsonFormatted = JsonSerializer.Serialize(match.Content, new JsonSerializerOptions
                        {
                            WriteIndented = true
                        });
                        logger.LogInformation("File content:\n{Content}", jsonFormatted);
                    }
                }
                else
                {
                    logger.LogError("Search failed: {Message}", searchResponse.Message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while performing the search");
            }

            // Optional: Wait for user input before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
```

## Error Handling
All functions return a `ServiceResponse<T>` object containing:
- Success status
- Error message (if applicable)
- Data payload (if successful)

## Best Practices
1. Always dispose of streams using `using` statements
2. Implement proper error handling for all operations
3. Use appropriate content types for different file formats
4. Implement logging for debugging and monitoring
5. Check response success before processing results

## Common Issues and Solutions
1. **Failed Authentication**: Verify SAS token validity and permissions
2. **Missing Blobs**: Check blob name and container existence
3. **Upload Failures**: Verify stream position and content type
4. **Download Errors**: Ensure adequate local storage and permissions

## Logging
The library uses Microsoft.Extensions.Logging for comprehensive logging:
- Information level for successful operations
- Error level for operation failures
- Debug level for detailed operation tracking
