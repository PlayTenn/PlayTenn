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
    public class TournamentsUsersController : ControllerBase
    {
        private readonly ITournamentUserManager TournamentUserManager;
        private readonly ITournamentUserService TournamentUserService;
        private readonly UserManager<User> _userManager;

        public TournamentsUsersController(ITournamentUserManager TournamentUserManager, ITournamentUserService TournamentUserService, UserManager<User> userManager)
        {
            this.TournamentUserManager = TournamentUserManager;
            this.TournamentUserService = TournamentUserService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all created, planned, visited Tournaments for user by id
        /// </summary>
        /// <returns>Array of all created, planned, visited Tournaments for user</returns>
        /// /// <param name="userId"></param>
        /// <response code="200">Instance of TournamentUserDTO</response>
        /// <response code="400">When the TournamentUserDTO is null or empty</response> 
        [HttpGet("TournamentUsers/{userId}")]
        public async Task<IActionResult> GetTournamentUserByUserId(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var TournamentUserModel = await TournamentUserService.TournamentUserAsync(userId, await _userManager.GetUserAsync(User));
            return Ok(TournamentUserModel);
        }

        /// <summary>
        /// Create a new Tournament
        /// </summary>
        /// <returns>A newly created Tournament</returns>
        /// <param name="createDTO"></param>
        /// <response code="201">Instance of TournamentCreateDTO</response>
        /// <response code="400">When the TournamentCreateDTO is null or empty</response> 
        [HttpPost("newTournament")]
        public async Task<IActionResult> TournamentCreate([FromBody] TournamentCreateDTO createDTO)
        {
            try
            {
                createDTO.Id = await TournamentUserManager.CreateTournamentAsync(createDTO);

                return Created(nameof(GetTournamentUserByUserId), createDTO);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        /// <summary>
        /// Put edited Tournament
        /// </summary>
        /// <returns>A newly edited Tournament</returns>
        /// <param name="createDTO"></param>
        /// <response code="204">Resource updated successfully</response>
        /// <response code="400">When the TournamentCreateDTO is null or empty</response>
        [HttpPut("editedTournament")]
        public async Task<IActionResult> TournamentEdit([FromBody] TournamentCreateDTO createDTO)
        {
            await TournamentUserManager.EditTournamentAsync((createDTO));

            return NoContent();
        }

        [HttpPost("startTournament/{id}")]
        public async Task<IActionResult> StartTournament(int id)
        {
            await TournamentUserManager.StartTournament(id);

            return NoContent();
        }

        [HttpPost("finishTournament/{id}")]
        public async Task<IActionResult> FinishTournament(int id)
        {
            await TournamentUserManager.FinishTournament(id);

            return NoContent();
        }
    }
}
