using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;
using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;
using PlayTen.BLL.DTO;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TrainingsUsersController : ControllerBase
    {
        private readonly ITrainingUserManager TrainingUserManager;
        private readonly ITrainingUserService TrainingUserService;
        private readonly UserManager<User> _userManager;

        public TrainingsUsersController(ITrainingUserManager TrainingUserManager, ITrainingUserService TrainingUserService, UserManager<User> userManager)
        {
            this.TrainingUserManager = TrainingUserManager;
            this.TrainingUserService = TrainingUserService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all created, planned, visited Trainings for user by id
        /// </summary>
        /// <returns>Array of all created, planned, visited Trainings for user</returns>
        /// /// <param name="userId"></param>
        /// <response code="200">Instance of TrainingUserDTO</response>
        /// <response code="400">When the TrainingUserDTO is null or empty</response> 
        [HttpGet("TrainingsUsers/{userId}")]
        public async Task<IActionResult> GetTrainingUserByUserId(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var TrainingUserModel = await TrainingUserService.TrainingUserAsync(userId, await _userManager.GetUserAsync(User));
            return Ok(TrainingUserModel);
        }

        /// <summary>
        /// Create a new Training
        /// </summary>
        /// <returns>A newly created Training</returns>
        /// <param name="createDTO"></param>
        /// <response code="201">Instance of TrainingCreateDTO</response>
        /// <response code="400">When the TrainingCreateDTO is null or empty</response> 
        [HttpPost("newTraining")]
        public async Task<IActionResult> TrainingCreate([FromBody] TrainingCreateDTO createDTO)
        {
            try
            {
                createDTO.Id = await TrainingUserManager.CreateTrainingAsync(createDTO);

                return Created(nameof(GetTrainingUserByUserId), createDTO);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        /// <summary>
        /// Put edited Training
        /// </summary>
        /// <returns>A newly edited Training</returns>
        /// <param name="createDTO"></param>
        /// <response code="204">Resource updated successfully</response>
        /// <response code="400">When the TrainingCreateDTO is null or empty</response>
        [HttpPut("editedTraining")]
        public async Task<IActionResult> TrainingEdit([FromBody] TrainingCreateDTO createDTO)
        {
            await TrainingUserManager.EditTrainingAsync((createDTO));

            return NoContent();
        }
    }
}
