using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayTen.BLL.Interfaces;
using System;
using System.Linq;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get info about all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users != null)
            {
                return Ok(users.Where(u => u.Email != "admin@playten.com").ToList());
            }

            return NotFound();
        }

        /// <summary>
        /// Get info about specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        /// <summary>
        /// Delete specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            try
            {
                _userService.DeleteUser(userId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }
    }
}
