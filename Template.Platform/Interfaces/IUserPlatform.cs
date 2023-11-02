using Microsoft.AspNetCore.Identity;
using Template.Domain.Dto.Token;
using Template.Domain.Entities;

namespace Template.Platform.Interfaces;
public interface IUserPlatform
{
    /// <summary>
    /// Get an user by name Async
    /// </summary>
    /// <param name="username">The name of wanted user</param>
    /// <returns>A Task with user OR null</returns>
    Task<UserApi?> GetByNameAsync(string username);

    /// <summary>
    /// Get an user by email Async
    /// </summary>
    /// <param name="email">The email of wanted user</param>
    /// <returns>A Task with user OR null</returns>
    Task<UserApi?> GetByEmailAsync(string email);
    /// <summary>
    /// Get an user by Id Async
    /// </summary>
    /// <param name="id">The id of wanted user</param>
    /// <returns>A Task with user OR null</returns>
    Task<UserApi?> GetByIdAsync(Guid id);

    /// <summary>
    /// Check if an user exist Async
    /// </summary>
    /// <param name="username">The name of wanted user</param>
    /// <param name="email">The email of wanted user</param>
    /// <returns>A Task with bool</returns>
    Task<bool> ExistAsync(Guid? id, string? username, string? email);

    /// <summary>
    /// Try Create User Async 
    /// </summary>
    /// <param name="user">User to create</param>
    /// <param name="password">password of user</param>
    /// <returns>A Task with identityResult OR null</returns>
    Task<IdentityResult?> CreateAsync(UserApi user, string password);

    /// <summary>
    /// Add a role to the specified user
    /// </summary>
    /// <param name="user">User to add role to</param>
    /// <param name="role">Role for user</param>
    /// <returns>A Task with identityResult OR null</returns>
    Task<IdentityResult?> AddRoleAsync(UserApi user, string role);

    /// <summary>
    /// Check that the password given corresponds to the user's password
    /// </summary>
    /// <param name="user">User where check password</param>
    /// <param name="password">password to check</param>
    /// <returns>A Task with bool</returns>
    Task<bool> CheckPasswordAsync(UserApi user, string password);

    /// <summary>
    /// Generate token for authenticated user
    /// </summary>
    /// <param name="user">User where check password</param>
    /// <returns>A Task with token</returns>
    Task<TokenDto> LoginAsync(UserApi user);

    /// <summary>
    /// Test the validity of provided token
    /// </summary>
    /// <param name="token">Token to check</param>
    /// <returns>Boolean </returns>
    bool TestTokenValidity(string token);

    /// <summary>
    /// Generate Reset password token
    /// </summary>
    /// <param name="user">user where generate token</param>
    /// <returns>Boolean </returns>
    Task<string> GeneratePasswordResetTokenAsync(UserApi user);


    /// <summary>
    /// Reset password of user with ResetToken
    /// </summary>
    /// <param name="user">User to change password</param>
    /// <param name="token">Token to make change</param>
    /// <param name="password">Password to change</param>
    /// <returns>A Task with IdentityResult OR null</returns>
    Task<IdentityResult?> ResetPasswordAsync(UserApi user, string token, string password);

    /// <summary>
    /// Delete User
    /// </summary>
    /// <param name="user">User to delete</param>
    /// <returns>A Task</returns>
    Task<IdentityResult> DeleteAsync(UserApi user);
}
