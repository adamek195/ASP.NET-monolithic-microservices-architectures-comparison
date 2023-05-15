using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Multimedia.Images.Data;

namespace Multimedia.Images.Data
{
    public static class DataBaseMigrator
    {
        public static void AddMigration(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                Migrate(serviceScope.ServiceProvider.GetService<ImagesMultimediaContext>());
            }
        }

        public static void Migrate(ImagesMultimediaContext imagesMultimediaContext)
        {
            imagesMultimediaContext.Database.Migrate();
        }
    }
}
