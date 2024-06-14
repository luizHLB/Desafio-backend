using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Product.Domain.DTO.Driver
{
    public class UpdateDriverDTO
    {
        [JsonIgnore]
        public IFormFile CNHImage { get; set; }
    }
}
