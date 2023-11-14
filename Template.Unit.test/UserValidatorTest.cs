using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.API.Validator.User;
using Template.Domain.Dto.User;
using Template.Domain.ErrorCodes;
using Xunit;

namespace Template.Unit.test;

public class UserValidatorTest
{
    #region Create User Validator
    [Theory]
    [MemberData(nameof(CreateUserDtoWithInvalidUsername))]
    [MemberData(nameof(CreateUserDtoWithInvalidPassword))]
    [MemberData(nameof(CreateUserDtoWithInvalidEmail))]
    public void RegisterUserValidator_WithInvalidData_ShouldHaveErrors(RegisterUserDto userDto, EUserErrorCodes expectedErrorCode)
    {
        // Arrange
        var validator = new RegisterUserValidator();

        // Act
        ValidationResult validationResult = validator.Validate(userDto);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(error => error.ErrorCode == expectedErrorCode.ToString());
    }

    public static TheoryData<RegisterUserDto, EUserErrorCodes> CreateUserDtoWithInvalidUsername()
    {
        return new TheoryData<RegisterUserDto, EUserErrorCodes>
            {
                {
                    // UserDto with empty UserName
                    new RegisterUserDto { UserName = string.Empty, Password = "password", Email = "test@example.com" },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // UserDto with null UserName
                    new RegisterUserDto { UserName = null, Password = "password", Email = "test@example.com" },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // UserDto with whitespace UserName
                    new RegisterUserDto { UserName = "      ", Password = "password", Email = "test@example.com" },
                    EUserErrorCodes.UserEmailNullOrEmpty
                }
            };
    }
    public static TheoryData<RegisterUserDto, EUserErrorCodes> CreateUserDtoWithInvalidPassword()
    {
        return new TheoryData<RegisterUserDto, EUserErrorCodes>
            {
                {
                    // UserDto with empty Password
                    new RegisterUserDto { UserName = "username", Password = string.Empty, Email = "test@example.com" },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                },
                {
                    // UserDto with null Password
                    new RegisterUserDto { UserName = "username", Password = null, Email = "test@example.com" },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                },
                {
                    // UserDto with whitespace Password
                    new RegisterUserDto { UserName = "username", Password = "         ", Email = "test@example.com" },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                }
            };
    }
    public static TheoryData<RegisterUserDto, EUserErrorCodes> CreateUserDtoWithInvalidEmail()
    {
        return new TheoryData<RegisterUserDto, EUserErrorCodes>
            {
                {
                    // UserDto with empty Email
                    new RegisterUserDto { UserName = "username", Password = "password", Email = string.Empty },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // UserDto with null Email
                    new RegisterUserDto { UserName = "username", Password = "password", Email = null },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // UserDto with whitespaces Email
                    new RegisterUserDto { UserName = "username", Password = "password", Email = "     " },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // UserDto with invalid Email
                    new RegisterUserDto { UserName = "username", Password = "password", Email = "invalid_email" },
                    EUserErrorCodes.UserEmailNotValid
                }
            };
    } 
    #endregion

    #region Login User Validator
    [Theory]
    [MemberData(nameof(LoginUserDtoWithInvalidLogin))]
    [MemberData(nameof(LoginUserDtoWithInvalidPassword))]
    public void LoginUserValidator_WithInvalidLogin_ShouldHaveErrors(LoginUserDto userDto, EUserErrorCodes expectedErrorCode)
    {
        // Arrange
        var validator = new LoginUserValidator();

        // Act
        ValidationResult validationResult = validator.Validate(userDto);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(error => error.ErrorCode == expectedErrorCode.ToString());
    }

    public static TheoryData<LoginUserDto, EUserErrorCodes> LoginUserDtoWithInvalidLogin()
    {
        return new TheoryData<LoginUserDto, EUserErrorCodes>
            {
                {
                    // Empty Login
                    new LoginUserDto { Login = string.Empty, Password = "password" },
                    EUserErrorCodes.UserUserNameNullOrEmpty
                },
                {
                    // Whitespace Login
                    new LoginUserDto { Login = "         ", Password = "password" },
                    EUserErrorCodes.UserUserNameNullOrEmpty
                },
                {
                    // Null Login
                    new LoginUserDto { Login = null, Password = "password" },
                    EUserErrorCodes.UserUserNameNullOrEmpty
                }
            };
    }
    public static TheoryData<LoginUserDto, EUserErrorCodes> LoginUserDtoWithInvalidPassword()
    {
        return new TheoryData<LoginUserDto, EUserErrorCodes>
            {
                {
                    // Empty Password
                    new LoginUserDto { Login = "username", Password = string.Empty },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                },
                {
                    // Whitespace Password
                    new LoginUserDto { Login = "username", Password = "         " },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                },
                {
                    // Null Password
                    new LoginUserDto { Login = "username", Password = null },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                }
            };
    }
    #endregion

