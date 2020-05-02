using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HelloWorldDurableFunctionApp
{
    public static class DurableFunctionChain
    {
        const string OrchestratorFunctionName = "CircleDurableFunctionOrchestrator";
        const string GetValueOfPiFunctionName = "GetValueOfPi";
        const string GetAreaOfCircle = "GetAreaOfCircle";
        [FunctionName("DurableFunctionChain_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(OrchestratorFunctionName, null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName(OrchestratorFunctionName)]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            
            double pi = await context.CallActivityAsync<double>(GetValueOfPiFunctionName, "Tokyo");
            outputs.Add($"Value of pi {pi}");
            string area = await context.CallActivityAsync<string>(GetAreaOfCircle, pi);
            outputs.Add($" Area of circle { area}");
            outputs.Add(await context.CallActivityAsync<string>("Function1_Hello", "London"));

            return outputs;
        }
        [FunctionName(GetAreaOfCircle)]
        public static double FindArea([ActivityTrigger] double pi)
        {
            return pi * 3 * 3;
        }
        [FunctionName(GetValueOfPiFunctionName)]
        async public static Task<double> GetValueOfPi([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Gettting value of pi.");
            //Respone is like \"03141\"
            //Try to find a better API returns with decimal point
            string piValueResponseString = await new HttpClient().GetStringAsync("https://uploadbeta.com/api/pi/?cached&n=4");
            var piValue = Convert.ToDouble(piValueResponseString
                                            .Replace("\"","")
                                            .Insert(2, "."));
            return piValue;
        }
        [FunctionName("Function1_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        
    }
}