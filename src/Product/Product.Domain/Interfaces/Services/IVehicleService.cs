using Product.Domain.DTO;
using Product.Domain.DTO.Vehicle;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Services
{
    public interface IVehicleService : IBaseService<Vehicle, VehicleDTO>
    {
        Task<VehicleDTO> Create(CreateVehicleDTO dto);
        Task<VehicleDTO> GetDtoById(long id);
        Task<PagedListDTO<VehicleDTO>> PagedListAsync(string licensePlate = "", int page = 1, int pageSize = 10);
        Task<VehicleDTO> Update(PatchVehicleDTO dto);
    }
}
