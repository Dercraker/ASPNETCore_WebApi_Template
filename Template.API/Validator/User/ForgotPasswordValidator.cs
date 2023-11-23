using FluentValidation;
using Template.Domain.Dto.User;
using Template.Domain.ErrorCodes;

namespace Template.API.Validator.User;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(EUserErrorCodes.UserDtoNull))
            .WithMessage("Forgot password Dto canno't be null");

        RuleFor(dto => dto.Email)
            .NotNull()
            .WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty))
            .WithMessage("Email is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty))
            .WithMessage("Email is required")
            .Unless(dto => dto is null);

        RuleFor(dto => dto.Email)
            .EmailAddress()
            .WithErrorCode(nameof(EUserErrorCodes.UserEmailNotValid))
            .WithMessage("Email is not valid")
            .Unless(dto => dto is null);
    }
}
