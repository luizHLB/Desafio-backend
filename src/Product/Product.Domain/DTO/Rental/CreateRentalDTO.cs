namespace Product.Domain.DTO.Rental
{
    public class CreateRentalDTO
    {
        public long DriverId { get; set; }
        public long VehicleId { get; set; }
        public long PlanId { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime EstimatedReturnDate { get; set; }
    }
}
