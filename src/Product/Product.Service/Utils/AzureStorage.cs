using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Product.Domain.Interfaces.Utils;

namespace Product.Service.Utils
{
    public class AzureStorage : IAzureStorage
    {
        private readonly BlobContainerClient _storageAccountAzure;
        public AzureStorage(BlobContainerClient storageAccountAzure)
        {
            _storageAccountAzure = storageAccountAzure;
        }

        public async Task DeleteFile(string fileName)
        {
            var client = _storageAccountAzure.GetBlobClient(fileName);
            await client.DeleteIfExistsAsync();
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            using(var memoryStream = new MemoryStream())
            {
                var fileName = $"{Guid.NewGuid()}";
                var extension = file.FileName.Split('.').Last();
                await file.CopyToAsync(memoryStream);
                var client = _storageAccountAzure.GetBlobClient(fileName);
                var options = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = $"image/{extension}" } };
                memoryStream.Position = 0;
                await client.UploadAsync(memoryStream, options);

                return $"{fileName}.{extension}";

            }
        }
    }
}
