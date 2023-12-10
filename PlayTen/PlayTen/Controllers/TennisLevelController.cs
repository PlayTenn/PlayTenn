using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayTen.BLL.Interfaces;
using System;
using System.Threading.Tasks;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TennisLevelController : ControllerBase
    {
        private readonly ITennisLevelService _tennisLevelService;
        private readonly ILoggerService<TennisLevelController> _loggerService;

        public TennisLevelController(ITennisLevelService tennisLevelService, ILoggerService<TennisLevelController> loggerService)
        {
            _tennisLevelService = tennisLevelService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Get sport kind of specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var tennisLevelDto = await _tennisLevelService.GetTennisLevelAsync(userId);
                return Ok(tennisLevelDto);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get all sport kinds
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _tennisLevelService.GetAllTennisLevelsAsync());
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Set sport kind for specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tennisLevelId"></param>
        /// <returns></returns>
        [HttpPost("set/{userId}-{tennisLevelId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SetUserSportKind(string userId, int tennisLevelId)
        {
            try
            {
                await _tennisLevelService.SetUpUserTennisLevelAsync(userId, tennisLevelId);
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
