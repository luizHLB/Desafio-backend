using Product.Domain.Entities.Enums;
using Product.Domain.Helpers;

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
}
