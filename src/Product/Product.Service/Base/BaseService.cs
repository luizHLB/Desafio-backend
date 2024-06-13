using Microsoft.Extensions.Logging;
using Product.Domain.DTO;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using System.Linq.Expressions;

namespace Product.Service.Base
{
    public class BaseService<T, TT> : IBaseService<T, TT> where T : class where TT : class
    {
        protected readonly ILogger<BaseService<T, TT>> _logger;
        protected readonly IBaseRespository<T, TT> _repository;

        public BaseService(ILogger<BaseService<T, TT>> logger, IBaseRespository<T, TT> respository)
        {
            _logger = logger;
            _repository = respository;
        }

        public virtual async Task Add(T entity)
        {
            Validate(entity);
            await _repository.Add(entity);
        }

        public Task<PagedListDTO<TT>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize)
        {
            return _repository.PagedListAsync(expression, pageNumber, pageSize);
        }

        public async Task Remove(long id)
        {
            await _repository.Remove(id);
        }

        public void Update(T entity)
        {
            Validate(entity);
            _repository.Update(entity);
        }

        public virtual void Validate(T entity)
        {
        }

        public virtual async Task<T> GetById(long id)
        {
            return await _repository.GetById(id);
        }
    }
}
