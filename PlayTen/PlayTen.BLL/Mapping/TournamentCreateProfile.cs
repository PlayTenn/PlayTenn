using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;
using AutoMapper;

namespace PlayTen.BLL.Mapping
{
    public class TournamentCreateProfile : Profile
    {
        public TournamentCreateProfile()
        {
            CreateMap<Tournament, TournamentCreateDTO>().ReverseMap();
        }
    }
}
