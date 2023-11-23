using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Template.Domain.Dto.Token;
using Template.Domain.Entities;
using Template.Domain.Settings;
using Template.Platform.Interfaces;

namespace Template.Platform;
public class UserPlatform : IUserPlatform
{
    #region Props
    private readonly UserManager<UserApi> _userManager;
    private readonly JWTSettings _jwtSettings;
    #endregion

    #region CTOR
    public UserPlatform(UserManager<UserApi> userManager, JWTSettings jwtSettings)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
    }


    #endregion

    #region Methods
    /// <inheritdoc/>
    public async Task<UserApi?> GetByNameAsync(string username) => await _userManager.FindByNameAsync(username);

    /// <inheritdoc/>
    public async Task<UserApi?> GetByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

    /// <inheritdoc/>
    public async Task<UserApi?> GetByIdAsync(Guid id) => await _userManager.FindByIdAsync(id.ToString());

    /// <inheritdoc/>
    public async Task<bool> ExistAsync(Guid? id, string? username, string? email)
    {
        UserApi? user = null;

        if (id is not null)
            user ??= await _userManager.FindByIdAsync(id.ToString());

        if (username is not null)
            user ??= await _userManager.FindByNameAsync(username);

        if (email is not null)
            user ??= await _userManager.FindByEmailAsync(email);

        return user is not null;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult?> CreateAsync(UserApi user, string password) => await _userManager.CreateAsync(user, password);

    /// <inheritdoc/>
    public async Task<IdentityResult?> AddRoleAsync(UserApi user, string role) => await _userManager.AddToRoleAsync(user, role);

    /// <inheritdoc/>
    public async Task<bool> CheckPasswordAsync(UserApi user, string password) => await _userManager.CheckPasswordAsync(user, password);

    /// <inheritdoc/>
    public async Task<TokenDto> LoginAsync(UserApi user)
    {
        IList<string>? userRoles = await _userManager.GetRolesAsync(user);

        List<Claim> authClaims = new List<Claim>
            {
                new Claim("Username", user.UserName),
                new Claim("Email", user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (string userRole in userRoles)
        {
            authClaims.Add(new Claim("Roles", userRole));
        }

        SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationTime),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new()
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };
    }

    /// <inheritdoc/>
    public bool TestTokenValidity(string token)
    {
        SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = _jwtSettings.ValidIssuer,
                ValidAudience = _jwtSettings.ValidAudience,
                IssuerSigningKey = authSigningKey
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public async Task<string> GeneratePasswordResetTokenAsync(UserApi user) => await _userManager.GeneratePasswordResetTokenAsync(user);

    /// <inheritdoc/>
    public async Task<IdentityResult?> ResetPasswordAsync(UserApi user, string token, string password) => await _userManager.ResetPasswordAsync(user, token, password);

    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteAsync(UserApi user) => await _userManager.DeleteAsync(user);
    #endregion
}
