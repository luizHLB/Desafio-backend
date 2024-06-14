using Product.Domain.DTO.Authentication;

namespace Product.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(AuthenticationDTO dto);
    }
}
