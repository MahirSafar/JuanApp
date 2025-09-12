using JuanApp.Persistance.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JuanApp.Persistance;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JuanAppContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}
