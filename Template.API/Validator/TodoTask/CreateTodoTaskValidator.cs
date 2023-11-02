using FluentValidation;
using Template.Domain.Dto.TodoTask;
using Template.Domain.ErrorCodes;

namespace Template.API.Validator.TodoTask;

public class CreateTodoTaskValidator : AbstractValidator<CreateTodoTaskDto>
{
    public CreateTodoTaskValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.DtoNull))
            .WithMessage("Create todo task canno't be null");

        RuleFor(dto => dto.TaskName)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.TaskNameNullOrEmpty))
            .WithMessage("Task name is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.TaskName)
            .Empty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.TaskNameNullOrEmpty))
            .WithMessage("Task name is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.Description)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.DescriptionNullOrEmpty))
            .WithMessage("Description is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.Description)
            .Empty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.DescriptionNullOrEmpty))
            .WithMessage("Description is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.IdUser)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.IdUserNullOrEmpty))
            .WithMessage("IdUser is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.IdUser)
            .Empty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.IdUserNullOrEmpty))
            .WithMessage("IdUser is required")
            .Unless(dto => dto is null);
    }
}
