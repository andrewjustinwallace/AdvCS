using System.Security.Claims;

namespace OAuthDemo.Models;

public class UserProfile
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public List<ClaimInfo> Claims { get; set; } = new();
}

public class ClaimInfo
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class AuthenticationProvider
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}