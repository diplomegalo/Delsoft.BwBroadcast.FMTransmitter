using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Domain;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddTransient<IRdsDomain, RdsDomain>();
                    services.AddTransient<ITransmitterService, TransmitterAuthenticatedServiceDecorator>();
                    services.AddTransient<TransmitterService>();

                    services.AddHttpClient();

                    services.Configure<NowPlayingOptions>(hostContext.Configuration.GetSection("NowPlaying"));
                    services.Configure<TransmitterOptions>(hostContext.Configuration.GetSection("Transmitter"));
                });
    }
}
