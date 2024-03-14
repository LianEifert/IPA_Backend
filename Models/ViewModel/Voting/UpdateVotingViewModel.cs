using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEstimaterBackend.Models.ViewModel.Voting
{
    public class UpdateVotingViewModel
    {
        public required decimal result { get; set; }
        public required bool isActive { get; set; }
    }
}
