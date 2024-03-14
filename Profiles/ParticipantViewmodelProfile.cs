using AutoMapper;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Models.ViewModel.Participant;
using ProjectEstimaterBackend.Models.ViewModel.Voting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
