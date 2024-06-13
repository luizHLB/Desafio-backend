using Product.Domain.DTO.Vehicle;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository : IBaseRespository<Vehicle, VehicleDTO>
    {
    }
}
