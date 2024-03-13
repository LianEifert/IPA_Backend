using ProjectEstimaterBackend.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEstimaterBackend.Services
{
    public class VotingDataService : IDataService<Voting>, IVotingParticipantService
    {
        public Voting Add(Voting entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Voting> GetAll()
        {
            throw new NotImplementedException();
        }

        public Voting GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Participant> GetParticipantsForVoting(string votingId)
        {
            throw new NotImplementedException();
        }

        public Voting Update(Voting entity, string id)
        {
            throw new NotImplementedException();
        }
    }
}
