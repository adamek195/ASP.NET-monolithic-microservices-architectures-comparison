using System;
using System.ComponentModel.DataAnnotations;

namespace Multimedia.Images.Dtos
{
    public class UserDto
    {
        [Required]
        public Guid UserId { get; set; }

    }
}
