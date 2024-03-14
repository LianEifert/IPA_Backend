using ProjectEstimaterBackend.Models.Data;

namespace ProjectEstimaterBackend.Services
{
    public interface IVotingDataService
    {
        IList<Voting> GetAll();
        Voting GetById(string id);
    }
}
