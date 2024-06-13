using Microsoft.AspNetCore.Http;
using Product.Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Product.Domain.DTO.Driver
{
    public class CreateDriverDTO
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNH { get; set; }
        public CNHCategory[] CNHCategory { get; set; }
        [JsonIgnore]
        public IFormFile CNHImage { get; set; }
    }
}
