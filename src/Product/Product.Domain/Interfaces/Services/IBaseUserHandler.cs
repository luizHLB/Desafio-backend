using Product.Domain.Secutiry;

namespace Product.Domain.Interfaces.Services
{
    public interface IBaseUserHandler
    {
        void SetJwtContext(JwtContextVO vo);
    }
}
