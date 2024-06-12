using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ProductContext context) : base(context)
        {
        }
    }
}
