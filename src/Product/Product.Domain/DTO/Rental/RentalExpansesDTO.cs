namespace Product.Domain.DTO.Rental
{
    public class RentalExpansesDTO
    {
        public double? TotalRental { get; set; }
        public double? TotalFines { get; set; }
        public double? TotalExtras { get; set; }
        public double Total => TotalRental.GetValueOrDefault() + TotalFines.GetValueOrDefault() + TotalExtras.GetValueOrDefault();
        public RentalExpansesDTO(Entities.Rental entity)
        {
            TotalRental = entity.TotalRental;
            TotalFines = entity.TotalFines;
            TotalExtras = entity.TotalExtras;
        }
    }

}
