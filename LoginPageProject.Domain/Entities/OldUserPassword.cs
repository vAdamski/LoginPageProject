using LoginPageProject.Domain.Common;

namespace LoginPageProject.Domain.Entities;

public class OldUserPassword : AuditableEntity
{
    public string UserId { get; set; }
    public string OldPassword { get; set; } = "";
}