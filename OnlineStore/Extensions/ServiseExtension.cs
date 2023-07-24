using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStore.Authentications.Facebook;
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
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
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
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            options.User.RequireUniqueEmail = true;
        });
    }

    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
    {
        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    ValidateIssuerSigningKey = true
                };
            });
    }

    public static AuthenticationBuilder AddFacebookAppToken(this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("Authentication:Facebook");

        var appId = section["AppId"];
        var appSecret = section["AppSecret"];

        return builder.AddFacebookAppToken(appId, appSecret);
    }

    public static AuthenticationBuilder AddFacebookAppToken(this AuthenticationBuilder builder, string appId,
        string appSecret)
    {
        return builder.AddFacebook(opt =>
        {
            opt.AppId = appId;
            opt.AppSecret = appSecret;

            opt.AuthorizationEndpoint = FacebookConstants.AuthorizationEndpoint;
            opt.TokenEndpoint = FacebookConstants.TokenEndpoint;
            opt.UserInformationEndpoint = FacebookConstants.UserInformationEndpoint;

            opt.CallbackPath = PathString.FromUriComponent("/api/v1/user/login/extern");
        });
    }

    public static AuthenticationBuilder AddGoogleAppToken(this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("Authentication:Google");

        var appId = section["ClientId"];
        var appSecret = section["ClientSecret"];

        return builder.AddGoogleAppToken(appId, appSecret);
    }

    public static AuthenticationBuilder AddGoogleAppToken(this AuthenticationBuilder builder, string clientId,
        string clientSecret)
    {
        return builder.AddGoogle(opt =>
        {
            opt.ClientId = clientId;
            opt.ClientSecret = clientSecret;

            opt.CallbackPath = PathString.FromUriComponent(OnlineStoreConstants.GoogleCallback);
        });
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

    public static IServiceCollection AddSwaggerGenWithScheme(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineStore", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
}