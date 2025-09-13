using JuanApp.Domain.Models;
using JuanApp.Persistance.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;

namespace JuanApp.MVC
{
    public static class ServiceRegistration
    {
        public static void AddMVCServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            // This is the correct way to register both Identity and Google
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });

            // AddIdentity no longer needs to be chained, as authentication is already set up
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireDigit = true;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Password.RequiredLength = 6;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                opt.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<JuanAppContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = opt.Events.OnRedirectToAccessDenied = context =>
                {
                    var uri = new Uri(context.RedirectUri);
                    if (context.Request.Path.Value.ToLower().StartsWith("/manage"))
                    {
                        context.Response.Redirect("/manage/account/login" + uri.Query);
                    }
                    else
                    {
                        context.Response.Redirect("/account/login" + uri.Query);
                    }
                    return Task.CompletedTask;
                };
            });
        }
    }
}