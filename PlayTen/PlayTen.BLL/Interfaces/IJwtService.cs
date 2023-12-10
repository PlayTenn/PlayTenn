using System.Threading.Tasks;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtTokenAsync(UserDTO userDto);
    }
}
