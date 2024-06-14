using Microsoft.EntityFrameworkCore;
using Product.Data.Contexts;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductContext _context;

        public UserRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(f => f.Email == email && f.Password == password);
        }
    }
}
