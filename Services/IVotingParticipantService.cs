using ProjectEstimaterBackend.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEstimaterBackend.Services
{
    public interface IVotingParticipantService
    {
        IList<Participant> GetParticipantsForVoting(string votingId);
    }
}
