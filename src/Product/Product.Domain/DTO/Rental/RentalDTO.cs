namespace Product.Domain.DTO.Rental
{
    public class RentalDTO : CreateRentalDTO
    {
        public long Id { get; set; }
        public DateTime? ReturnDate { get; set; }
        public RentalExpansesDTO Expanses { get; set; }
        public RentalDTO(Entities.Rental entity)
        {
            Id = entity.Id;
            DriverId = entity.DriverId;
            VehicleId = entity.VehicleId;
            WithdrawDate = entity.WithdrawDate;
            EstimatedReturnDate = entity.EstimatedReturnDate;
            ReturnDate = entity.ReturnDate;
            if (entity.ReturnDate.HasValue)
                Expanses = new RentalExpansesDTO(entity);
        }
        public RentalDTO()
        {
        }
    }

}
