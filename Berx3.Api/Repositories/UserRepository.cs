using Berx3.Api.Data;
using Berx3.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Berx3.Api.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
            => _context = context;

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
            

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task SoftDeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) { 
                user.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
