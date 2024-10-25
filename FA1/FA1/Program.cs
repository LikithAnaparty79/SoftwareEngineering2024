using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        IHost host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        /*/ Register BlobServiceClient
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration["AzureWebJobsStorage"];
            return new BlobServiceClient(connectionString);
        });*/
    })
    .Build();

        host.Run();
    }
}
