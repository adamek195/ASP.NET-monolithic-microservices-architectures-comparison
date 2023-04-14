using System.ComponentModel.DataAnnotations;

namespace MonolithicMultimedia.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First Name of the user is required.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name of the user is required.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email of the user is required.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
