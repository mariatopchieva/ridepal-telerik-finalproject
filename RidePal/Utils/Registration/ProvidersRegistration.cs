using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RidePal.Service;
using RidePal.Service.Contracts;
using RidePal.Service.Providers;
using RidePal.Service.Providers.Contracts;
using System;

namespace RidePal.Utils.Registration
{
    public static class ProvidersRegistration
    {
        public static IServiceCollection RegisterProviders(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseSeedService, DatabaseSeedService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IFileCheckProvider, FileCheckProvider>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RidePal API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

            return services;
        }
    }
}
