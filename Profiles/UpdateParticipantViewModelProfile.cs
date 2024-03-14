using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
