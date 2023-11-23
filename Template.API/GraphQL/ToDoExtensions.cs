using Template.Domain.Entities;

namespace Template.API.GraphQL;

public class ToDoExtensions : ObjectTypeExtension<TodoTask>
{
    protected override void Configure(IObjectTypeDescriptor<TodoTask> descriptor) => base.Configure(descriptor);
}
