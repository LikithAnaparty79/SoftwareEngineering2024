using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Net;
using Microsoft.Extensions.Logging;

namespace FA1;
public class UploadInterface
{
    [Function("GetUploadInterface")]
    public async static Task<HttpResponseData> GetUploadInterface(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "upload")] HttpRequestData req,
    FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("UploadInterface");
        logger.LogInformation("GetUploadInterface function triggered.");

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/html");

        try
        {
            // Get the base directory of the function app
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(basePath, "www", "upload.html");
            logger.LogInformation("Reading HTML file from path: {htmlPath}", htmlPath);

            // Read the HTML file
            string htmlContent = await File.ReadAllTextAsync(htmlPath);
            response.WriteString(htmlContent);
            logger.LogInformation("HTML file successfully read and returned.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading the HTML file.");
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.WriteString("Error loading the upload interface.");
        }

        return response;
    }
}


