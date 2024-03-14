using Microsoft.Azure.Cosmos;
using ProjectEstimaterBackend.Models.Data;

namespace ProjectEstimaterBackend.Services
{
    public class ParticipantDataService : IDataService<Participant>
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        private readonly Container _ParticipantsContainer;

        public ParticipantDataService()
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
            _cosmosClient = new CosmosClient(connectionString);
            _database = _cosmosClient.GetDatabase("ProjectEstimater");

            _ParticipantsContainer = _database.GetContainer("Participants");
        }

        public Participant Add(Participant entity)
        {
            return AddAsync(entity).GetAwaiter().GetResult();
        }
        public async Task<Participant> AddAsync(Participant entity)
        {
            //Check if null
            if (string.IsNullOrWhiteSpace(entity.id)) throw new ArgumentException("Id not set");
            if (string.IsNullOrWhiteSpace(entity.name)) throw new ArgumentException("Name not set");
            if (string.IsNullOrWhiteSpace(entity.votingId)) throw new ArgumentException("VotingId not set");

            //Create new Participant
            return await _ParticipantsContainer.CreateItemAsync(entity, new PartitionKey(entity.id));
        }

        public Participant Update(Participant entity, string id)
        {
            return UpdateAsync(entity,id).GetAwaiter().GetResult();
        }
        public async Task<Participant> UpdateAsync(Participant entity, string id)
        {
            //Check if null
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id not set");

            //Only update the vote property
            return await _ParticipantsContainer.PatchItemAsync<Participant>(
                id: id,
                partitionKey: new PartitionKey(id),
                patchOperations: new[] { PatchOperation.Replace("/vote", entity.vote) }
                );
        }
    }
}
