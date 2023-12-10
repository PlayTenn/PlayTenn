using AutoMapper;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;
using System.Linq;

namespace PlayTen.BLL.Mapping
{
    public class TrainingInfoProfile: Profile
    {
        public TrainingInfoProfile()
        {
            CreateMap<Training, TrainingInfoDTO>()
                .ForMember(d => d.TrainingId, s => s.MapFrom(e => e.Id))
                .ForMember(d => d.OwnerName, s => s.MapFrom(e => $"{e.User.Name} {e.User.Surname}"))
                .ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                .ForMember(d => d.DateStart, s => s.MapFrom(e => e.DateStart))
                .ForMember(d => d.TrainingPlaceId, s => s.MapFrom(e => e.PlaceId))
                .ForMember(d => d.NumberOfParticipants, s => s.MapFrom(e => e.NumberOfParticipants))
                .ForMember(d => d.HasBalls, s => s.MapFrom(e => e.HasBalls))
                .ForMember(d => d.DateEnd, s => s.MapFrom(e => e.DateEnd))
                .ForMember(d => d.UserId, s => s.MapFrom(e => e.OwnerId))
                .ForMember(d => d.Participants, s => s.MapFrom(e => e.Participants.ToList()));
        }
    }
}
