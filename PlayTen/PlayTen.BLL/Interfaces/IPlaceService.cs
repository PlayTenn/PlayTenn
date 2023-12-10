using Microsoft.AspNetCore.Http;
using PlayTen.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public interface IPlaceService
    {
        Task AddPlaceAsync(PlaceDTO place);
        void DeletePlace(int placeId);
        Task<IEnumerable<PlaceDTO>> GetAllAsync();
        Task<PlaceDTO> GetPlaceAsync(int placeId);
        Task<IEnumerable<PlaceDTO>> GetPlacesBySurfaceIdAsync(int surfaceId);
        void UpdatePlace(PlaceDTO place);
        Task UploadPhoto(int placeId, IFormFile file);
        Task DeletePhoto(int placeId, string filename);
    }
}