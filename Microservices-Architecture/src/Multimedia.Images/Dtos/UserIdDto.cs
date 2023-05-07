using System;
using System.ComponentModel.DataAnnotations;

namespace Multimedia.Images.Dtos
{
    public class UserIdDto
    {
        [Required]
        public string UserId { get; set; }

    }
}
