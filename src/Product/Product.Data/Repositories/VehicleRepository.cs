using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.DTO.Vehicle;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle, VehicleDTO>, IVehicleRepository
    {
        public VehicleRepository(ProductContext context) : base(context) { }

        public override List<VehicleDTO> Cast(List<Vehicle> itens)
        {
            return itens.Select(s => new VehicleDTO(s)).ToList();
        }
    }
}
