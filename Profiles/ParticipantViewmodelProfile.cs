using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;

namespace ProjectEstimaterBackend.Profiles
{
    public class ParticipantViewmodelProfile : Profile
    {
        public ParticipantViewmodelProfile()
        {
            CreateMap<Participant, ParticipantViewModel>()
                .ForMember(x => x.id, y => y.MapFrom(z => z.id))
                .ForMember(x => x.vote, y => y.MapFrom(z => z.vote))
                .ForMember(x => x.name, y => y.MapFrom(z => z.name))
                .ForMember(x => x.votingId, y => y.MapFrom(z => z.votingId));
                }
    }
}
