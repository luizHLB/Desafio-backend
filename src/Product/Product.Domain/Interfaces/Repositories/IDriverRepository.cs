using Product.Domain.DTO.Driver;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IDriverRepository : IBaseRepository<Driver, DriverDTO>
    {
    }
}
