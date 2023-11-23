using Microsoft.AspNetCore.Identity;

namespace Template.Domain.Entities;
public class UserApi : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    public IList<TodoTask> Tasks { get; set; } = new List<TodoTask>();
}
