using Microsoft.EntityFrameworkCore;
using EduViz.Entities;
using EduViz;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using EduViz.Controllers;
using EduViz.Data;
using EduViz.Dtos;
using EduViz.Mappers;
using EduViz.Middlewares;
using EduViz.Repositories;
using EduViz.Services;
using EduViz.Settings;

namespace EduViz.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped<ExceptionMiddleware>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationMapper());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<DatabaseInitialiser>();
            services.AddScoped<CloudinaryService>();
            services.AddScoped<IdentityService>();
            services.AddScoped<UserService>();
            services.AddScoped<PayOsPaymentService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<MentorDetailService>();
            services.AddScoped<UpgradeOrderDetailService>();
            services.AddScoped<SubjectService>();
            return services;
        }
    }
}
