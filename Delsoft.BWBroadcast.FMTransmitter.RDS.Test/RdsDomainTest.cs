using System.Threading;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace Delsoft.BWBroadcast.FMTransmitter.RDS.Test
{
    public class RdsDomainTest
    {
        [Fact]
        public void Can_SetNowPlaying()
        {
            // Arrange
            var actual = string.Empty;
            
            const string track = "EEAU";
            
            var logger = Mock.Of<ILogger<BwBroadcast.FMTransmitter.RDS.Services.RDS>>();
            var options = Mock.Of<IOptions<NowPlayingOptions>>();
            var trackSplitter = Mock.Of<INowPlayingTrack>();
            var transmitterService = Mock.Of<ITransmitterService>();
            
            var target = new BwBroadcast.FMTransmitter.RDS.Services.RDS(logger, transmitterService, options, trackSplitter);
            
            // Act
            target.SetNowPlaying(string.Empty, CancellationToken.None);
            
            // Assert
            Mock.Get(transmitterService).Verify(v => v.SetRadioText(track), Times.Once);
        }
    }
}