using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Voting;

namespace ProjectEstimaterBackend.Profiles
{
    public class AddVotingViewModelProfile : Profile
    {
        public AddVotingViewModelProfile()
        {
            CreateMap<AddVotingViewModel, Voting>()
                .ForMember(x => x.id, y => y.MapFrom(z => Guid.NewGuid().ToString()))
                .ForMember(x => x.title, y => y.MapFrom(z => z.title))
                .ForMember(x => x.date, y => y.MapFrom(z => DateTime.Now.Date.ToShortDateString()))
                .ForMember(x => x.result, y => y.MapFrom(z => -1))
                .ForMember(x => x.isActive, y => y.MapFrom(z => true))
                .ForMember(x => x.creator, y => y.MapFrom(z => z.creator));
        }
    }
}
