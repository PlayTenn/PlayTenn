using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PlayTen.BLL.DTO;

namespace PlayTen.BLL.Interfaces
{
    public interface IBlobStorageService
    {
        Task<BlobDTO> UploadAsync(IFormFile file, string storageContainerName);
        Task<int> DeleteAsync(string blobUri, string storageContainerName);
    }
}
