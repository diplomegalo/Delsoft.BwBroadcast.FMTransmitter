using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Data
{
    public class NowPlayingFile : INowPlayingFile
    {
        private readonly ILogger<NowPlayingFile> _logger;
        private readonly IOptions<NowPlayingFileOptions> _options;
        private FileSystemWatcher _watcher;

        private string NowPlayingFileFullPath => $"{_options.Value.FilePath}\\{_options.Value.FileName}";
        
        public NowPlayingFile(ILogger<NowPlayingFile> logger, IOptions<NowPlayingFileOptions> options)
        {
            _logger = logger;
            _options = options;
        }
        
        public void Watch()
        {
            _logger.LogTrace($"Worker begins to watch on {this.NowPlayingFileFullPath}");
            
            _watcher = new FileSystemWatcher(_options.Value.FilePath) { Filter = _options.Value.FileName };
        }
        
        public WaitForChangedResult WaitForChange()
        {
            _logger.LogTrace($"Worker waits for file name {_options.Value.FileName} change.");
            
            return _watcher.WaitForChanged(WatcherChangeTypes.Changed);
        }
        
        public async Task<string> ReadNowPlayingFile(CancellationToken cancellationToken)
        {
            var retry = 3;
            while (retry > 0)
            {
                try
                {
                    return await File.ReadAllTextAsync(this.NowPlayingFileFullPath, cancellationToken)
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