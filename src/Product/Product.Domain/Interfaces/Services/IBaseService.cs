using Product.Domain.DTO;
using System.Linq.Expressions;

namespace Product.Domain.Interfaces.Services
{
    public interface IBaseService<T, TT> : IBaseServiceUserHandler where T : class where TT : class
    {
        Task Add(T entity);
        Task Remove(long id);
        Task Remove(T entity);
        Task Update(T entity);
        Task<T> GetById(long id);
    }
}
