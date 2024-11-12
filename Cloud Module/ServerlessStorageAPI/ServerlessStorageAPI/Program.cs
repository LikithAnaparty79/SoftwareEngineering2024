using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using FA1;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices((context, services) => {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                services.AddSingleton<IConfiguration>(context.Configuration);

                services.AddLogging();

                services.AddSingleton(sp => {
                    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                    string? connectionString = configuration["AzureWebJobsStorage"];
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException("AzureWebJobsStorage connection string is not configured.");
                    }
                    return new BlobServiceClient(connectionString);
                });

                services.AddScoped<ConfigRetrieve>();
            })
            .Build();

        host.Run();
    }
}

/*               

         Register BlobServiceClient
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration["AzureWebJobsStorage"];
            return new BlobServiceClient(connectionString);
        });
    })
    .Build();

        host.Run();
    }
}*/
