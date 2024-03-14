
namespace ProjectEstimaterBackend.Models.ViewModel.Participant
{
    public class ParticipantViewModel
    {
        public required string id { get; set; }
        public required string votingId { get; set; }
        public required string name { get; set; }
        public required int vote { get; set; }
    }
}
