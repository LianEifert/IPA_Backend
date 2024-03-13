using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEstimaterBackend.Models.Data
{
    public class Voting
    {
        public required string id {  get; set; }
        public required string date { get; set; }
        public required int result { get; set; }
        public required bool isActive { get; set; }
    }
}
