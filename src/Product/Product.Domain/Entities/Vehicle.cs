using Product.Domain.Entities.Base;

namespace Product.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public string Identifier { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
