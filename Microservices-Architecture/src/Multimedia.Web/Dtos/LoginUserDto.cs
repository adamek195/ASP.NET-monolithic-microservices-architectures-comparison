using System.ComponentModel.DataAnnotations;

namespace Multimedia.Web.Dtos
{
    public class LoginUserDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email of the user is required.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password of the user is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
