    using JuanApp.Domain.Models;
namespace JuanApp.Application.Services.Interfaces;

public interface IAccountService
{
    Task<(bool Success, string? Error)> LoginAdminAsync(string username, string password, bool rememberMe);
    Task<(bool Success, string? Error)> LogoutAsync(); 
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task<bool> IsInRoleAsync(AppUser user, string role);
    Task<(bool Success, string? Error)> LoginUserAsync(string usernameOfEmail, string password, bool rememberMe);
    Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string fullname, string password);
    Task<(bool Success, string? Error, string? EmailConfirmationToken)> GenerateEmailConfirmationTokenAsync(string username);
    Task<(bool Success, string? Error)> ConfirmEmailAsync(string email, string token);
    Task<(bool Success, string? Error, string? PasswordResetToken)> GeneratePasswordResetTokenAsync(string email);
    Task<(bool Success, string? Error)> ResetPasswordAsync(string email, string token, string newPassword);
    Task<(bool Success, string? Error)> HandleExternalLoginAsync(string email, string fullName, string externalId, bool isPersistent = false);
}
