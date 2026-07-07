using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Infrastructure.Identity.Context;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Options;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Repositories;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Services;
using GoogleOptions = Microsoft.AspNetCore.Authentication.Google.GoogleOptions;

namespace ShoppingSystem.Microservice.Infrastructure.Identity;

/// <summary>
/// ToDo: change it microservice platform then add cross database feature for it  
/// </summary>
public static class ConfigureService
{
    public static IServiceCollection RegisterIdentityDependencies(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        var connectionString = configuration.GetConnectionString(
            "ShoppingSystem_Microservice_Identity");

        services.AddDbContext<IdentityAppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.Configure<JwtOptions>(
            configuration.GetSection("Jwt"));

        services.Configure<GoogleOptions>(
            configuration.GetSection("Google"));


        services.AddIdentity<ApplicationUser
                , IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityAppDbContext>()
            .AddDefaultTokenProviders();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                GoogleDefaults.AuthenticationScheme;
        })

        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    configuration["Jwt:Issuer"],

                ValidAudience =
                    configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:Key"]!))
            };
        })


        .AddGoogle(options =>
        {
            options.ClientId =
                configuration["Google:ClientId"]!;

            options.ClientSecret =
                configuration["Google:ClientSecret"]!;

            options.SaveTokens = true;
        });


        services.AddAuthorization();


        services.AddScoped<IAuthService
            , AuthServiceRepository>();
        
        services.AddScoped<ITokenService
            , TokenRepository>();
        
        services.AddScoped<IUserQueryService
            , AuthRepository>();

        return services;
    }
}