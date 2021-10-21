using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Data;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public class RDS : IRDS
    {
        private readonly ILogger<RDS> _logger;
        private readonly ITransmitterService _transmitterService;
        private readonly INowPlayingTrack _nowPlayingTrack;
        private readonly INowPlayingFile _nowPlayingFile;

        public RDS(ILogger<RDS> logger, ITransmitterService transmitterService, INowPlayingTrack nowPlayingTrack, INowPlayingFile nowPlayingFile)
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
            var nowPlaying = _nowPlayingTrack.StartWith(await _nowPlayingFile.ReadNowPlayingFile(cancellationToken));
            await _transmitterService.SetRadioText(_nowPlayingTrack.NowPlaying).ConfigureAwait(true);
            _logger.LogTrace($"Radio text set with: {_nowPlayingTrack.NowPlaying}");
        }
        
        public async Task<string> GetNowPlaying()
            => (await _transmitterService.GetRadioText().ConfigureAwait(true)).FirstOrDefault();
    }
}