using AutoMapper;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;
using System.Linq;

namespace PlayTen.BLL.Mapping
{
    public class TournamentInfoProfile : Profile
    {
        public TournamentInfoProfile()
        {
            CreateMap<Tournament, TournamentInfoDTO>()
                .ForMember(d => d.TournamentId, s => s.MapFrom(e => e.Id))
                .ForMember(d => d.OwnerName, s => s.MapFrom(e => $"{e.User.Name} {e.User.Surname}"))
                .ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                .ForMember(d => d.DateStart, s => s.MapFrom(e => e.DateStart))
                .ForMember(d => d.TournamentPlaceId, s => s.MapFrom(e => e.PlaceId))
                .ForMember(d => d.NumberOfParticipants, s => s.MapFrom(e => e.NumberOfParticipants))
                .ForMember(d => d.DateEnd, s => s.MapFrom(e => e.DateEnd))
                .ForMember(d => d.HasStarted, s => s.MapFrom(e => e.HasStarted))
                .ForMember(d => d.Finished, s => s.MapFrom(e => e.Finished))
                .ForMember(d => d.AmountOfRounds, s => s.MapFrom(e => e.AmountOfRounds))
                .ForMember(d => d.UserId, s => s.MapFrom(e => e.OwnerId))
                .ForMember(d => d.Participants, s => s.MapFrom(e => e.Participants.ToList()));
        }
    }
}
