using JuanApp.Domain.Models;

namespace JuanApp.Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string? Error)> LoginAsync(string username, string password, bool rememberMe);
        Task LogoutAsync();
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<bool> IsInRoleAsync(AppUser user, string role);
    }
}