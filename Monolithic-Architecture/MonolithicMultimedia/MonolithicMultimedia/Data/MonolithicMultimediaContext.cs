using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonolithicMultimedia.Entities;
using System;

namespace MonolithicMultimedia.Data
{
    public class MonolithicMultimediaContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MonolithicMultimediaContext(DbContextOptions<MonolithicMultimediaContext> options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
