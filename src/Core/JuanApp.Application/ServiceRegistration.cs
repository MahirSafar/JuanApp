using JuanApp.Application.Profiles;
using JuanApp.Application.Services.Concretes;
using JuanApp.Application.Services.Interfaces;
using JuanApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JuanApp.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(opt => { }, typeof(MapperProfile).Assembly);
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ISliderService, SliderService>();
        services.AddScoped<IEmailService, EmailService>();
    }
}
