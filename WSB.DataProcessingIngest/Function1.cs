using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace WSB.DataProcessingIngest
{
    public static class Function1
    {
        [FunctionName("ApiIngest")]
        public static void Run([TimerTrigger("0 5 */2 * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context,
            [CosmosDB("Fagkveld", "ViktigeData", CreateIfNotExists = true, ConnectionStringSetting = "Default")] out dynamic document)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            string token = config.GetValue<string>("waqi_token");
            log.LogInformation($"Executing waqi api...");
            var responseList = new List<dynamic>();

            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDirectory = Path.GetFullPath(Path.Combine(binDirectory, ".."));

            var stations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(rootDirectory + "/latlang.json"));

            foreach (var station in stations)
            {
                var client = new RestClient(@$"https://api.waqi.info/feed/geo:{station.lat};{station.lon}/?token={token}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<dynamic>(request);

                if (response.IsSuccessful)
                    responseList.Add(response.Data["data"]);
            }

            log.LogInformation($"got the response!");

            document = new { values= responseList };
        }
    }
}
