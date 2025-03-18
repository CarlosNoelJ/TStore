using Berx3.Api.Data;
using Berx3.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Berx3.Api.Repositories
{
    public class TshirtRepository : IRepository<TShirt>
    {

        private readonly AppDbContext _context;

        public TshirtRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TShirt>> GetAllAsync()
        {
            return await _context.TShirts
                .Where(t => t.IsAvailable)
                .ToListAsync();
        }

        public async Task<TShirt?> GetByIdAsync(int id)
        {
            return await _context.TShirts.FindAsync(id);
        }

        public async Task AddAsync(TShirt entity)
        {
            await _context.TShirts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TShirt entity)
        {
            _context.TShirts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var tshirt = await _context.TShirts.FindAsync(id);
            if (tshirt != null)
            {
                tshirt.IsAvailable = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
