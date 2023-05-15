using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Multimedia.Users.Data
{
    public static class DataBaseMigrator
    {
        public static void AddMigration(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                Migrate(serviceScope.ServiceProvider.GetService<UsersMultimediaContext>());
            }
        }

        public static void Migrate(UsersMultimediaContext usersMultimediaContext)
        {
            usersMultimediaContext.Database.Migrate();
        }
    }
}
