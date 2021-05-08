using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRdsDomain _rdsDomain;

        public Worker(ILogger<Worker> logger, IRdsDomain rdsDomain)
        {
            _logger = logger;
            _rdsDomain = rdsDomain;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            _rdsDomain.Watch();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var waitForChange = _rdsDomain.WaitForChange();

                    if (waitForChange.ChangeType == WatcherChangeTypes.Changed
                        | waitForChange.ChangeType == WatcherChangeTypes.Created)
                    {
                        var nowPlaying = await _rdsDomain.ReadNowPlayingFile(stoppingToken);
                        _logger.LogInformation($"Worker begin to set now playing: {nowPlaying}");
                        await _rdsDomain.SetNowPlaying(nowPlaying, stoppingToken).ConfigureAwait(true);
                        nowPlaying = await _rdsDomain.GetNowPlaying(stoppingToken).ConfigureAwait(true);
                        _logger.LogInformation($"Current now playing : {nowPlaying}");
                    }
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogError(e.Message);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e.Message);
                }
            }
        }
    }
}