using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Core.IRepositories;
using RoboticsContainer.Infrastructure.Data;
using RoboticsContainer.Infrastructure.Extensions;
using RoboticsContainer.Infrastructure.Mapper;
using RoboticsContainer.Infrastructure.Repositories;
using RoboticsContainer.Infrastructure.Services;
using RoboticsContainer.Services;
using StackExchange.Redis;
using System.Text;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 26)), // Replace with your actual MySQL version
            mysqlOptions => mysqlOptions.EnableRetryOnFailure()));

        // Add custom services
        services.AddSingleton<IPathService, PathService>();
        services.AddSingleton<ICommandService, CommandService>();
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

        // Add JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["AuthConfiguration:Jwt:Issuer"], // Use your issuer from appsettings
                ValidAudience = configuration["AuthConfiguration:Jwt:Audience"], // Use your audience from appsettings
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfiguration:Jwt:SecretKey"])) // Use your secret key from appsettings
            };
        });

        return services;
    }
}
