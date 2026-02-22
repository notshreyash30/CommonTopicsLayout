using System.ComponentModel.DataAnnotations;

namespace CommonTopicsLayout.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        // Password Reset Fields (Must be Nullable ?)
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}