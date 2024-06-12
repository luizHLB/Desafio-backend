using Product.Domain.Entities.Base;
using Product.Domain.Entities.Enums;

namespace Product.Domain.Entities
{
    public class Driver: BaseEntity
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNH { get; set; }
        public int CNHCategory { get; set; }
        public string CNHImage { get; set; }
        public ImageFormat ImageFormat { get; set; }


        public virtual ICollection<Rental>Rentals{ get; set; }
    }
}
