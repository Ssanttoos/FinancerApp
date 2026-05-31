using Microsoft.EntityFrameworkCore;
using FinanceiroApp.Models;

namespace FinanceiroApp.Data
{
    // AppDbContext é a classe que representa o banco de dados
    // Ela herda de DbContext (Entity Framework Core)
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Cada DbSet representa uma tabela no banco
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura precisão dos campos decimais (para valores monetários)
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Goal>()
                .Property(g => g.TargetAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Goal>()
                .Property(g => g.CurrentAmount)
                .HasColumnType("decimal(18,2)");

            // Email deve ser único
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
