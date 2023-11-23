using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;
using Template.EFCore.Interfaces;
using Template.Provider.Interfaces;

namespace Template.Provider;
public class TodoTaskProvider : ITodoTaskProvider
{
    #region Props
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Ctor
    public TodoTaskProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    #endregion

    /// <inheritdoc/>
    public async Task CreateTodoTaskAsync(TodoTask todoTask)
    {
        await _unitOfWork.TodoTasks.AddAsync(todoTask);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public IQueryable<TodoTask> GetAll() => _unitOfWork.TodoTasks.GetAll();

    /// <inheritdoc/>
    public IQueryable<TodoTask> GetById(Guid todoTaskId) => _unitOfWork.TodoTasks.GetTaskById(todoTaskId);

    /// <inheritdoc/>
    public async Task<TodoTask?> GetByIdAsync(Guid todoTaskId) => await _unitOfWork.TodoTasks.GetByIdAsync(todoTaskId);

    /// <inheritdoc/>
    public async Task RemoveAsync(TodoTask todoTask)
    {
        _unitOfWork.TodoTasks.Remove(todoTask);
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc/>
    public async Task<TodoTask> UpdateAsync(TodoTask todoTask, UpdateTodoTaskDto updateDto)
    {
        todoTask = await _unitOfWork.TodoTasks.UpdateAsync(todoTask, updateDto);
        await _unitOfWork.CompleteAsync();
        return todoTask;
    }
}
