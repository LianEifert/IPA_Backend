using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProjectEstimaterBackend.Functions
{
    public class UpdateVoting
    {
        private readonly ILogger<UpdateVoting> _logger;

        public UpdateVoting(ILogger<UpdateVoting> logger)
        {
            _logger = logger;
        }

        [Function("UpdateVoting")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "patch", Route = "Voting/{id}")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
