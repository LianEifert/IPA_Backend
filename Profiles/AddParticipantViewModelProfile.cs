using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;

namespace ProjectEstimaterBackend.Profiles
{
    public class AddParticipantViewModelProfile : Profile
    {
        public AddParticipantViewModelProfile()
        {
            CreateMap<AddParticipantViewModel, Participant>()
                .ForMember(x => x.id, y => y.MapFrom(z => Guid.NewGuid().ToString()))
                .ForMember(x => x.votingId, y => y.MapFrom(z => z.votingId))
                .ForMember(x => x.name, y => y.MapFrom(z => z.name))
                .ForMember(x => x.vote, y => y.MapFrom(z => -1));
        }
    }
}
