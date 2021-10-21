using System.Net.Http;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
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

                    services.AddTransient<IRDS, Services.RDS>();
                    services.AddTransient<TransmitterService>();
                    services.AddTransient<INowPlayingTrack, NowPlayingTrack>();

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
