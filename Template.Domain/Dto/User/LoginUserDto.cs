namespace Template.Domain.Dto.User;
public class LoginUserDto
{
    /// <summary>
    /// Username Or Email
    /// </summary>
    public string Login { get; set; }
    public string Password { get; set; }
}