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
using ProjectEstimaterBackend.Models.ViewModel.Voting;
using ProjectEstimaterBackend.Services;
using System.Net;

namespace ProjectEstimaterBackend.Functions
{
    public class UpdateVoting
    {
        private readonly IDataService<Voting> _votingService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateVoting(ILoggerFactory loggerFactory, IDataService<Voting> votingService, IMapper mapper)
        {
            _votingService = votingService;
            _logger = loggerFactory.CreateLogger<CreateVoting>();
            _mapper = mapper;
        }

        [Function("UpdateVoting")]
        [OpenApiOperation(operationId: "4", tags: new[] { "Voting" }, Summary = "Update a Voting", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter("id", Description = "Id of the Voting", In = ParameterLocation.Path, Required = true, Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(Voting), Description = "Returns the updated Voting")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "patch", Route = "Voting/{id}")] HttpRequestData req, string id)
        {
            //Get data from body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var voting = JsonConvert.DeserializeObject<UpdateVotingViewModel>(requestBody);

            if (voting == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                var result = _votingService.Update(_mapper.Map<Voting>(voting), id);

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
