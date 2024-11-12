using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using System.Collections.Generic;
using System.IO;

namespace FA1;

/// <summary>
/// JSON Search Class for searching through blob containers
/// </summary>
public class JsonSearch
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<JsonSearch> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor for JsonSearch class
    /// </summary>
    public JsonSearch(BlobServiceClient blobServiceClient, ILogger<JsonSearch> logger, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Azure Function to search for specific key-value pairs across all JSON files in a container
    /// </summary>
    /// <param name="req">HTTP request data</param>
    /// <param name="team">Container name to search in</param>
    /// <returns>List of matching files with their content</returns>
    [Function("SearchJsonFiles")]
    public async Task<HttpResponseData> SearchJsonFiles(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "search/{team}")] HttpRequestData req,
        string team)
    {
        try
        {
            _logger.LogInformation($"Search function triggered for container: {team}");

            // Get search parameters from query string
            System.Collections.Specialized.NameValueCollection queryDictionary = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string searchKey = queryDictionary["key"];
            string searchValue = queryDictionary["value"];

            if (string.IsNullOrEmpty(searchKey) || string.IsNullOrEmpty(searchValue))
            {
                HttpResponseData badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Both 'key' and 'value' query parameters are required");
                return badRequest;
            }

            // Get container client
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(team);

            // Check if container exists
            if (!await containerClient.ExistsAsync())
            {
                HttpResponseData notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync($"Container '{team}' not found");
                return notFound;
            }

            var matchingFiles = new List<object>();

            // List all blobs in the container
            await foreach (Azure.Storage.Blobs.Models.BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                if (!blobItem.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);

                try
                {
                    // Download and parse JSON content
                    Response<Azure.Storage.Blobs.Models.BlobDownloadResult> content = await blobClient.DownloadContentAsync();
                    string jsonContent = content.Value.Content.ToString();
                    using JsonDocument doc = JsonDocument.Parse(jsonContent);

                    // Search for the key-value pair using recursive search
                    if (SearchJsonElement(doc.RootElement, searchKey, searchValue))
                    {
                        matchingFiles.Add(new {
                            fileName = blobItem.Name,
                            content = JsonDocument.Parse(jsonContent).RootElement
                        });
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning($"Error parsing JSON in file {blobItem.Name}: {ex.Message}");
                    continue;
                }
            }

            // Create response
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new {
                searchKey = searchKey,
                searchValue = searchValue,
                matchCount = matchingFiles.Count,
                matches = matchingFiles
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing search request");
            HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Error processing request: {ex.Message}");
            return errorResponse;
        }
    }

    /// <summary>
    /// Recursively searches through JSON elements for a specific key-value pair
    /// </summary>
    private bool SearchJsonElement(JsonElement element, string searchKey, string searchValue)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (property.Name.Equals(searchKey, StringComparison.OrdinalIgnoreCase) &&
                        property.Value.ToString().Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    if (SearchJsonElement(property.Value, searchKey, searchValue))
                    {
                        return true;
                    }
                }
                break;

            case JsonValueKind.Array:
                foreach (JsonElement arrayElement in element.EnumerateArray())
                {
                    if (SearchJsonElement(arrayElement, searchKey, searchValue))
                    {
                        return true;
                    }
                }
                break;
        }

        return false;
    }
}
