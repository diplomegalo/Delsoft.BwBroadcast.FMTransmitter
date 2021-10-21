using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRds _rds;

        public Worker(ILogger<Worker> logger, IRds rds)
        {
            _logger = logger;
            _rds = rds;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RDS Service running at: {time}", DateTimeOffset.Now);

            _rds.Watch();
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var waitForChange = _rds.WaitForChange();

                    if (waitForChange.ChangeType == WatcherChangeTypes.Changed
                        | waitForChange.ChangeType == WatcherChangeTypes.Created)
                    {
                        _logger.LogTrace($"Worker begin to set now playing");
                        
                        await _rds.SetNowPlaying(cancellationToken).ConfigureAwait(true);
                        
                        _logger.LogTrace($"Worker end set now playing");
                        
                        _logger.LogInformation($"Current now playing : {await _rds.GetNowPlaying().ConfigureAwait(true)}");
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