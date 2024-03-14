using ProjectEstimaterBackend.Models.Data;

namespace ProjectEstimaterBackend.Services
{
    public interface IVotingParticipantService
    {
        IList<Participant> GetParticipantsForVoting(string votingId);
    }
}
