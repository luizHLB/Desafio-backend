using Product.Domain.Entities;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUser(string email, string password);
    }
}
