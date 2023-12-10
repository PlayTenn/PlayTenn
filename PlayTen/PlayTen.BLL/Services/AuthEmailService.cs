using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;

namespace PlayTen.BLL.Services
{
    public class AuthEmailService : IAuthEmailService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        public AuthEmailService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IRepositoryWrapper repoWrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }


        public async Task<SignInResult> SignInAsync(LoginDTO loginDto)
        {
            var user = _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user.Result, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDTO registerDto)
        {
            var user = new User()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                TennisLevelId = int.Parse(registerDto.TennisLevelId)
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

        public async Task AddRoleAsync(RegisterDTO registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<UserDTO> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<User, UserDTO>(user);
        }

    }
}
