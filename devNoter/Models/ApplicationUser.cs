using Microsoft.AspNetCore.Identity;

namespace devNoter.Models
{
    public class ApplicationUser : IdentityUser
    {
        // IdentityUser already includes alot of useful properties:
        // Id, UserName, Email, PasswordHash, PhoneNumber, etc.
        // Additional custom properties can be added here
        public string? FullName { get; set; }
    }
}
