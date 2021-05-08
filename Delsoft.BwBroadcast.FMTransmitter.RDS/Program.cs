using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Domain;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHttpClient();

                    services.AddTransient<IRdsDomain, RdsDomain>();
                    services.AddTransient<TransmitterService>();

                    services.Configure<NowPlayingOptions>(hostContext.Configuration.GetSection("NowPlaying"));
                    services.Configure<TransmitterOptions>(hostContext.Configuration.GetSection("Transmitter"));

                    services.AddTransient<ITransmitterService>(provider =>
                        new TransmitterAuthenticatedServiceDecorator(
                            provider.GetRequiredService<ILogger<TransmitterAuthenticatedServiceDecorator>>(),
                            provider.GetRequiredService<IHttpClientFactory>(),
                            provider.GetRequiredService<TransmitterService>(),
                            provider.GetRequiredService<IOptions<TransmitterOptions>>()));
                });
    }
}
