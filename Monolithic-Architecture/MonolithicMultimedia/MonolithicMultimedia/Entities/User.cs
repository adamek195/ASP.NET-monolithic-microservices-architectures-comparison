using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MonolithicMultimedia.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }


        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}
