using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            if(dbContextService.Database.GetPendingMigrations().Any()) 
                dbContextService.Database.Migrate();

            return app;
        }

        public static WebApplication SeedDatabase (this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataInitializerService = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            dataInitializerService.Initialize();

            return app;
        }
    }
}
