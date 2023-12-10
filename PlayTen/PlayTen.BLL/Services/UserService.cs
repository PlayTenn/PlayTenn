using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using System.Linq;

namespace PlayTen.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(IRepositoryWrapper repoWrapper,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _repoWrapper.User.GetAll().ToListAsync();
            users.ForEach(user => user.TennisLevel = _repoWrapper.TennisLevel.GetFirstAsync(predicate: t => t.Id == user.TennisLevelId).Result);
            var model = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);

            return model;
        }

        public async Task<UserDTO> GetUserAsync(string userId)
        {
            var user = await _repoWrapper.User.GetAll().FirstOrDefaultAsync(
                i => i.Id == userId);
            user.TennisLevel = await _repoWrapper.TennisLevel.GetFirstAsync(predicate: t => t.Id == user.TennisLevelId);
            var model = _mapper.Map<User, UserDTO>(user);

            return model;
        }

        public async Task<IEnumerable<string>> GetRolesAsync(UserDTO user)
        {
            var result = await _userManager.GetRolesAsync(_mapper.Map<UserDTO, User>(user));
            return result;
        }

        public void DeleteUser(string userId)
        {
            var user = _repoWrapper.User.GetAll(filter: x => x.Id == userId).AsEnumerable().First();
            _repoWrapper.User.Delete(user);
        }
    }
}
