using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;
using ProjectEstimaterBackend.Models.ViewModel.Voting;
using ProjectEstimaterBackend.Services;
using System.Net;

namespace ProjectEstimaterBackend.Functions
{
    public class CreateVoting
    {
        private readonly IDataService<Voting> _votingService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateVoting(ILoggerFactory loggerFactory, IDataService<Voting> votingService, IMapper mapper)
        {
            _votingService = votingService;
            _logger = loggerFactory.CreateLogger<CreateVoting>();
            _mapper = mapper;
        }

        [Function("CreateVoting")]
        [OpenApiOperation(operationId: "1", tags: new[] { "Voting" }, Summary = "Create a Voting", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("Request Body", typeof(AddVotingViewModel), Description = "Request Body", Required = true, Example = typeof(AddParticipantViewModel))]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(Voting), Description = "Returns the created Voting")]

        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Voting")] HttpRequestData req)
        {
            //Get data from body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var voting = JsonConvert.DeserializeObject<AddVotingViewModel>(requestBody);

            if (voting == null)
            {
                _logger.LogError("Voting data is null");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                var result = _votingService.Add(_mapper.Map<Voting>(voting));

                var response = req.CreateResponse(HttpStatusCode.Created);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(JsonConvert.SerializeObject(result));
                return response;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Cosmos DB error: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
