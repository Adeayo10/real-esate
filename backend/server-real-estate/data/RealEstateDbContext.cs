using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;using server_real_estate.Model;

namespace server_real_estate.Data;
public class RealEstateDbContext : DbContext, IRealEstatateDbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
    }
    public DbSet<Property> Properties { get; set; }
    public DbSet<User> Users { get; set; }
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
}

public interface IRealEstatateDbContext
{
    DbSet<Property> Properties { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
    int SaveChanges();
    EntityEntry Entry(object entity);
   
}

