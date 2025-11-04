
using Microsoft.Extensions.DependencyInjection;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using FluentValidation;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;
using LigaLibre.Application.Mapping;



namespace LigaLibre.Application
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Mapster
            MappingConfig.RegisterMappings();


            //services
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IRefereeServices, RefereeServices>();
            //validators
            services.AddScoped<IValidator<CreatePlayerDto>, CreatePlayerValidator>();
            services.AddScoped<IValidator<CreateClubDto>, CreateClubValidator>();
            services.AddScoped<IValidator<CreateMatchDto>, CreateMatchValidator>();
            services.AddScoped<IValidator<CreateRefereeDto>, CreateRefereeValidator>();
            services.AddScoped<IValidator<UpdatePlayerDto>, UpdatePlayerValidator>();
            services.AddScoped<IValidator<UpdateClubDto>, UpdateClubValidator>();
            services.AddScoped<IValidator<UpdateMatchDto>, UpdateMatchValidator>();
            services.AddScoped<IValidator<UpdateRefereeDto>, UpdateRefereeValidator>();
            return services;
        }
    }
}
