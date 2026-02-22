using System.ComponentModel.DataAnnotations;

namespace CommonTopicsLayout.Models
{
    public class SubscribeModel
    {
        [Required(ErrorMessage = "We require an email address to join the circle.")]
        [EmailAddress(ErrorMessage = "Please enter a valid architectural email address.")]
        public string Email { get; set; }
    }
}