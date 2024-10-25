using MyBackendApp.IServices;
using MyBackendApp.Services;

namespace MyBackendApp.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IHomeUserService, HomeUserService>();
        return services;
    }
}
