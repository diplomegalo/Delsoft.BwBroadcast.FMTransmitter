using System;
using System.IO;
using System.Linq;
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
        private readonly ITransmitterService _transmitterService;
        private readonly IOptions<NowPlayingOptions> _options;
        private FileSystemWatcher _watcher;

        public RdsDomain(ILogger<RdsDomain> logger, ITransmitterService transmitterService, IOptions<NowPlayingOptions> options)
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

        public async Task SetNowPlaying(string nowPlaying, CancellationToken stoppingToken)
        {
            if (string.IsNullOrWhiteSpace(nowPlaying))
            {
                throw new ArgumentNullException(nameof(nowPlaying));
            }
            
            nowPlaying = nowPlaying
                .CleanAccent()
                .ToUpper()[..32];
            
            await _transmitterService.SetRadioText(nowPlaying).ConfigureAwait(true);

            _logger.LogTrace($"Set now playing: {nowPlaying}");
        }

        public async Task<string> GetNowPlaying(CancellationToken stoppingToken)
            => (await _transmitterService.GetRadioText().ConfigureAwait(true)).FirstOrDefault();

        public async Task<string> ReadNowPlayingFile(CancellationToken cancellationToken)
        {
            var retry = 3;
            while (retry > 0)
            {
                try
                {
                    return await File.ReadAllTextAsync(this.FullPath, cancellationToken)
                        .ConfigureAwait(true);
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