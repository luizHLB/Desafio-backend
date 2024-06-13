namespace Product.Domain.DTO.Vehicle
{
    public class CreateVehicleDTO
    {
        public string Identifier { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
