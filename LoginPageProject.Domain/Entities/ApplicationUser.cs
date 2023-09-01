using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int LoginAttempts { get; set; }
    public DateTime BlockedTo { get; set; }
    
    public List<OldUserPassword> OldUserPasswords { get; set; } = new();
    public List<UserActionInfo> UserActionInfos { get; set; } = new();
}