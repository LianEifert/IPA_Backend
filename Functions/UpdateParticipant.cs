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
        [OpenApiOperation(operationId: "2", tags: new[] { "Participant" }, Summary = "Update a Participant", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("id", Description = "Id of the Participant", In = ParameterLocation.Path, Required = true, Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(Participant), Description = "Returns the updated Participant")]
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
