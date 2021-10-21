using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Data;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
using Microsoft.Extensions.Logging;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public class Rds : IRds
    {
        private readonly ILogger<Rds> _logger;
        private readonly ITransmitterService _transmitterService;
        private readonly INowPlayingTrack _nowPlayingTrack;
        private readonly INowPlayingFile _nowPlayingFile;

        public Rds(ILogger<Rds> logger, ITransmitterService transmitterService, INowPlayingTrack nowPlayingTrack, INowPlayingFile nowPlayingFile)
        {
            _logger = logger;
            _transmitterService = transmitterService;
            _nowPlayingTrack = nowPlayingTrack;
            _nowPlayingFile = nowPlayingFile;
        }

        public void Watch()
        {
            _nowPlayingFile.Watch();
        }

        public WaitForChangedResult WaitForChange()
        {
            return _nowPlayingFile.WaitForChange();
        }
        
        public async Task SetNowPlaying(CancellationToken cancellationToken)
        {
            await _transmitterService.SetRadioText(
                _nowPlayingTrack.StartWith(
                    await _nowPlayingFile.ReadNowPlayingFile(cancellationToken)));
         
            _logger.LogTrace($"Radio text set with: {_nowPlayingTrack.NowPlaying}");
        }
        
        public async Task<string> GetNowPlaying()
            => (await _transmitterService.GetRadioText().ConfigureAwait(true)).FirstOrDefault();
    }
}