﻿using Product.Domain.DTO.Rental;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IRentalRepository : IBaseRepository<Rental, RentalDTO>
    {
        Task<bool> CheckVehicleDisponibilty(DateTime withdrawDate, DateTime estimatedReturnDate, long vehicleId);
    }
}
