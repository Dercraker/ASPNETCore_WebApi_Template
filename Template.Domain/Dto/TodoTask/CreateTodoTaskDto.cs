namespace Template.Domain.Dto.TodoTask;
public class CreateTodoTaskDto
{
    public string TaskName { get; set; }
    public string Description { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public Guid IdUser { get; set; }
}
