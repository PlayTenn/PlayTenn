using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Services;
using Microsoft.AspNetCore.Http;
using PlayTen.DAL.Entities;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly ILoggerService<PlaceController> _loggerService;

        public MatchController(IMatchService matchService, ILoggerService<PlaceController> loggerService)
        {
            _matchService = matchService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Get specific match
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        [HttpGet("getAllMatches/{id:int}")]
        public async Task<IActionResult> GetMatches(int id)
        {
            try
            {
                var matches = await _matchService.GetAllMatches(id);
                return Ok(matches);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get all matches for specific user
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/generateDraw")]
        public async Task<IActionResult> GenerateDrawByTournamentId(int id)
        {
            try
            {
                await _matchService.GenerateDrawAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Create new match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("new")]
        public async Task<IActionResult> AddMatch(Match match)
        {
            try
            {
                await _matchService.AddMatchAsync(match);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Update existing match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public IActionResult UpdateMatch([FromBody] Match match)
        {
            try
            {
                _matchService.UpdateMatch(match);
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
