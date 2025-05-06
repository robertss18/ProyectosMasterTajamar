using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SneakersServiceSNS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SneakersServiceSNS.Models.Product> Products { get; set; }
        public DbSet<SneakersServiceSNS.Models.Order> Orders { get; set; }
    }
}
