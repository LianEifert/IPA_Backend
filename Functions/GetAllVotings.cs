using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Voting;
using ProjectEstimaterBackend.Services;
using System.Net;

namespace ProjectEstimaterBackend.Functions
{
    public class GetAllVotings
    {
        private readonly IVotingDataService _votingService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetAllVotings(ILoggerFactory loggerFactory, IVotingDataService votingService, IMapper mapper)
        {
            _votingService = votingService;
            _logger = loggerFactory.CreateLogger<GetAllVotings>();
            _mapper = mapper;
        }

        [Function("GetAllVotings")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Votings")] HttpRequestData req)
        {
            try
            {
                var votings = _votingService.GetAll();
                var ViewModels = new List<VotingViewModel>();

                foreach(var voting in votings)
                {
                    ViewModels.Add(_mapper.Map<VotingViewModel>(voting));
                }

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonConvert.SerializeObject(ViewModels));
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
