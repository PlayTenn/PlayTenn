using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PlayTen.BLL.DTO;
using PlayTen.DAL.Entities;

namespace PlayTen.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<SignInResult> SignInAsync(LoginDTO loginDto);
        Task SignOutAsync();
        Task<IdentityResult> CreateUserAsync(RegisterDTO registerDto);
        Task AddRoleAsync(RegisterDTO registerDto);
        Task<UserDTO> FindByEmailAsync(string email);
        Task<User> FindUserByEmailAsync(string email);
        Task<string> GetConfirmationTokenAsync(User user);
    }
}
