using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Core.IRepositories;
using RoboticsContainer.Infrastructure.Data;
using RoboticsContainer.Infrastructure.Extensions;
using RoboticsContainer.Infrastructure.Mapper;
using RoboticsContainer.Infrastructure.Repositories;
using RoboticsContainer.Infrastructure.Services;
using RoboticsContainer.Services;
using StackExchange.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                 sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

        // Add custom services
        services.AddSingleton<IPathService, PathService>();
        services.AddSingleton<ICommandService,CommandService>();
        services.AddScoped<NtpTimeService>();
        services.AddScoped<IContainerEntryRepository, ContainerEntryRepository>();
        services.AddScoped<IContainerEntryService, ContainerEntryService>();
        services.AddSingleton<IDockerService, DockerService>();
        services.AddAutoMapper(typeof(MappingProfile));
        

        // Redis connection
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect("localhost:6379"));
        services.AddScoped<ICacheService, DistributedCacheService>();

        // Add additional services
        services.AddTransient<ISqlContainerTimeService, SqlContainerTimeService>();

        return services;
    }
}
