using System.ComponentModel.DataAnnotations;

namespace Multimedia.Videos.Dtos
{
    public class UserIdDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
