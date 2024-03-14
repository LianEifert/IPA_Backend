
namespace ProjectEstimaterBackend.Models.Data
{
    public class Voting
    {
        public required string id {  get; set; }
        public required string title {  get; set; }
        public required string date { get; set; }
        public required decimal result { get; set; }
        public required bool isActive { get; set; }
    }
}
