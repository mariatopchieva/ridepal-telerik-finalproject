using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RidePal.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HostedService
{
    public class HostedDBSeedService : IHostedService
    {
        private Timer timer;
        private readonly IServiceProvider serviceProvider;

        public HostedDBSeedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //this.timer = new Timer(CallSyncGenresAsync, null, TimeSpan.Zero,
                                    //TimeSpan.FromHours(8));

            return Task.CompletedTask;
        }

        //private async void CallSyncGenresAsync(object state)
        //{
        //    using (var scope = this.serviceProvider.CreateScope())
        //    {
        //        var seedService = scope.ServiceProvider.GetRequiredService<IDatabaseSeedService>();

        //        await seedService.DownloadTrackData("metal");
        //        await seedService.DownloadTrackData("rock");
        //        await seedService.DownloadTrackData("pop");
        //        await seedService.DownloadTrackData("jazz");
        //    }
        //}

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
