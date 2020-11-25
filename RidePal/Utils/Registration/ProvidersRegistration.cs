using Microsoft.Extensions.DependencyInjection;
using RidePal.Service;
using RidePal.Service.Contracts;
using RidePal.Service.Providers;
using RidePal.Service.Providers.Contracts;

namespace RidePal.Utils.Registration
{
    public static class ProvidersRegistration
    {
        public static IServiceCollection RegisterProviders(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseSeedService, DatabaseSeedService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IFileCheckProvider, FileCheckProvider>();

            return services;
        }
    }
}
