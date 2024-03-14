using ProjectEstimaterBackend.Models.ViewModel.Participant;

namespace ProjectEstimaterBackend.Models.ViewModel.Voting
{
    public class VotingViewModel
    {
        public required string id { get; set; }
        public required string title { get; set; }
        public required string date { get; set; }
        public required decimal result { get; set; }
        public required bool isActive { get; set; }
        public required string creator { get; set; }

        public required IList<ParticipantViewModel> participants { get; set; }
    }
}
