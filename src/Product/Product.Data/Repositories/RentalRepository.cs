using Product.Data.Contexts;
using Product.Data.Repositories.Base;
using Product.Domain.DTO.Rental;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class RentalRepository : BaseRepository<Rental, RentalDTO>, IRentalRepository
    {
        public RentalRepository(ProductContext context) : base(context) { }

        public override List<RentalDTO> Cast(List<Rental> itens) => itens.Select(s => new RentalDTO(s)).ToList();
    }
}
