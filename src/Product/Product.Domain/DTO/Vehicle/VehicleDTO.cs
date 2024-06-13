namespace Product.Domain.DTO.Vehicle
{
    public class VehicleDTO : CreateVehicleDTO
    {
        public long Id { get; set; }

        public VehicleDTO(Entities.Vehicle entity)
        {
            Id = entity.Id;
            Identifier = entity.Identifier;
            LicensePlate = entity.LicensePlate; 
            Model = entity.Model; 
            Year = entity.Year;
        }
        public VehicleDTO()
        {
            
        }
    }
}
