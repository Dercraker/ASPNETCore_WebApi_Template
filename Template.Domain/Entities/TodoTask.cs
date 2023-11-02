namespace Template.Domain.Entities;
public class TodoTask
{
    public Guid IdTask { get; set; }
    public string TaskName { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Description { get; set; }
    public Guid IdUser { get; set; }
    public virtual UserApi? User { get; set; }
}
