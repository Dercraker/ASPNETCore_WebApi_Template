using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;

namespace Template.Platform.Interfaces;
public interface ITodoTaskPlatform
{
    /// <summary>
    /// Create Todo Task Async
    /// </summary>
    /// <param name="todoTask">todoTask To create</param>
    /// <returns>A Task with Todo OR Null</returns>
    Task CreateTodoTaskAsync(TodoTask todoTask);

    /// <summary>
    /// Remove one Entity
    /// </summary>
    /// <param name="todoTask">the entity to remove</param>
    /// <returns>A Task</returns>
    Task DeleteAsync(TodoTask todoTask);

    /// <summary>
    /// Get All Todo Task
    /// </summary>
    /// <returns>Iqueryable with TodoTask</returns>
    IQueryable<TodoTask> GetAll();

    /// <summary>
    /// Get one TodoTask by its id
    /// </summary>
    /// <param name="todoTaskId">Id of wanted Todo Task</param>
    /// <returns>
    /// </returns>
    Task<TodoTask?> GetByIdAsync(Guid todoTaskId);

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="todoTask">Task to update</param>
    /// <param name="updateDto">new values</param>
    /// <returns>new Updated Task</returns>
    Task<TodoTask> UpdateAsync(TodoTask todoTask, UpdateTodoTaskDto updateDto);
}
