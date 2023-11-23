using FluentValidation;
using Template.Domain.Dto.User;
using Template.Domain.ErrorCodes;

namespace Template.API.Validator.User;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(dto => dto)
            .NotNull()
            .WithErrorCode(nameof(EUserErrorCodes.UserDtoNull))
            .WithMessage("Reset password dto canno't be null");

        RuleFor(dto => dto.Token).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserTokenNullOrEmpty)).WithMessage("Token is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Token).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserTokenNullOrEmpty)).WithMessage("Token is required").Unless(dto => dto is null);

        RuleFor(dto => dto.Email).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty)).WithMessage("Email is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Email).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserEmailNullOrEmpty)).WithMessage("Email is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Email).EmailAddress().WithErrorCode(nameof(EUserErrorCodes.UserEmailNotValid)).WithMessage("Token is not valid").Unless(dto => dto is null);

        RuleFor(dto => dto.Password).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserPasswordNullOrEmpty)).WithMessage("Password is required").Unless(dto => dto is null);
        RuleFor(dto => dto.Password).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserPasswordNullOrEmpty)).WithMessage("Password is required").Unless(dto => dto is null);

        RuleFor(dto => dto.ConfirmPassword).NotNull().WithErrorCode(nameof(EUserErrorCodes.UserConfirmPasswordNullOrEmpty)).WithMessage("ConfirmPassword is required").Unless(dto => dto is null);
        RuleFor(dto => dto.ConfirmPassword).NotEmpty().WithErrorCode(nameof(EUserErrorCodes.UserConfirmPasswordNullOrEmpty)).WithMessage("ConfirmPassword is required").Unless(dto => dto is null);
        RuleFor(dto => dto).Must(dto => dto.Password == dto.ConfirmPassword)
                           .WithErrorCode(nameof(EUserErrorCodes.UserPasswordAndConfirmPasswordNotMatch))
                           .WithMessage("Password and ConfirmPassword not match")
                           .Unless(dto => dto is null);
    }
}
