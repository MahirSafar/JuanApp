using JuanApp.Application.Services.Interfaces;
using JuanApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace JuanApp.Infrastructure.Services
{
    public class AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : IAccountService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<(bool Success, string? Error)> LoginAdminAsync(string username, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return (false, "Invalid username or password");

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
                return (false, "Access denied. Admin rights required.");

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            if (!result.Succeeded)
                return (false, "Invalid username or password");

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> LoginUserAsync(string usernameOfEmail, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(usernameOfEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(usernameOfEmail);
                if (user == null)
                    return (false, "Invalid username or password");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return (false, "Access denied. Member rights required.");

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, true);

            if (result.IsLockedOut)
                return (false, "Your account is blocked.");

            if (!user.EmailConfirmed)
            {
                await _signInManager.SignOutAsync();
                return (false, "Please confirm your email address.");
            }

            if (!result.Succeeded)
                return (false, "Invalid username or password");

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string fullname, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
                return (false, "This username is already taken.");

            user = new AppUser
            {
                UserName = username,
                Email = email,
                FullName = fullname,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return (true, null);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> IsInRoleAsync(AppUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<(bool Success, string? Error, string? EmailConfirmationToken)> GenerateEmailConfirmationTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return (false, "User not found.", null);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return (true, null, token);
        }

        public async Task<(bool Success, string? Error)> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return (true, null);
            }

            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            return (false, errors);
        }

        public async Task<(bool Success, string? Error, string? PasswordResetToken)> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "There is no user with this email.", null);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return (true, null, token);
        }

        public async Task<(bool Success, string? Error)> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "Invalid Request.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return (true, null);
            }

            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            return (false, errors);
        }
        public async Task<(bool Success, string? Error)> HandleExternalLoginAsync(string email, string fullName, string externalId, bool isPersistent = false)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // User doesn't exist, create a new one.
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    GoogleId = externalId
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return (false, "Failed to create new user account.");
                }

                await _userManager.AddToRoleAsync(user, "Member");
            }
            else
            {
                // User exists. Update the GoogleId if it's missing.
                if (string.IsNullOrEmpty(user.GoogleId))
                {
                    user.GoogleId = externalId;
                    await _userManager.UpdateAsync(user);
                }
            }

            // Sign the user in after they are created or found.
            await _signInManager.SignInAsync(user, isPersistent);

            return (true, null);
        }
    }
}
