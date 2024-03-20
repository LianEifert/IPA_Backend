using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectEstimaterBackend.Models.ViewModel.Participant;
using ProjectEstimaterBackend.Models.ViewModel.Voting;
using ProjectEstimaterBackend.Services;
using System.Net;

namespace ProjectEstimaterBackend.Functions
{
    public class GetVoting
    {
        private readonly IVotingDataService _votingService;
        private readonly IVotingParticipantService _votingParticipantService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetVoting(ILoggerFactory loggerFactory, IVotingDataService votingService, IVotingParticipantService votingParticipantService, IMapper mapper)
        {
            _votingService = votingService;
            _votingParticipantService = votingParticipantService;
            _logger = loggerFactory.CreateLogger<CreateVoting>();
            _mapper = mapper;
        }
        [Function("GetVoting")]
        [OpenApiOperation(operationId: "3", tags: new[] { "Voting" }, Summary = "Get a Voting", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("id", Description = "Id of the Voting", In = ParameterLocation.Path, Required = true, Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(VotingViewModel), Description = "Returns the Voting")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Voting/{id}")] HttpRequestData req, string id)
        {
            try
            {
                var voting = _mapper.Map<VotingViewModel>(_votingService.GetById(id));
                var votingParticipants = _mapper.Map<IList<ParticipantViewModel>>(_votingParticipantService.GetParticipantsForVoting(id));

                voting.participants = votingParticipants;

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonConvert.SerializeObject(voting));
                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError($"Item not found: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Cosmos DB error: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
