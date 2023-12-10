using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Interfaces
{
    public interface IAccountsService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> GetUserAsync(string userId);
        void UpdateUser(UserDTO user);
        Task DeleteUserAsync(string userId);
        Task<string> UploadPhoto(string userId, IFormFile file);
        Task DeletePhoto(string userId);

    }
}
