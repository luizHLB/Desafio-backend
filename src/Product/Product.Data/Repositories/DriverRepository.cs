using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.DTO.Driver;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class DriverRepository : BaseRepository<Driver, DriverDTO>, IDriverRepository
    {
        public DriverRepository(ProductContext context) : base(context)
        {
        }

        public override List<DriverDTO> Cast(List<Driver> itens)
        {
            return itens.Select(s => new DriverDTO(s)).ToList();
        }
    }
}
