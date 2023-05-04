using Microsoft.EntityFrameworkCore;
using Multimedia.Videos.Entities;

namespace Multimedia.Videos.Data
{
    public class VideosMultimediaContext : DbContext
    {
        public VideosMultimediaContext(DbContextOptions<VideosMultimediaContext> options) : base(options)
        {
        }

        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
