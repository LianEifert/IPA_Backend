using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProjectEstimaterBackend.Functions
{
    public class GetAllVotings
    {
        private readonly ILogger<GetAllVotings> _logger;

        public GetAllVotings(ILogger<GetAllVotings> logger)
        {
            _logger = logger;
        }

        [Function("GetAllVotings")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Votings")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
