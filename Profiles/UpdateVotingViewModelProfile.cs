using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Voting;

namespace ProjectEstimaterBackend.Profiles
{
    public class UpdateVotingViewModelProfile : Profile
    {
        public UpdateVotingViewModelProfile()
        {
            CreateMap<UpdateVotingViewModel, Voting>()
                .ForMember(x => x.isActive, y => y.MapFrom(z => z.isActive))
                .ForMember(x => x.result, y => y.MapFrom(z => z.result));
        }
    }
}
