namespace Template.Domain.Models;
public class HttpError
{
    public record Error(string Code, string Message);
}