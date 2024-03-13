using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEstimaterBackend.Models.ViewModel
{
    public class AddParticipantViewModel
    {
        public required string votingId { get; set; }
        public required string name { get; set; }
    }
}
