using AutoMapper;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Mapping
{
    public class TennisLevelProfile : Profile
    {
        public TennisLevelProfile() => CreateMap<TennisLevel, TennisLevelDTO>().ReverseMap();
    }
}

