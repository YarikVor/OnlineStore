using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStore.Contexts;
using OnlineStore.Entities;
using OnlineStore.Mappers;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace OnlineStore;

public static class ServiseExtension
{
    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("_MyPolicy",
                policy =>
                {
                    policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration)
    {
        return AddContexts(services, configuration.GetRequiredDefaultConnectionString());
    }

    public static IServiceCollection AddContexts(this IServiceCollection services, string connectionString)
    {
        return services
            .AddDbContext<ApplicationContext>(Connect)
            .AddDbContext<OnlineStoreContext>(Connect);

        void Connect(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(connectionString);
        }
    }

    public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();


        return services;
    }

    public static IServiceCollection AddConfigurationIdentity(this IServiceCollection services)
    {
        return services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            options.User.RequireUniqueEmail = true;
        });
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.Client,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
 
    public static IServiceCollection AddTokenGenerator(this IServiceCollection services)
    {
        return services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
    }
    
    public static IServiceCollection AddOnlineStoreAutoMapper(this IServiceCollection services)
    {
        return services
            .AddScoped<IConfigurationProvider, OnlineShopMapperConfiguration>()
            .AddScoped<IMapper>(s => s.GetRequiredService<IConfigurationProvider>().CreateMapper());
    }
}