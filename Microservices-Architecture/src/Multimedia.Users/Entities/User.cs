using Microsoft.AspNetCore.Identity;
using System;

namespace Multimedia.Users.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
