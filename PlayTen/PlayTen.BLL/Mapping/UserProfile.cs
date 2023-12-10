using AutoMapper;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Mapping
{
    public class UserProfile: Profile
    {
        public UserProfile() => CreateMap<User, UserDTO>().ReverseMap();
    }
}
