using Microsoft.AspNetCore.Http;
using Product.Domain.Entities.Enums;
using Product.Domain.Helpers;
using System.Text.Json.Serialization;

namespace Product.Domain.DTO.Driver
{
    public class DriverDTO : CreateDriverDTO
    {
        public long Id { get; set; }
        public string CNHImage { get; set; }
        

        public DriverDTO(Entities.Driver entity)
        {
            Id = entity.Id;
            Identifier = entity.Identifier;
            Name = entity.Name;
            CNPJ = entity.CNPJ;
            BirthDate = entity.BirthDate;
            CNH = entity.CNH;
            CNHCategory = EnumHelper<CNHCategory>.GetEnums(entity.CNHCategory).ToArray();
        }
    }

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
