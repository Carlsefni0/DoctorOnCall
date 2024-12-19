using DoctorOnCall.Models;
using DoctorOnCall.Repositories;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

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
                    new string[] { }
                }
            });
        });


        // DbContext
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Identity
        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        // Репозиторії
        services.AddScoped<IVisitRequestRepository, VisitRequestRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IVisitRequestMedicineRepository, VisitRequestMedicineRepository>();

        // Сервіси
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IMedicineService, MedicineService>();
        services.AddScoped<IAuthService, AuthService>();

        // EmailService як Singleton (якщо він не залежить від DbContext)
        services.AddSingleton<IEmailService, EmailService>();

        // TokenService як Singleton, якщо не залежить від DbContext
        services.AddScoped<ITokenService, TokenService>();

        // ActionContextAccessor
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        // АвтоMapper
        services.AddAutoMapper(typeof(AutoMapperProfiles));

        return services;
    }

}