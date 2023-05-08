using AdminCustomerAPI.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminCustomerAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }

    }
}
