using FluentValidation;
using Template.Domain.Dto.User;
using Template.Domain.ErrorCodes;

namespace Template.API.Validator.User;

public class LoginUserValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(EUserErrorCodes.UserDtoNull))
            .WithMessage("Login user dto canno't be null");

        RuleFor(dto => dto.Login).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserUserNameNullOrEmpty))
            .WithMessage("Login is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Login).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserUserNameNullOrEmpty))
            .WithMessage("Login is required").Unless(dto => dto is null);

        RuleFor(dto => dto.Password).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserPasswordNullOrEmpty))
            .WithMessage("Password is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Password).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserPasswordNullOrEmpty))
            .WithMessage("Password is required").Unless(dto => dto is null);
    }
}
