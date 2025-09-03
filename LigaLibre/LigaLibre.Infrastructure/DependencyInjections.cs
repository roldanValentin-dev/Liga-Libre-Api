using LigaLibre.Domain.Interfaces;
using LigaLibre.Infrastructure.Data;
using LigaLibre.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LigaLibre.Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Repositories
            services.AddScoped<IClubRepository, ClubRepository>();

            //Services

            return services;
        }
    }
}
