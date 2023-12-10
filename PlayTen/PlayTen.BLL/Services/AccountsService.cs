using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTen.DAL.Entities;
using PlayTen.DAL.Repositories;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PlayTen.BLL.Services
{
    public class AccountsService : IAccountsService
    {
        private const string storageContainerName = "avatars";
        //private readonly UserManager<User> _userManager;
        private IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private IBlobStorageService _blobStorageService;

        public AccountsService(IMapper mapper, UserManager<User> userManager, IRepositoryWrapper repositoryWrapper, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _blobStorageService = blobStorageService;
            //_userManager = userManager;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _repositoryWrapper.User.GetAll().ToListAsync();
            return _mapper.Map<List<User>, List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserAsync(string userId)
        {
            var user = await _repositoryWrapper.User.GetAll().FirstAsync(x => x.Id == userId);
            return _mapper.Map<User, UserDTO>(user);
        }

        public void UpdateUser(UserDTO user)
        {
            var newUser = _mapper.Map<UserDTO, User>(user);
            _repositoryWrapper.User.Update(newUser);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _repositoryWrapper.User.GetAll().FirstAsync(x => x.Id == userId);
            _repositoryWrapper.User.Delete(user);
        }

        public async Task<string> UploadPhoto(string userId, IFormFile file)
        {
            UserDTO userDto = await GetUserAsync(userId);
            string oldFilename = userDto.ProfileImageFilename;

            BlobDTO blobDto = await _blobStorageService.UploadAsync(file, storageContainerName);
            if (!blobDto.Error)
            {
                userDto.ProfileImageFilename = blobDto.Name;
                userDto.ProfileImageUrl = blobDto.Uri;

                UpdateUser(userDto);
                await _blobStorageService.DeleteAsync(oldFilename, storageContainerName);

                return userDto.ProfileImageUrl;
            }

            return null;
        }

        public async Task DeletePhoto(string userId)
        {
            UserDTO userDto = await GetUserAsync(userId);
            var response = await _blobStorageService.DeleteAsync(userDto.ProfileImageFilename, storageContainerName);
            if (response == StatusCodes.Status200OK)
            {
                userDto.ProfileImageFilename = null;
                userDto.ProfileImageUrl = null;

                UpdateUser(userDto);
            }

        }
    }
}
