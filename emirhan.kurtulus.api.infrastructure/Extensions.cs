using emirhan.kurtulus.api.application;
using emirhan.kurtulus.api.application.Services;
using emirhan.kurtulus.api.core.Repositories;
using emirhan.kurtulus.api.infrastructure.Mongo;
using emirhan.kurtulus.api.infrastructure.Options;
using emirhan.kurtulus.api.infrastructure.Repositories;
using emirhan.kurtulus.api.infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;

namespace emirhan.kurtulus.api.infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoDbOptions>()
                .Bind(configuration.GetSection(MongoDbOptions.DefaulSectionName))
                .ValidateDataAnnotations()
                .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString))
                .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName))
                .ValidateOnStart();

        services.AddSingleton<IMongoClient>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            var settings = MongoClientSettings.FromConnectionString(opts.ConnectionString);
            return new MongoClient(settings);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(opts.DatabaseName);
        });

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<MongoInitializer>();
        services.AddSingleton<IPasswordService, PasswordService>();

        return services;
    }
}