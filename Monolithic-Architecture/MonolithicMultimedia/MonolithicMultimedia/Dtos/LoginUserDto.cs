using System.ComponentModel.DataAnnotations;

namespace MonolithicMultimedia.Dtos
{
    public class LoginUserDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email of the user is required")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
