using LoginPageProject.Domain.Common;

namespace LoginPageProject.Domain.Entities;

public class UserActionInfo : AuditableEntity
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string Action { get; set; }
    public DateTime ActionDate { get; set; }
}