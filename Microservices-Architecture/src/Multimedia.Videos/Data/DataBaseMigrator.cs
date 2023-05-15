using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Multimedia.Videos.Data
{
    public static class DataBaseMigrator
    {
        public static void AddMigration(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                Migrate(serviceScope.ServiceProvider.GetService<VideosMultimediaContext>());
            }
        }

        public static void Migrate(VideosMultimediaContext videosMultimediaContext)
        {
            videosMultimediaContext.Database.Migrate();
        }
    }
}
