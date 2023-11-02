using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;

namespace Template.EFCore.Interfaces;
public interface ITodoTaskRepository : IGenericRepository<TodoTask>
{
    Task<TodoTask> UpdateAsync(TodoTask todoTask, UpdateTodoTaskDto updateDto);
}
