using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            try
            {
                _rdsDomain.Watch();
                while (!stoppingToken.IsCancellationRequested)
                {
                    var waitForChange = _rdsDomain.WaitForChange();

                    if (waitForChange.ChangeType == WatcherChangeTypes.Changed
                        | waitForChange.ChangeType == WatcherChangeTypes.Created)
                    {
                        _rdsDomain.SetNowPlaying(stoppingToken);
                    }
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
