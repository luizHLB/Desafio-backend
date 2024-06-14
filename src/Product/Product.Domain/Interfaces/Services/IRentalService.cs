﻿using Product.Domain.DTO.Rental;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Services
{
    public interface IRentalService : IBaseService<Rental, RentalDTO>
    {
        Task<RentalDTO> Create(CreateRentalDTO dto);
        Task<RentalDTO> Complete(UpdateRentalDTO dto);
    }
}
