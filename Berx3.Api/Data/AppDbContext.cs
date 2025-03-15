using Berx3.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Berx3.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TShirt> TShirts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Purchase>()
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<TShirt>()
                .HasMany<Purchase>()
                .WithOne(p => p.TShirt)
                .HasForeignKey(p => p.TShirtId);
        }
    }
}
