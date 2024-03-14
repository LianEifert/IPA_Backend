using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;
using ProjectEstimaterBackend.Services;
using System.Net;

namespace ProjectEstimaterBackend.Functions
{
    public class CreateParticipant
    {
        private readonly IDataService<Participant> _participantService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateParticipant(ILoggerFactory loggerFactory, IDataService<Participant> participantService, IMapper mapper)
        {
            _participantService = participantService;
            _logger = loggerFactory.CreateLogger<CreateParticipant>();
            _mapper = mapper;
        }

        [Function("CreateParticipant")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function,"post", Route = "Participant")] HttpRequestData req)
        {
            //Get data from body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var participant = JsonConvert.DeserializeObject<AddParticipantViewModel>(requestBody);

            if (participant == null)
            {
                _logger.LogError("Participant data is null");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrWhiteSpace(participant.votingId))
            {
                _logger.LogError("Participant is not part of a voting");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                var result = _participantService.Add(_mapper.Map<Participant>(participant));

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
