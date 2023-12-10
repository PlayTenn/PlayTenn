using AutoMapper;
using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;

namespace PlayTen.BLL.Mapping
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<Participant, ParticipantDTO>()
                .ForMember(d => d.ParticipantId, s => s.MapFrom(f => f.ID))
                .ForMember(d => d.FullName, s => s.MapFrom(f => $"{f.User.Name} {f.User.Surname}"))
                .ForMember(d => d.Email, s => s.MapFrom(f => f.User.UserName))
                .ForMember(d => d.TennisLevel, s => s.MapFrom(f => f.User.TennisLevel.Level))
                .ForMember(d => d.UserId, s => s.MapFrom(f => f.UserId))
                .ForMember(d => d.StatusId, s => s.MapFrom(f => f.ParticipantStatusId))
                .ForMember(d => d.Status, s => s.MapFrom(f => f.ParticipantStatus.ParticipantStatusName));
        }
    }
}
