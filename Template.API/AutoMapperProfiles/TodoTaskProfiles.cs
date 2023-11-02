using AutoMapper;
using Template.Domain.Dto.TodoTask;
using Template.Domain.Entities;

namespace Template.API.AutoMapperProfiles;

public class TodoTaskProfiles : Profile
{
    public TodoTaskProfiles()
    {
        CreateMap<CreateTodoTaskDto, TodoTask>();
        CreateMap<TodoTask, TodoTaskDto>();
    }
}
