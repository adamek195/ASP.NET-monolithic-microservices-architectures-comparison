using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Multimedia.Users.Entities;
using System;

namespace Multimedia.Users.Data
{
    public class UsersMultimediaContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public UsersMultimediaContext(DbContextOptions<UsersMultimediaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
