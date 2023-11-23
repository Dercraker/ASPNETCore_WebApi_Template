using FluentValidation;
using Template.Domain.Dto.TodoTask;
using Template.Domain.ErrorCodes;

namespace Template.API.Validator.TodoTask;

public class UpdateTodoTaskValidator : AbstractValidator<UpdateTodoTaskDto>
{
    public UpdateTodoTaskValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.DtoNull))
            .WithMessage("DTO canno't be null");

        RuleFor(dto => dto.IdTask)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.TaskIdNullOrEmpty))
            .WithMessage("TaskId is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.IdTask)
            .NotEmpty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.TaskIdNullOrEmpty))
            .WithMessage("TaskId is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.TaskName)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.TaskNameNullOrEmpty))
            .WithMessage("TaskName is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.TaskName)
            .NotEmpty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.TaskNameNullOrEmpty))
            .WithMessage("TaskName is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.Start)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.StartDateNullOrEmpty))
            .WithMessage("Start date is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.Start)
            .NotEmpty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.StartDateNullOrEmpty))
            .WithMessage("Start date is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.End)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.EndDateNullOrEmpty))
            .WithMessage("End date is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.End)
            .NotEmpty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.EndDateNullOrEmpty))
            .WithMessage("End date is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.Description)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.DescriptionNullOrEmpty))
            .WithMessage("Description is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.DescriptionNullOrEmpty))
            .WithMessage("Description is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.IdUser)
            .NotNull()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.IdUserNullOrEmpty))
            .WithMessage("IdUser is required")
            .Unless(dto => dto is null);
        RuleFor(dto => dto.IdUser)
            .NotEmpty()
            .WithErrorCode(nameof(ETodoTaskErrorCodes.IdUserNullOrEmpty))
            .WithMessage("IdUser is required")
            .Unless(dto => dto is null);
    }
}