    #region Forgot Password Validator
    [Theory]
    [MemberData(nameof(ForgotPasswordDtoWithInvalidEmail))]
    public void ForgotPasswordValidator_WithInvalidEmail_ShouldHaveErrors(ForgotPasswordDto userDto, EUserErrorCodes expectedErrorCode)
    {
        // Arrange
        var validator = new ForgotPasswordValidator();

        // Act
        ValidationResult validationResult = validator.Validate(userDto);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(error => error.ErrorCode == expectedErrorCode.ToString());
    }

    public static TheoryData<ForgotPasswordDto, EUserErrorCodes> ForgotPasswordDtoWithInvalidEmail()
    {
        return new TheoryData<ForgotPasswordDto, EUserErrorCodes>
            {
                {
                    // Empty Email
                    new ForgotPasswordDto { Email = string.Empty },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // Whitespace Email
                    new ForgotPasswordDto { Email = "         " },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // Null Email
                    new ForgotPasswordDto { Email = null },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // Invalid Email format
                    new ForgotPasswordDto { Email = "invalid_email" },
                    EUserErrorCodes.UserEmailNotValid
                }
            };
    }
    #endregion

    #region Reset Password Validator
    [Theory]
    [MemberData(nameof(ResetPasswordDtoWithInvalidToken))]
    [MemberData(nameof(ResetPasswordDtoWithInvalidEmail))]
    [MemberData(nameof(ResetPasswordDtoWithInvalidPassword))]
    [MemberData(nameof(ResetPasswordDtoWithInvalidConfirmPassword))]
    public void ResetPasswordValidator_WithInvalidData_ShouldHaveErrors(ResetPasswordDto userDto, EUserErrorCodes expectedErrorCode)
    {
        // Arrange
        var validator = new ResetPasswordValidator();

        // Act
        ValidationResult validationResult = validator.Validate(userDto);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(error => error.ErrorCode == expectedErrorCode.ToString());
    }

    public static TheoryData<ResetPasswordDto, EUserErrorCodes> ResetPasswordDtoWithInvalidToken()
    {
        return new TheoryData<ResetPasswordDto, EUserErrorCodes>
            {
                {
                    // Null Token
                    new ResetPasswordDto { Token = null, Email = "test@example.com", Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserTokenNullOrEmpty
                },
                {
                    // Whitespace Token
                    new ResetPasswordDto { Token = "     ", Email = "test@example.com", Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserTokenNullOrEmpty
                },
                {
                    // Empty Token
                    new ResetPasswordDto { Token = "", Email = "test@example.com", Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserTokenNullOrEmpty
                },
            };
    }
    public static TheoryData<ResetPasswordDto, EUserErrorCodes> ResetPasswordDtoWithInvalidEmail()
    {
        return new TheoryData<ResetPasswordDto, EUserErrorCodes>
            {
                {
                    // Null Email
                    new ResetPasswordDto { Token = "token", Email = null, Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // Withespace Email
                    new ResetPasswordDto { Token = "token", Email = "       ", Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // Empty Email
                    new ResetPasswordDto { Token = "token", Email = "", Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserEmailNullOrEmpty
                },
                {
                    // Invalid Email format
                    new ResetPasswordDto { Token = "token", Email = "invalid_email", Password = "password", ConfirmPassword = "password" },
                    EUserErrorCodes.UserEmailNotValid
                }
            };
    }
    public static TheoryData<ResetPasswordDto, EUserErrorCodes> ResetPasswordDtoWithInvalidPassword()
    {
        return new TheoryData<ResetPasswordDto, EUserErrorCodes>
            {
                {
                    // Null Password
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = null, ConfirmPassword = "password" },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                },
                {
                    // Empty Password
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = "", ConfirmPassword = "password" },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                },
                {
                    // WhiteSpace Password
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = "             ", ConfirmPassword = "password" },
                    EUserErrorCodes.UserPasswordNullOrEmpty
                }
            };
    }
    public static TheoryData<ResetPasswordDto, EUserErrorCodes> ResetPasswordDtoWithInvalidConfirmPassword()
    {
        return new TheoryData<ResetPasswordDto, EUserErrorCodes>
            {
                {
                    // Null ConfirmPassword
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = "password", ConfirmPassword = null },
                    EUserErrorCodes.UserConfirmPasswordNullOrEmpty
                },
                {
                    // Empty ConfirmPassword
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = "password", ConfirmPassword = "" },
                    EUserErrorCodes.UserConfirmPasswordNullOrEmpty
                },
                {
                    // WhiteSpace ConfirmPassword
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = "password", ConfirmPassword = "          " },
                    EUserErrorCodes.UserConfirmPasswordNullOrEmpty
                },
                {
                    // Password and ConfirmPassword mismatch
                    new ResetPasswordDto { Token = "token", Email = "test@example.com", Password = "password", ConfirmPassword = "mismatched_password" },
                    EUserErrorCodes.UserPasswordAndConfirmPasswordNotMatch
                }
            };
    } 
    #endregion
}
