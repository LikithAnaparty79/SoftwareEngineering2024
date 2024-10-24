/******************************************************************************
* Filename    = FileDelete.cs
*
* Author      = Pranav Guruprasad Rao
*
* Product     = Cloud
* 
* Project     = Unnamed Software Project
*
* Description = To delete files from local to cloud
*****************************************************************************/

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Net;
using System.Net.Http.Headers;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace FA1;

/// <summary>
/// Class responsible for handling file delete from Azure Blob Storage.
/// </summary>
public class FileDelete
{
    private const string ConnectionStringValue = "AzureWebJobsStorage";

    /// <summary>
    /// Azure Function to delete a file from an Azure Blob Storage container.
    /// The function is triggered via an HTTP DELETE request with anonymous authorization and a specific route.
    /// </summary>
    /// <param name="req">HTTP request data.</param>
    /// <param name="team">Path parameter representing the team (used as the container name).</param>
    /// <param name="filename">Path parameter representing the filename to delete.</param>
    /// <param name="executionContext">Execution context for the function (used to access the logger).</param>
    /// <returns>An HTTP response with the file content or an error message.</returns>
    [Function("DeleteFile")]
    public static async Task<HttpResponseData> DeleteFile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete/{team}/{filename}")] HttpRequestData req,
        string team,
        string filename,
        FunctionContext executionContext)
    {
        ILogger<FileDelete> logger = executionContext.GetLogger<FileDelete>();
        logger.LogInformation($"Attempting to delete file '{filename}' from container '{team}'.");

        // Get the connection string from the environment variables.
        string? connectionString = Environment.GetEnvironmentVariable(ConnectionStringValue);
        if (string.IsNullOrEmpty(connectionString))
        {
            logger.LogError("Connection string not found");
            // Create an internal server error response indicating the missing connection string.
            HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Storage connection string not configured");
            return errorResponse;
        }

        try
        {
            // Initialize a BlobServiceClient using the connection string and get a reference to the Blob container for the specified team.
            var blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(team.ToLowerInvariant());

            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the specific blob (file) using the filename.
            BlobClient blobClient = containerClient.GetBlobClient(filename);
            logger.LogInformation($"Blob URI: {blobClient.Uri}");

            if (await blobClient.ExistsAsync())
            {
                logger.LogInformation($"File '{filename}' exists in the container '{team}'.");

                // Attempt to delete the blob.
                bool deleted = await blobClient.DeleteIfExistsAsync();
                if (deleted)
                {
                    logger.LogInformation($"File '{filename}' successfully deleted from container '{team}'.");
                    HttpResponseData successResponse = req.CreateResponse(HttpStatusCode.OK);
                    await successResponse.WriteStringAsync($"File {filename} deleted successfully from {team} container.");
                    return successResponse;
                }
                else
                {
                    logger.LogWarning($"File '{filename}' could not be deleted from container '{team}'.");
                    HttpResponseData notDeletedResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await notDeletedResponse.WriteStringAsync($"Failed to delete file {filename} from {team} container.");
                    return notDeletedResponse;
                }
            }

            logger.LogWarning($"File '{filename}' not found in the container '{team}'.");
            // Create a NotFound response if the file does not exist.
            HttpResponseData notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            await notFoundResponse.WriteStringAsync($"File {filename} not found in {team} container.");
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error processing request for file '{filename}' in container '{team}'");
            // Create an internal server error response in case of an exception.
            HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Error processing request: {ex.Message}");
            return errorResponse;
        }
    }
}
