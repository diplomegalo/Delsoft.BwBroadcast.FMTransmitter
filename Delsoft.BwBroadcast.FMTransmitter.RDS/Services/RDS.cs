using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public class RDS : IRDS
    {
        private readonly ILogger<RDS> _logger;
        private readonly ITransmitterService _transmitterService;
        private readonly IOptions<NowPlayingOptions> _options;
        private readonly INowPlayingTrack _nowPlayingTrack;
        private FileSystemWatcher _watcher;

        public RDS(ILogger<RDS> logger, ITransmitterService transmitterService, IOptions<NowPlayingOptions> options, INowPlayingTrack nowPlayingTrack)
        {
            _logger = logger;
            _transmitterService = transmitterService;
            _options = options;
            _nowPlayingTrack = nowPlayingTrack;
        }

        private string FullPath => $"{_options.Value.FilePath}\\{_options.Value.FileName}";

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
        public async Task SetNowPlaying(CancellationToken cancellationToken)
        {
            _nowPlayingTrack.StartWith(await ReadNowPlayingFile(cancellationToken));
            await _transmitterService.SetRadioText(_nowPlayingTrack.NowPlaying).ConfigureAwait(true);
            _logger.LogTrace($"Radio text set with: {_nowPlayingTrack.NowPlaying}");
        }
        
        public async Task<string> GetNowPlaying()
            => (await _transmitterService.GetRadioText().ConfigureAwait(true)).FirstOrDefault();
        
        private async Task<string> ReadNowPlayingFile(CancellationToken cancellationToken)
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
                    await Task.Delay(1000, cancellationToken);
                    retry--;
                }
            }

            throw new InvalidOperationException("Unable to read radio text.");
        }
    }
}