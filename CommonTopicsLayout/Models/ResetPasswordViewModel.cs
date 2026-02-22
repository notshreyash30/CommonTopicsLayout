using System.ComponentModel.DataAnnotations;

namespace CommonTopicsLayout.Models
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}