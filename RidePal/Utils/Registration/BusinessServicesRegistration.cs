using HostedService;
using Microsoft.Extensions.DependencyInjection;
using RidePal.Service;
using RidePal.Service.Contracts;

namespace RidePal.Utils.Registration
{
    public static class BusinessServicesRegistration
    {
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IGeneratePlaylistService, GeneratePlaylistService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IPixaBayImageService, PixabayImageService>();
            services.AddHostedService<HostedDBSeedService>();

            return services;
        }
    }
}
