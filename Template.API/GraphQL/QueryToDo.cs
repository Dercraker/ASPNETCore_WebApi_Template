using Microsoft.AspNetCore.Mvc;
using Template.Domain.Entities;
using Template.Platform.Interfaces;

namespace Template.API.GraphQL;

public class QueryToDo
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
}
