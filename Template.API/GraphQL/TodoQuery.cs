using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Template.API.Validator.TodoTask;
using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;
using Template.Platform;
using Template.Platform.Interfaces;

namespace Template.API.GraphQL;

public class TodoQuery
{
    /// <summary>
    /// Get All Todo Task With Graph QL
    /// </summary>
    /// <returns>IQueryable of TodoTask</returns>
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TodoTask> GetTodoTasks([FromServices] ITodoTaskPlatform todoTaskPlatform)
    {
        if (todoTaskPlatform is null)
            throw new ArgumentNullException(nameof(todoTaskPlatform));
        return todoTaskPlatform.GetAll();
    }

    /// <summary>
    /// Get one Task by this ID
    /// </summary>
    /// <param name="todoTaskId">id of wanted task</param>
    /// <returns></returns>
    [UseProjection]
    public IQueryable<TodoTask> GetTodoTask([FromServices] ITodoTaskPlatform todoTaskPlatform, Guid todoTaskId)
    {
        return todoTaskPlatform.GetById(todoTaskId);
    }

    /// <summary>
    /// Update TodoTask
    /// </summary>
    /// <param name="todoTaskPlatform"></param>
    /// <param name="updateTodoTaskValidator"></param>
    /// <param name="todoTaskId"></param>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    [UseProjection]
    public async Task<TodoTask> UpdateTodoTask([FromServices] ITodoTaskPlatform todoTaskPlatform,
                                               Guid todoTaskId,
                                               UpdateTodoTaskDto updateDto)
    {
        TodoTask? todoTask = await todoTaskPlatform.GetByIdAsync(todoTaskId);

        todoTask = await todoTaskPlatform.UpdateAsync(todoTask, updateDto);

        return todoTask;
    }

    [UseProjection]
    public async Task DeleteTodoTask([FromServices] ITodoTaskPlatform todoTaskPlatform, Guid todoTaskId)
    {
        TodoTask? todoTask = await todoTaskPlatform.GetByIdAsync(todoTaskId);
        await todoTaskPlatform.DeleteAsync(todoTask);
    }
}
