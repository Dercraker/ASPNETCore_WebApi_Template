namespace Template.Domain.Settings;
public class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Unknown = "Unknown";

    public static List<string> GetAllRoles() => new() { Admin, User, Unknown };
}
