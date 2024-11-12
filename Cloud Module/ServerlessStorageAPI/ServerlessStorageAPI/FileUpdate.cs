using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace FA1;

public class FileUpdate
{
    private const string ConnectionStringName = "AzureWebJobsStorage";

    [Function("UpdateFile")]
    public static async Task<HttpResponseData> UpdateFile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "put/{team}/{filename}")] HttpRequestData req,
        string team,
        string filename,
        FunctionContext executionContext)
    {
        ILogger<FileUpdate> logger = executionContext.GetLogger<FileUpdate>();
        HttpResponseData response = req.CreateResponse();

        try
        {
            // Get the connection string from configuration
            string? connectionString = Environment.GetEnvironmentVariable(ConnectionStringName);
            if (string.IsNullOrEmpty(connectionString))
            {
                logger.LogError("Storage connection string not found");
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync("Storage configuration error");
                return response;
            }

            // Create blob service client
            var blobServiceClient = new BlobServiceClient(connectionString);

            // Get or create container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(team.ToLowerInvariant());
            await containerClient.CreateIfNotExistsAsync();

            // Get blob reference
            BlobClient blobClient = containerClient.GetBlobClient(filename);

            // Check if the blob exists
            bool blobExists = await blobClient.ExistsAsync();

            // Copy the request body to a stream
            using var stream = new MemoryStream();
            await req.Body.CopyToAsync(stream);
            stream.Position = 0; // Reset stream position for reading

            // Set content type or default to application/octet-stream
            string contentType = req.Headers.TryGetValues("Content-Type", out IEnumerable<string>? contentTypeValues)
                ? contentTypeValues.FirstOrDefault() ?? "application/octet-stream"
                : "application/octet-stream";

            // Upload the blob
            if (blobExists)
            {
                // Update the existing blob
                await blobClient.UploadAsync(stream, new BlobUploadOptions {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
                });
                logger.LogInformation($"Successfully updated file {filename} in container {team}");
            }
            else
            {
                // Create a new blob
                await blobClient.UploadAsync(stream, new BlobUploadOptions {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
                });
                logger.LogInformation($"Successfully created file {filename} in container {team}");
            }

            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync($"File {filename} uploaded/updated successfully");
            return response;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            logger.LogWarning($"Container {team} not found: {ex.Message}");
            response.StatusCode = HttpStatusCode.NotFound;
            await response.WriteStringAsync($"Team container {team} not found");
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error uploading file: {ex.Message}");
            response.StatusCode = HttpStatusCode.InternalServerError;
            await response.WriteStringAsync("An error occurred while uploading the file");
            return response;
        }
    }
}
