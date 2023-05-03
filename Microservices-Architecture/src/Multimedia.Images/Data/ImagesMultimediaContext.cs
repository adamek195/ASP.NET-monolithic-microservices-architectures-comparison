using Microsoft.EntityFrameworkCore;
using Multimedia.Images.Entities;

namespace Multimedia.Images.Data
{
    public class ImagesMultimediaContext : DbContext
    {
        public ImagesMultimediaContext(DbContextOptions<ImagesMultimediaContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
