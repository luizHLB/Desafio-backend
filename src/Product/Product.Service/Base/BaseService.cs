using Microsoft.Extensions.Logging;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Domain.Secutiry;

namespace Product.Service.Base
{
    public class BaseService<T, TT> : IBaseService<T, TT> where T : class where TT : class
    {
        protected readonly ILogger<BaseService<T, TT>> _logger;
        protected readonly IBaseRepository<T, TT> _repository;
        public IBaseRepositoryUserHandler Repository { get; set; }

        private JwtContextVO jwtContext;
        public JwtContextVO _JwtContext { get { return jwtContext; }}


        public BaseService(ILogger<BaseService<T, TT>> logger, IBaseRepository<T, TT> repository)
        {
            _logger = logger;
            _repository = repository;
            Repository = repository;
        }

        public virtual async Task Add(T entity)
        {
            await Validate(entity);
            await _repository.Add(entity);
        }

        public virtual async Task Remove(long id)
        {
            await _repository.Remove(id);
        }

        public virtual async Task Remove(T entity)
        {
            await _repository.Remove(entity);
        }

        public async Task Update(T entity)
        {
            await Validate(entity);
            _repository.Update(entity);
        }

        protected virtual async Task Validate(T entity)
        {
        }

        public virtual async Task<T> GetById(long id)
        {
            return await _repository.GetById(id);
        }

        public void SetJwtContext(JwtContextVO vo)
        {
            jwtContext = vo;
        }
    }
}
