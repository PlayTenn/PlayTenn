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
    /// Implements all business logic related with trainings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ITrainingActionManager _actionManager;
        private readonly UserManager<User> _userManager;

        public TrainingsController(ITrainingActionManager actionManager, UserManager<User> userManager)
        {
            _actionManager = actionManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Get Trainings of the appropriate Training type and Training category.
        /// </summary>
        /// <returns>List of Trainings of the appropriate Training type and Training category.</returns>
        /// <response code="200">List of Trainings</response>
        /// <response code="400">Server could not understand the request due to i
        /// nvalid syntax</response> 
        /// <response code="404">Trainings don't exist</response> 
        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetTrainings()
        {
            return Ok(await _actionManager.GetTrainingsAsync(await _userManager.GetUserAsync(User)));
        }


        /// <summary>
        /// Get detailed information about specific Training.
        /// </summary>
        /// <returns>A detailed information about specific Training.</returns>
        /// <param name="id">The Id of Training</param>
        /// <response code="200">An instance of Training</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Training does not exist</response> 
        [HttpGet("{id:int}/details")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetTrainingDetail(int id)
        {
            return Ok(await _actionManager.GetTrainingInfoAsync(id, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Delete Training by Id.
        /// </summary>
        /// <returns>Status code of the Training deleting operation.</returns>
        /// <param name="id">The Id of Training</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(int id)
        {
            return StatusCode(await _actionManager.DeleteTrainingAsync(id));
        }

        /// <summary>
        /// Create new Training participant.
        /// </summary>
        /// <returns>Status code of the subscribing on Training operation.</returns>
        /// <param name="TrainingId">The Id of Training</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPost("{TrainingId:int}/participants")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SubscribeOnTraining(int TrainingId)
        {
            return StatusCode(await _actionManager.SubscribeOnTrainingAsync(TrainingId, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Delete Training participant by Training id.
        /// </summary>
        /// <returns>Status code of the unsubscribing on Training operation.</returns>
        /// <param name="TrainingId">The Id of Training</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{TrainingId:int}/participants")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UnSubscribeOnTraining(int TrainingId)
        {
            return StatusCode(await _actionManager.UnSubscribeOnTrainingAsync(TrainingId, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Change Training participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing Training participant status operation.</returns>
        /// <param name="participantId">The Id of Training participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/approved")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ApproveParticipant(int participantId)
        {
            return StatusCode(await _actionManager.ApproveParticipantAsync(participantId));
        }

        /// <summary>
        /// Change Training participant status to under reviewed.
        /// </summary>
        /// <returns>Status code of the changing Training participant status operation.</returns>
        /// <param name="participantId">The Id of Training participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/underReviewed")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UnderReviewParticipant(int participantId)
        {
            return StatusCode(await _actionManager.UnderReviewParticipantAsync(participantId));
        }

        /// <summary>
        /// Change Training participant status to rejected.
        /// </summary>
        /// <returns>Status code of the changing Training participant status operation.</returns>
        /// <param name="participantId">The Id of Training participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/rejected")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RejectParticipant(int participantId)
        {
            return StatusCode(await _actionManager.RejectParticipantAsync(participantId));
        }
    }
}
