using AutoMapper;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Mapping
{
    public class SurfaceProfile: Profile
    {
        public SurfaceProfile() => CreateMap<Surface, SurfaceDTO>().ReverseMap();
    }
}
