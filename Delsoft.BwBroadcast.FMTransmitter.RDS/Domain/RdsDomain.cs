using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static System.Threading.Tasks.Task;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Domain
{
    public class RdsDomain : IRdsDomain
    {
        private readonly ILogger<RdsDomain> _logger;
        private readonly TransmitterService _transmitterService;
        private readonly IOptions<NowPlayingOptions> _options;
        private FileSystemWatcher _watcher;

        public RdsDomain(ILogger<RdsDomain> logger, TransmitterService transmitterService, IOptions<NowPlayingOptions> options)
        {
            _logger = logger;
            _transmitterService = transmitterService;
            _options = options;
        }

        public string FullPath => $"{_options.Value.FilePath}\\{_options.Value.FileName}";


        public void Watch()
        {
            _logger.LogTrace($"Worker begins to watch on {this.FullPath}");
            _watcher = new FileSystemWatcher(_options.Value.FilePath) { Filter = _options.Value.FileName };
        }

        public WaitForChangedResult WaitForChange()
        {
            _logger.LogTrace($"Worker waits for file name {_options.Value.FileName} change.");
            return _watcher.WaitForChanged(WatcherChangeTypes.Changed);
        }

        public async Task SetNowPlaying(CancellationToken stoppingToken)
        {
            var nowPlaying = await this.ReadNowPlayingFile(stoppingToken) ?? throw new InvalidOperationException("Unexpected null value in the now playing file.");
            nowPlaying = nowPlaying.ToUpper();

            _logger.LogInformation($"Worker is changing radio text now playing by {nowPlaying}");

            _transmitterService.SetRadioText(nowPlaying);
        }

        private async Task<string> ReadNowPlayingFile(CancellationToken cancellationToken)
        {
            var retry = 3;
            while (retry > 0)
            {
                try
                {
                    return await File.ReadAllTextAsync(this.FullPath, cancellationToken);
                }
                catch (Exception)
                {
                    await Delay(1000, cancellationToken);
                    retry--;
                }
            }

            throw new InvalidOperationException("Unable to read radio text.");
        }
    }
}