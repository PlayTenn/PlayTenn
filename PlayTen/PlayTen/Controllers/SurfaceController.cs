using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Services;
using PlayTen.DAL.Entities;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SurfaceController : ControllerBase
    {
        private readonly ISurfaceService _surfaceService;
        private readonly ILoggerService<PlaceController> _loggerService;

        public SurfaceController(ISurfaceService surfaceService, ILoggerService<PlaceController> loggerService)
        {
            _surfaceService = surfaceService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Get all surfaces
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var surfaces = await _surfaceService.GetAllAsync();
                return Ok(surfaces);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get surface by id
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int surfaceId)
        {
            try
            {
                var surface = await _surfaceService.GetSurfaceAsync(surfaceId);
                return Ok(surface);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }
    }
}
