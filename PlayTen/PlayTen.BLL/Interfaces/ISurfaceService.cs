using PlayTen.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTen.BLL.Services
{
    public interface ISurfaceService
    {
        Task<IEnumerable<SurfaceDTO>> GetAllAsync();
        Task<SurfaceDTO> GetSurfaceAsync(int surfaceId);
    }
}