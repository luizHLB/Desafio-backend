using Microsoft.AspNetCore.Http;

namespace Product.Domain.Interfaces.Utils
{
    public interface IAzureStorage
    {
        Task DeleteFile(string fileName);
        Task<string> UploadFile(IFormFile file);
    }
}
