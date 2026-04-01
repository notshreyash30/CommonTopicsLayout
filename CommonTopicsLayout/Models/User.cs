using System;
using System.Collections.Generic;

namespace CommonTopicsLayout.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public string? Bio { get; set; }

    public string? ProfilePicturePath { get; set; }
}
