using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;
using Template.Platform.Interfaces;
using Template.Provider.Interfaces;

namespace Template.Platform;
public class TodoTaskPlatform : ITodoTaskPlatform
{
    #region Props
    private readonly ITodoTaskProvider _todoTaskProvider;
    #endregion

    #region Ctor
    public TodoTaskPlatform(ITodoTaskProvider todoTaskProvider)
    {
        _todoTaskProvider = todoTaskProvider ?? throw new ArgumentNullException(nameof(todoTaskProvider));
    }

    #endregion

    /// <inheritdoc/>
    public async Task CreateTodoTaskAsync(TodoTask todoTask) => await _todoTaskProvider.CreateTodoTaskAsync(todoTask);

    public async Task DeleteAsync(TodoTask todoTask) => await _todoTaskProvider.RemoveAsync(todoTask);
    public IQueryable<TodoTask> GetAll() => _todoTaskProvider.GetAll();
    public async Task<TodoTask?> GetByIdAsync(Guid todoTaskId) => await _todoTaskProvider.GetByIdAsync(todoTaskId);
    public async Task<TodoTask> UpdateAsync(TodoTask todoTask, UpdateTodoTaskDto updateDto) => await _todoTaskProvider.UpdateAsync(todoTask, updateDto);
}
