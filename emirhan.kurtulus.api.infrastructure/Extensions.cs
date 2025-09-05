using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace emirhan.kurtulus.api.infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(
        IServiceCollection services, 
        IConfiguration configuration)
    {
        return services;
    }
}