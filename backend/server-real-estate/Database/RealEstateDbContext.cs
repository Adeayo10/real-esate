using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using server_real_estate.Model;

namespace server_real_estate.Database;

public class RealEstateDbContext : IdentityDbContext<User>, IRealEstatateDbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }
    public override DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override EntityEntry Entry(object entity)
    {
        return base.Entry(entity);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Map entities to tables
    modelBuilder.Entity<Property>().ToTable("Properties");
    modelBuilder.Entity<User>().ToTable("Users", "dbo"); // Explicitly map Users to dbo schema

    // Set default schema
    modelBuilder.HasDefaultSchema("dbo");

    // Configure Property entity
    modelBuilder.Entity<Property>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18,2)"); // Explicitly set precision and scale for Price
}
}

public interface IRealEstatateDbContext
{
    DbSet<Property> Properties { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
    EntityEntry Entry(object entity);
}