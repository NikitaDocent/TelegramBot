using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }
        public DbSet<TransactionDb> Transactions { get; set; }

        public ModelContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=telegramappdb;Trusted_Connection=True;");
        }
    }
}
