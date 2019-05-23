using AutoMapper;
using GildedRose.Api.Profiles;
using GildedRose.BusinessLogic;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.BusinessLogic.Services;
using GildedRose.DataAccess.DatabaseContext;
using GildedRose.DataAccess.Interfaces.Repositories;
using GildedRose.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GildedRose.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void ConfigureDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GildedRoseContext>(options => options.UseInMemoryDatabase());
        }

        internal static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAddFakeDataService, AddFakeDataService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IItemService, ItemService>();
        }

        internal static void ConfigureAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<ItemProfile>();
            });
            Mapper.AssertConfigurationIsValid();
        }

        internal static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAllOrigin", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
        }

        internal static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = TokenConfig.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = TokenConfig.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = TokenConfig.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,
                    };
                });
        }
    }
}
