using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Services;

using System;
using System.Threading.Tasks;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        private readonly ILoggerService<PlaceController> _loggerService;


        public AccountsController(IAccountsService accountsService, ILoggerService<PlaceController> loggerService)
        {
            _accountsService = accountsService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Get all users(accounts), available only for admin
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _accountsService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get specific user's account
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var user = await _accountsService.GetUserAsync(userId);
                return Ok(user);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit existing account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        public IActionResult Edit(UserDTO user)
        {
            try
            {
                _accountsService.UpdateUser(user);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete specific user account
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                await _accountsService.DeleteUserAsync(userId);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        ///  Upload user profile picture
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("uploadPhoto/{userId}")]
        public async Task<IActionResult> UploadPhoto(string userId, IFormFile file)
        {
            try
            {
                string imageUrl = await _accountsService.UploadPhoto(userId, file);
                return Ok(new { avatarUrl = imageUrl });
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        ///  Delete user profile picture
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("deletePhoto/{userId}")]
        public async Task<IActionResult> DeletePhoto(string userId)
        {
            try
            {
                await _accountsService.DeletePhoto(userId);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }
    }
}
