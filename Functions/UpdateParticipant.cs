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
    public class UpdateParticipant
    {
        private readonly IDataService<Participant> _participantService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateParticipant(ILoggerFactory loggerFactory, IDataService<Participant> participantService, IMapper mapper)
        {
            _participantService = participantService;
            _logger = loggerFactory.CreateLogger<UpdateParticipant>();
            _mapper = mapper;
        }

        [Function("UpdateParticipant")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "patch", Route = "Participant/{id}")] HttpRequestData req, string id)
        {
            //Get data from body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var participant = JsonConvert.DeserializeObject<UpdateParticipantViewModel>(requestBody);

            if (participant == null || string.IsNullOrWhiteSpace(id))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                var result = _participantService.Update(_mapper.Map<Participant>(participant), id);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(JsonConvert.SerializeObject(result));
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
