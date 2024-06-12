using Microsoft.Extensions.Logging;
using Product.Domain.DTO;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using System.Linq.Expressions;

namespace Product.Service.Base
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly ILogger<BaseService<T>> _logger;
        protected readonly IBaseRespository<T> _repository;

        public BaseService(ILogger<BaseService<T>> logger, IBaseRespository<T> respository)
        {
            _logger = logger;
            _repository = respository;
        }

        public virtual void Add(T entity)
        {
            _repository.Add(entity);
        }

        public Task<PagedListDTO<T>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize)
        {
            return _repository.PagedListAsync(expression, pageNumber, pageSize);
        }

        public void Remove(T entity)
        {
            _repository.Remove(entity);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }
    }
}
