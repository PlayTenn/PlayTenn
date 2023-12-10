using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.Controllers
{
    /// <summary>
    /// Implements all business logic related to tournaments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class TournamentController : ControllerBase
    {
        private readonly ITournamentActionManager _actionManager;
        private readonly UserManager<User> _userManager;

        public TournamentController(ITournamentActionManager actionManager, UserManager<User> userManager)
        {
            _actionManager = actionManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Get tournaments
        /// </summary>
        /// <returns>List of Tournaments of the appropriate Tournament type and Tournament category.</returns>
        /// <response code="200">List of tournaments</response>
        /// <response code="400">Server could not understand the request due to i
        /// nvalid syntax</response> 
        /// <response code="404">Tournaments don't exist</response> 
        [HttpGet("all")]
        public async Task<IActionResult> GetTournaments()
        {
            return Ok(await _actionManager.GetTournamentsAsync(await _userManager.GetUserAsync(User)));
        }


        /// <summary>
        /// Get detailed information about specific tournament.
        /// </summary>
        /// <returns>A detailed information about specific Tournament.</returns>
        /// <param name="id">The Id of tournament</param>
        /// <response code="200">An instance of tournament</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Tournament does not exist</response> 
        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetTournamentDetail(int id)
        {
            return Ok(await _actionManager.GetTournamentInfoAsync(id, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Delete Tournament by Id.
        /// </summary>
        /// <returns>Status code of the Tournament deleting operation.</returns>
        /// <param name="id">The Id of Tournament</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return StatusCode(await _actionManager.DeleteTournamentAsync(id));
        }

        /// <summary>
        /// Create new Tournament participant.
        /// </summary>
        /// <returns>Status code of the subscribing on Tournament operation.</returns>
        /// <param name="TournamentId">The Id of Tournament</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPost("{TournamentId:int}/participants")]
        public async Task<IActionResult> SubscribeOnTournament(int TournamentId)
        {
            return StatusCode(await _actionManager.SubscribeOnTournamentAsync(TournamentId, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Delete Tournament participant by Tournament id.
        /// </summary>
        /// <returns>Status code of the unsubscribing on Tournament operation.</returns>
        /// <param name="TournamentId">The Id of Tournament</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{TournamentId:int}/participants")]
        public async Task<IActionResult> UnSubscribeOnTournament(int TournamentId)
        {
            return StatusCode(await _actionManager.UnSubscribeOnTournamentAsync(TournamentId, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Change Tournament participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing Tournament participant status operation.</returns>
        /// <param name="participantId">The Id of Tournament participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/approved")]
        public async Task<IActionResult> ApproveParticipant(int participantId)
        {
            return StatusCode(await _actionManager.ApproveParticipantAsync(participantId));
        }

        /// <summary>
        /// Change Tournament participant status to under reviewed.
        /// </summary>
        /// <returns>Status code of the changing Tournament participant status operation.</returns>
        /// <param name="participantId">The Id of Tournament participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/underReviewed")]
        public async Task<IActionResult> UnderReviewParticipant(int participantId)
        {
            return StatusCode(await _actionManager.UnderReviewParticipantAsync(participantId));
        }

        /// <summary>
        /// Change Tournament participant status to rejected.
        /// </summary>
        /// <returns>Status code of the changing Tournament participant status operation.</returns>
        /// <param name="participantId">The Id of Tournament participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/rejected")]
        public async Task<IActionResult> RejectParticipant(int participantId)
        {
            return StatusCode(await _actionManager.RejectParticipantAsync(participantId));
        }
    }
}
