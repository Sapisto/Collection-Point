using Microsoft.EntityFrameworkCore;
using CollectPoint.Domain.Entities;

namespace CollectPoint.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // One DbSet per table, like prisma.customer
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Postgres convention: lowercase snake_case for tables and columns
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customer");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(30);
            entity.Property(x => x.CustomerName).HasColumnName("customer_name").HasMaxLength(200).IsRequired();
            entity.Property(x => x.AccountNumber).HasColumnName("account_number").HasMaxLength(10).IsRequired();
            entity.Property(x => x.Currency).HasColumnName("currency").HasMaxLength(3).IsRequired();
            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(x => x.AccountNumber).IsUnique();
        });
    }
}
