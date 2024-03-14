using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;

namespace ProjectEstimaterBackend.Profiles
{
    public class UpdateParticipantViewModelProfile : Profile
    {
        public UpdateParticipantViewModelProfile()
        {
            CreateMap<UpdateParticipantViewModel, Participant>()
                .ForMember(x => x.vote, y => y.MapFrom(z => z.vote));
        }
    }
}
