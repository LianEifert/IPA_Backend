using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Voting;

namespace ProjectEstimaterBackend.Profiles
{
    public class VotingViewModelProfile : Profile
    {
        public VotingViewModelProfile()
        {
            CreateMap<Voting, VotingViewModel>()
                .ForMember(x => x.id, y => y.MapFrom(z => z.id))
                .ForMember(x => x.title, y => y.MapFrom(z => z.title))
                .ForMember(x => x.date, y => y.MapFrom(z => z.date))
                .ForMember(x => x.result, y => y.MapFrom(z => z.result))
                .ForMember(x => x.isActive, y => y.MapFrom(z => z.isActive));
        }
    }
}
