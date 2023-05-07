using AdminCustomerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminCustomerAPI.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }

    }
}
