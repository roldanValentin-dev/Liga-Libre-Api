
using LigaLibre.API.Middlewares;
using LigaLibre.Application;
using LigaLibre.Infrastructure;
using LigaLibre.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace LigaLibre
{
    public partial class Program
    {
        public static void Main(string[] args)
        {

            //configuracion de logging con Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // configuramos Serilog como el proveedor de logging
            builder.Host.UseSerilog();

            // agregamos servicios a travez de inyeccion de dependencias de infraestructura y aplicacion
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // configuramos swagger para que acepte el token JWT
            builder.Services.AddSwaggerGen(options =>
            {
                // configuramos la seguridad para usar JWT en Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                // agregamos el requisito de seguridad para que todas las operaciones requieran el token
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // configuramos la autenticacion con JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                // configuramos la validacion del token
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAndClient", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });

            // Configuramos CORS para permitir solicitudes desde el frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("*") // Reemplaza con la URL de tu frontend
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            //Diagnostica la base de datos
            builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // a�adimos el middleware de logging de errores van en cascada
            app.UseMiddleware<ErrorLoggingMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
