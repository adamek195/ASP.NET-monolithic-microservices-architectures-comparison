using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MonolithicMultimedia.Data
{
    public static class DataBaseMigrator
    {
        public static void AddMigration(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                Migrate(serviceScope.ServiceProvider.GetService<MonolithicMultimediaContext>());
            }
        }

        public static void Migrate(MonolithicMultimediaContext monolithicMultimediaContext)
        {
            monolithicMultimediaContext.Database.Migrate();
        }
    }
}
