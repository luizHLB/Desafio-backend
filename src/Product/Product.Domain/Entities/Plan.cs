using Product.Domain.Entities.Base;

namespace Product.Domain.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; }
        public int Period { get; set; }
        public double Price { get; set; }

        public double? Fine { get; set; }
        public double? Extra { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
