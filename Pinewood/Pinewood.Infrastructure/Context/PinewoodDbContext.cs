using Microsoft.EntityFrameworkCore;
using Pinewood.Domain.Entities;

namespace Pinewood.Infrastructure.Context
{
    public class PinewoodDbContext : DbContext
    {
        public PinewoodDbContext(DbContextOptions<PinewoodDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; } = default!;
    }
}
