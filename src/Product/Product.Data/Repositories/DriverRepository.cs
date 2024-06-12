using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class DriverRepository : BaseRepository<Driver>, IDriverRepository
    {
        public DriverRepository(ProductContext context) : base(context)
        {
        }
    }
}
