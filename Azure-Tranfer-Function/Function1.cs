
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Azure_Dio_App
{
        
    public class TransferValue
    {
        private readonly ILogger<TransferValue> _logger;
        private readonly HttpClient _httpClient;
        public TransferValue(ILogger<TransferValue> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [Function(nameof(TransferValue))]
        public static async Task<OutputType> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Transaction")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PostToDo");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TransferValue transferValue = JsonConvert.DeserializeObject<TransferValue>(requestBody);


            return new OutputType()
            {
                transfer = transferValue,
                HttpResponse = req.CreateResponse(System.Net.HttpStatusCode.Created)
            };
        }


    }
    public class OutputType
    {
        [SqlOutput("dbo.usuarios", connectionStringSetting: "SqlConnectionString")]
        public TransferValue transfer { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}
