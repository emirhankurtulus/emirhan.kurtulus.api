using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace emirhan.kurtulus.api.application;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}