using System;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace WSB.DataProcessingIngest
{
    public static class Function1
    {
        [FunctionName("ApiIngest")]
        public static void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context,
            [CosmosDB("Fagkveld", "ViktigeData", CreateIfNotExists = true, ConnectionStringSetting = "Default")] out dynamic document)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            string token = config.GetValue<string>("waqi_token");
            log.LogInformation($"Executing waqi api...");

            var client = new RestClient($"https://api.waqi.info/feed/geo:54.380277777778;18.620277777778/?token={token}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<dynamic>(request);

            log.LogInformation($"got the response!");

            document = response.Data;
        }
    }
}
