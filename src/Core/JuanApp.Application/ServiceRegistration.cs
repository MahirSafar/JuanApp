using JuanApp.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace JuanApp.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(opt => { }, typeof(MapperProfile).Assembly);
    }
}
