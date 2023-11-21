using Microsoft.EntityFrameworkCore;
using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;
using Template.EFCore.Interfaces;

namespace Template.EFCore.Repositories;
public class TodoTaskRepository : GenericRepository<TodoTask>, ITodoTaskRepository
{
    #region Ctor
    public TodoTaskRepository(TemplateContext _context) : base(_context)
    {
    }

    public IQueryable<TodoTask> GetTaskById(Guid todoTaskId) => _context.Tasks.AsQueryable().Where(t => t.IdTask == todoTaskId);

    #endregion
    public async Task<TodoTask> UpdateAsync(TodoTask todoTask, UpdateTodoTaskDto updateDto)
    {
        todoTask.TaskName = updateDto.TaskName;
        todoTask.Start = updateDto.Start;
        todoTask.End = updateDto.End;
        todoTask.Description = updateDto.Description;
        todoTask.IdUser = updateDto.IdUser;

        await _context.SaveChangesAsync();

        return todoTask;
    }
}
