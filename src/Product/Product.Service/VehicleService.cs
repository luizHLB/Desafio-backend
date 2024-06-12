using Microsoft.Extensions.Logging;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Service.Base;

namespace Product.Service
{
    public class VehicleService : BaseService<Vehicle>, IVehicleService
    {
        public VehicleService(ILogger<BaseService<Vehicle>> logger, IVehicleRepository respository) : base(logger, respository)
        {
        }
    }
}
