using PlayTen.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserAsync(string userId);
        Task<IEnumerable<string>> GetRolesAsync(UserDTO user);
        void DeleteUser(string userId);
    }
}
