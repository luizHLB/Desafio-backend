using Microsoft.AspNetCore.Http;
using Product.Domain.DTO;
using Product.Domain.DTO.Driver;
using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Services
{
    public interface IDriverService : IBaseService<Driver, DriverDTO>
    {
        Task<DriverDTO> Create(CreateDriverDTO dto);
        Task<DriverDTO> GetDtoById(long id);
        Task<PagedListDTO<DriverDTO>> PagedListAsync(string name = "", string cnpj = "", string cnh = "", int page = 1, int pageSize = 10);
        Task<DriverDTO> Update(long id, UpdateDriverDTO dto);
    }
}
