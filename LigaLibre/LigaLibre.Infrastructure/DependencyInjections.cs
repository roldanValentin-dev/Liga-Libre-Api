using Amazon.SQS;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using LigaLibre.Infrastructure.Data;
using LigaLibre.Infrastructure.Repositories;
using LigaLibre.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LigaLibre.Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            //cadena de conexion a la base de datos
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //redis cache
            var redisConection = configuration.GetConnectionString("redis") ?? "localhost:6379";

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConection;
                options.InstanceName = "LigaLibreApi";
            });

            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Repositorios
            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IRefereeRepository, RefereeRepository>();

            //AWS services
            services.AddSingleton<IAmazonSQS>(provider =>
            {
                var config = new AmazonSQSConfig
                {
                    ServiceURL = "http://localhost:4566", // LocalStack URL
                    UseHttp = true
                };
                return new AmazonSQSClient("test", "test", config);
            });

            //servicios
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISqsService, SqsService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            return services;
        }
    }
}
