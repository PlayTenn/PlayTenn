using System.Collections.Generic;
using System.Threading.Tasks;
using PlayTen.BLL.Interfaces;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.Configuration;
using PlayTen.BLL.DTO;
using System;

namespace PlayTen.BLL.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _storageConnectionString;

        public BlobStorageService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetConnectionString("BlobConnectionString");
        }

        public async Task<BlobDTO> UploadAsync(IFormFile blob, string storageContainerName)
        {
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, storageContainerName);
            await container.CreateIfNotExistsAsync();

            try
            {
                BlobClient client = container.GetBlobClient(GenerateFilename(blob.FileName));

                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                return new BlobDTO
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name,
                    Error = false
                };
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");

                return new BlobDTO
                {
                    Error = true
                };
            }
        }

        public async Task<int> DeleteAsync(string blobFilename, string storageContainerName)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, storageContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                await file.DeleteAsync();
                return StatusCodes.Status200OK;
            }
            catch
            {
                Console.WriteLine($"File {blobFilename} was not found.");
                return StatusCodes.Status400BadRequest;
            }
        }

        private string GenerateFilename(string originalFilename)
        {
            string[] strName = originalFilename.Split('.');
            string filename = strName[0] + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." +
               strName[strName.Length - 1];

            return filename;
        }
    }
}
