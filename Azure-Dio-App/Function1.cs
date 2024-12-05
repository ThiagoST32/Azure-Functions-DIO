using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure_Dio_App.Models;

namespace Azure_Dio_App
{
    public class Function1
    {
        [Function(nameof(Function1))]
        public static async Task<OutputType> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "PostFunction")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PostToDo");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
           

            return new OutputType()
            {
                user = user,
                HttpResponse = req.CreateResponse(System.Net.HttpStatusCode.Created)
            };
        }


    }
    public class OutputType
    {
        [SqlOutput("dbo.usuarios", connectionStringSetting: "SqlConnectionString")]
        public User user { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}
