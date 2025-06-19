using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    public string? FirstName { get; set; }
    
    [StringLength(100)]
    public string? LastName { get; set; }
    
    public int? Age { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required] 
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Range(13, 120)]
    public int? Age { get; set; }
}

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}

public class UserProfileViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Age { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<string> Claims { get; set; } = new();
}