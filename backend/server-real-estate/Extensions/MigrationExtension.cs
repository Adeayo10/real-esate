using Microsoft.EntityFrameworkCore;
using server_real_estate.Database;

namespace server_real_estate.Extensions;
public static class MigrationExtension
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
        dbContext.Database.Migrate();
    }   
}
