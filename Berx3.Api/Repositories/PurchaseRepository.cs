using Berx3.Api.Data;
using Berx3.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Berx3.Api.Repositories
{
    public class PurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Purchase>> GetAllAsync()
        {
            return await _context.Purchases
                .Include(p => p.User)
                .Include(p => p.TShirt)
                .ToListAsync();
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            return await _context.Purchases
                .Include(p => p.User)
                .Include (p => p.TShirt)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<string> AddPurchaseAsync(int userId, int tshirdId, int quantity)
        {
            var user = await _context.Users.FindAsync(userId);
            var tshirt = await _context.TShirts.FindAsync(tshirdId);

            if (user == null)
            {
                return "User not found.";
            }
            if (tshirt == null || !tshirt.IsAvailable)
            {
                return "T-Shirt not available.";
            }
            if (tshirt.Quantity < quantity)
            {
                return "Not enough stock available.";
            }

            // Create purchase record
            var purchase = new Purchase
            {
                UserId = userId,
                TShirtId = tshirdId,
                Quantity = quantity,
                PurchaseDate = DateTime.UtcNow
            };

            tshirt.Quantity -= quantity; // Reduce stock

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return "Purchase successful.";
        }
    }
}
