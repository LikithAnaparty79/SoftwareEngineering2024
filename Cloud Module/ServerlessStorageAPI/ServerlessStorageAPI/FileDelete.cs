using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using System.Net;
using Microsoft.Extensions.Logging;

namespace FA1;

public class FileDelete
{
    private const string ConnectionStringName = "AzureWebJobsStorage";

    [Function("DeleteFile")]
    public static async Task<HttpResponseData> DeleteFile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete/{team}/{filename}")] HttpRequestData req,
        string team,
        string filename,
        FunctionContext executionContext)
    {
        ILogger<FileDelete> logger = executionContext.GetLogger<FileDelete>();
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

            // Get container reference (assuming container name is team name)
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(team.ToLowerInvariant());

            // Get blob reference
            BlobClient blobClient = containerClient.GetBlobClient(filename);

            // Check if blob exists
            if (!await blobClient.ExistsAsync())
            {
                logger.LogWarning($"File {filename} not found in container {team}");
                response.StatusCode = HttpStatusCode.NotFound;
                await response.WriteStringAsync($"File {filename} not found");
                return response;
            }

            // Delete the blob
            await blobClient.DeleteAsync();

            logger.LogInformation($"Successfully deleted file {filename} from container {team}");
            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync($"File {filename} deleted successfully");
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
            logger.LogError($"Error deleting file: {ex.Message}");
            response.StatusCode = HttpStatusCode.InternalServerError;
            await response.WriteStringAsync("An error occurred while deleting the file");
            return response;
        }
    }
}
