using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CheckVehicleDisponibilty(DateTime withdrawDate, DateTime estimatedReturnDate, long vehicleId)
        {
            var query = from r in _context.Rentals.AsNoTracking()
                        where r.VehicleId == vehicleId &&
                        ((r.ReturnDate != null && withdrawDate >= r.WithdrawDate && withdrawDate <= r.ReturnDate && estimatedReturnDate >= r.WithdrawDate) ||
                        (r.ReturnDate == null && withdrawDate >= r.EstimatedReturnDate && withdrawDate <= r.EstimatedReturnDate && estimatedReturnDate >= r.EstimatedReturnDate))
                        select 1;

            return !await query.AnyAsync();
        }
    }
}
