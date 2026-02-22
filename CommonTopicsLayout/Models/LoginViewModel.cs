using System.ComponentModel.DataAnnotations;

namespace CommonTopicsLayout.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Identity is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}