using Product.Domain.DTO;
using Product.Domain.Secutiry;
using System.Linq.Expressions;

namespace Product.Domain.Interfaces.Services
{
    public interface IBaseService<T, TT> where T : class where TT : class
    {
        Task<PagedListDTO<TT>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        Task Add(T entity);
        Task Remove(long id);
        Task Remove(T entity);
        Task Update(T entity);
        Task<T> GetById(long id);

        void SetJwtContext(JwtContextVO vo);
    }
}
