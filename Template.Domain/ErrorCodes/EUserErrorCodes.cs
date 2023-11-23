namespace Template.Domain.ErrorCodes;
public enum EUserErrorCodes
{
    UserDtoNull = 101,
    UserUserNameNullOrEmpty = 102,
    UserPasswordNullOrEmpty = 103,
    UserEmailNullOrEmpty = 104,
    UserIdNullOrEmpty = 105,
    UserAlreadyExist = 106,
    UserNotFoundById = 107,
    UserEmailNotValid = 108,
    UserEmailAlreadyUsed = 109,
    UserTokenNullOrEmpty = 110,
    UserConfirmPasswordNullOrEmpty = 111,
    UserPasswordAndConfirmPasswordNotMatch = 112,
    UserNotFoundByLogin = 113
}