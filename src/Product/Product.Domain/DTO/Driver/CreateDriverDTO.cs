using Product.Domain.Entities.Enums;

namespace Product.Domain.DTO.Driver
{
    public class CreateDriverDTO : UpdateDriverDTO
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNH { get; set; }
        public CNHCategory[] CNHCategory { get; set; }
    }
}
