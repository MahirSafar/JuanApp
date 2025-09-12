using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JuanApp.Infrastructure.Services
{
    public class AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IAccountService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<(bool Success, string? Error)> LoginAsync(string username, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return (false, "Invalid username or password");

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            if (!result.Succeeded)
                return (false, "Invalid username or password");

            if (await _userManager.IsInRoleAsync(user, "Member"))
                return (false, "Access denied. Admin rights required.");

            return (true, null);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> IsInRoleAsync(AppUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}