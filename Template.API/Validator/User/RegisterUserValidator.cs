using FluentValidation;
using Template.Domain.Dto.User;
using Template.Domain.ErrorCodes;

namespace Template.API.Validator.User;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(EUserErrorCodes.UserDtoNull))
            .WithMessage("Register user dto canno't be null");

        RuleFor(dto => dto.UserName).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty)).WithMessage("Username is required").Unless(dto => dto is null);
        RuleFor(dto => dto.UserName).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty)).WithMessage("Username is required").Unless(dto => dto is null);

        RuleFor(dto => dto.Password).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserPasswordNullOrEmpty)).WithMessage("Password is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Password).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserPasswordNullOrEmpty)).WithMessage("Password is required").Unless(dto => dto is null);

        RuleFor(dto => dto.Email).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty)).WithMessage("Email is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Email).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty)).WithMessage("Email is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Email).EmailAddress().WithErrorCode(nameof(EUserErrorCodes.UserEmailNotValid)).WithMessage("Email is not valid").Unless(dto => dto is null);
    }
}
