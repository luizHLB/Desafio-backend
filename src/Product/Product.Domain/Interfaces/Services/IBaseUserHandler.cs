using Product.Domain.Secutiry;

namespace Product.Domain.Interfaces.Services
{
    public interface IBaseServiceUserHandler : IBaseRepositoryUserHandler
    {
        IBaseRepositoryUserHandler Repository { get; set; }
    }

    public interface IBaseRepositoryUserHandler
    {
        void SetJwtContext(JwtContextVO vo);

    }
}
