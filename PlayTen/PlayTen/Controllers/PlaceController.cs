using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Services;
using Microsoft.AspNetCore.Http;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        private readonly ILoggerService<PlaceController> _loggerService;

        public PlaceController(IPlaceService placeService, ILoggerService<PlaceController> loggerService)
        {
            _placeService = placeService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Get all places
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var places = await _placeService.GetAllAsync();
                return Ok(places);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get specific place
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int placeId)
        {
            try
            {
                var place = await _placeService.GetPlaceAsync(placeId);
                return Ok(place);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Get all place for specific sport kind
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetBySurfaceId(int surfaceId)
        {
            try
            {
                var place = await _placeService.GetPlacesBySurfaceIdAsync(surfaceId);
                return Ok(place);
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Create new place
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("new")]
        public async Task<IActionResult> Addplace(PlaceDTO place)
        {
            try
            {
                await _placeService.AddPlaceAsync(place);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit existing place
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("edit")]
        public IActionResult Editplace(PlaceDTO place)
        {
            try
            {
                _placeService.UpdatePlace(place);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete specific place
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{placeId}")]
        public IActionResult Delete(int placeId)
        {
            try
            {
                _placeService.DeletePlace(placeId);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        ///  Upload place photo
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("uploadPhoto/{placeId}")]
        public async Task<IActionResult> UploadPhoto(int placeId, IFormFile file)
        {
            try
            {
                await _placeService.UploadPhoto(placeId, file);
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError(e.Message);
            }

            return BadRequest();
        }

        /// <summary>
        ///  Delete place photo
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("deletePhoto/{placeId}")]
        public async Task<IActionResult> DeletePhoto(int placeId, string filename)
        {
            try
            {
                await _placeService.DeletePhoto(placeId, filename);
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
