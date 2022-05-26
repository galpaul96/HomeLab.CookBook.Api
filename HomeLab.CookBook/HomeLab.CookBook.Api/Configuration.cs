using System.Reflection;
using AutoMapper;
using HomeLab.CookBook.API.Infra;
using HomeLab.CookBook.Domain.Settings;
using HomeLab.CookBook.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HomeLab.CookBook.Api
{
    public static class Configuration
    {
        public static void ConfigureGeneralServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureAppSettings(configuration);

            //services.ConfigureIdentity(configuration);
            services.ConfigureSwagger(configuration);
            services.ConfigureAutoMapper(configuration);

            services.ConfigureServices(configuration);
        }

        internal static void ConfigureAutoMapper(this IServiceCollection services,
            IConfiguration configuration)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApiMappingProfile());
                mc.AddProfile(new ServiceMappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        internal static void ConfigureAppSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<IdentitySettings>(configuration.GetSection(nameof(IdentitySettings)));
            services.Configure<Configs>(configuration.GetSection(nameof(Configs)));
        }

        internal static void ConfigureSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeLab.CookBook", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        internal static void ConfigureIdentity(this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = new IdentitySettings();
            configuration.Bind(nameof(IdentitySettings), settings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = settings.Authority;
                    options.TokenValidationParameters.ValidateAudience = false;
                });
            services.AddAuthorization(options =>
                options.AddPolicy("cookbook-api", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "cookbook-api");
                })
            );
        }
    }
}
