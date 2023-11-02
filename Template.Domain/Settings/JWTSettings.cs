namespace Template.Domain.Settings;
public class JWTSettings
{
    public string Secret { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int DurationTime { get; set; }
}