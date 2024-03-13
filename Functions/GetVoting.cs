using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProjectEstimaterBackend.Functions
{
    public class GetVoting
    {
        private readonly ILogger<GetVoting> _logger;

        public GetVoting(ILogger<GetVoting> logger)
        {
            _logger = logger;
        }

        [Function("GetVoting")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Voting/{id}")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
