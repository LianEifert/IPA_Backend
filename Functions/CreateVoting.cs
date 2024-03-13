using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProjectEstimaterBackend.Functions
{
    public class CreateVoting
    {
        private readonly ILogger<CreateVoting> _logger;

        public CreateVoting(ILogger<CreateVoting> logger)
        {
            _logger = logger;
        }

        [Function("CreateVoting")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Voting")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
