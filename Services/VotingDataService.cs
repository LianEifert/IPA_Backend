using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using ProjectEstimaterBackend.Models.Data;
using static Grpc.Core.Metadata;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

namespace ProjectEstimaterBackend.Services
{
    public class VotingDataService : IDataService<Voting>, IVotingParticipantService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        private readonly Container _ParticipantsContainer;
        private readonly Container _VotingsContainer;

        public VotingDataService()
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
            _cosmosClient = new CosmosClient(connectionString);
            _database = _cosmosClient.GetDatabase("ProjectEstimater");

            _ParticipantsContainer = _database.GetContainer("Participants");
            _VotingsContainer = _database.GetContainer("Votings");
        }

        public Voting Add(Voting entity)
        {
            return AddAsync(entity).GetAwaiter().GetResult();
        }
        public async Task<Voting> AddAsync(Voting entity)
        {
            //Check if null
            if (string.IsNullOrWhiteSpace(entity.id)) throw new ArgumentException("Id not set");
            if (string.IsNullOrWhiteSpace(entity.date)) throw new ArgumentException("date not set");
            if (string.IsNullOrWhiteSpace(entity.title)) throw new ArgumentException("title not set");
            if (entity.isActive == null) throw new ArgumentException("isActive not set");
            if (entity.result == null) throw new ArgumentException("Number not set");

            //Create new Voting
            return await _VotingsContainer.CreateItemAsync(entity, new PartitionKey(entity.id));
        }

        public IList<Voting> GetAll()
        {
            return GetAllAsync().GetAwaiter().GetResult();
        }
        public async Task<IList<Voting>> GetAllAsync()
        {
            //Get all Votings
            var query = "SELECT * FROM v";
            var iterator = _VotingsContainer.GetItemQueryIterator<Voting>(query);
            var votings = new List<Voting>();

            //Loop over every item it gets from the query
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                votings.AddRange(response);
            }

            return votings;
        }

        public Voting GetById(string id)
        {
            return GetByIdAsync(id).GetAwaiter().GetResult();
        }
        public async Task<Voting> GetByIdAsync(string id)
        {
            return await _VotingsContainer.ReadItemAsync<Voting>(id, new PartitionKey(id));
        }

        public IList<Participant> GetParticipantsForVoting(string votingId)
        {
            return GetParticipantsForVotingAsync(votingId).GetAwaiter().GetResult();
        }
        public async Task<IList<Participant>> GetParticipantsForVotingAsync(string votingId)
        {
            //Only get participants where defined votingId is equal
            var query = new QueryDefinition("SELECT * FROM v WHERE v.votingId = @votingId")
             .WithParameter("@votingId", votingId);
            var iterator = _ParticipantsContainer.GetItemQueryIterator<Participant>(query);
            var participants = new List<Participant>();

            //Loop over every item it gets from the query
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                participants.AddRange(response.Resource);
            }
            return participants;
        }

        public Voting Update(Voting entity, string id)
        {
            return UpdateAsync(entity, id).GetAwaiter().GetResult();
        }
        public async Task<Voting> UpdateAsync(Voting entity, string id)
        {
            //Check if null
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id not set");
            if (entity.result== null) throw new ArgumentException("Vote not set");
            if (entity.isActive== null) throw new ArgumentException("isActive not set");

            //Define patch Operations
            var patchOperations = new List<PatchOperation>
            {
                PatchOperation.Replace("/result", entity.result),
                PatchOperation.Replace("/isActive", entity.isActive)
            };

            //Only update the defined properties
            return await _VotingsContainer.PatchItemAsync<Voting>(
                id: id,
                partitionKey: new PartitionKey(id),
                patchOperations: patchOperations
                );
        }
    }
}
