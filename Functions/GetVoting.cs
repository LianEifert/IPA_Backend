using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;
using ProjectEstimaterBackend.Models.ViewModel.Voting;
using ProjectEstimaterBackend.Services;
using System.Net;

namespace ProjectEstimaterBackend.Functions
{
    public class GetVoting
    {
        private readonly IDataService<Voting> _votingService;
        private readonly IVotingParticipantService _votingParticipantService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetVoting(ILoggerFactory loggerFactory, IDataService<Voting> votingService, IVotingParticipantService votingParticipantService, IMapper mapper)
        {
            _votingService = votingService;
            _votingParticipantService = votingParticipantService;
            _logger = loggerFactory.CreateLogger<CreateVoting>();
            _mapper = mapper;
        }
        [Function("GetVoting")]
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
