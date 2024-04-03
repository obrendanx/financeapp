using FinanceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Payments> Payments { get; set; }
    }
}