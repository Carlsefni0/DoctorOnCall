using DoctorOnCall.AutoMappers;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories;
using DoctorOnCall.Repositories.Implementations;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.Repository.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Services;
using DoctorOnCall.Services.Implementations;
using DoctorOnCall.Services.Interfaces;
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
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PatientMappingProfile>();
            cfg.AddProfile<DoctorMappingProfile>();
            cfg.AddProfile<MedicineMappingProfile>();
            cfg.AddProfile<VisitRequestMappingProfile>();
            cfg.AddProfile<VisitMappingProfile>();
            cfg.AddProfile<ScheduleMappingProfile>();
            cfg.AddProfile<ScheduleExceptionMappingProfile>();
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "DoctorOnCall API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below.\nExample: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
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
                    Array.Empty<string>()
                }
            });
        });
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidateModelStateFilter>();
        });
        


        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        // Репозиторії
        services.AddScoped<IVisitRequestRepository, VisitRequestRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IScheduleDayRepository, ScheduleDayRepository>();
        services.AddScoped<IScheduleExceptionRepository, ScheduleExceptionRepository>();
        services.AddScoped<HttpClient>();

        // Сервіси
        services.AddScoped<IVisitRequestService, VisitRequestService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IMedicineService, MedicineService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IScheduleExceptionService, ScheduleExceptionService>();
        services.AddScoped<IGoogleMapsService, GoogleMapsService>();

        services.AddScoped<ValidateModelStateFilter>();

      
        services.AddSingleton<IEmailService, EmailService>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        return services;
    }

}