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

    /// <inheritdoc/>
    public async Task DeleteAsync(TodoTask todoTask) => await _todoTaskProvider.RemoveAsync(todoTask);

    /// <inheritdoc/>
    public IQueryable<TodoTask> GetAll() => _todoTaskProvider.GetAll();

    /// <inheritdoc/>
    public IQueryable<TodoTask> GetById(Guid todoTaskId) => _todoTaskProvider.GetById(todoTaskId);

    /// <inheritdoc/>
    public async Task<TodoTask?> GetByIdAsync(Guid todoTaskId) => await _todoTaskProvider.GetByIdAsync(todoTaskId);

    /// <inheritdoc/>
    public async Task<TodoTask> UpdateAsync(TodoTask todoTask, UpdateTodoTaskDto updateDto) => await _todoTaskProvider.UpdateAsync(todoTask, updateDto);
}
