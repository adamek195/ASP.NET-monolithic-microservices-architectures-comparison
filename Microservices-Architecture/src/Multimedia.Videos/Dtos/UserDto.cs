using System;
using System.ComponentModel.DataAnnotations;

namespace Multimedia.Videos.Dtos
{
    public class UserDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
