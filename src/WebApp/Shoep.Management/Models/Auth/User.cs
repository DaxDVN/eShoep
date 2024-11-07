using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Shoep.Management.Models.Auth;

public class User : IdentityUser
{
    [MaxLength(100)] public string? FirstName { get; set; }
    [MaxLength(100)] public string? LastName { get; set; }
    [MaxLength(100)] public string? Address { get; set; }
    [MaxLength(100)] public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}