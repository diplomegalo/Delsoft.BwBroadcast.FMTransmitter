using System.Threading;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Data;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Delsoft.BWBroadcast.FMTransmitter.RDS.Test
{
    public class RdsDomainTest
    {
        [Fact]
        public void Can_SetNowPlaying()
        {
            // Arrange
            const string track = "éèà123456789";
            
            var logger = Mock.Of<ILogger<BwBroadcast.FMTransmitter.RDS.Services.RDS>>();
            var nowPlayingFile = Mock.Of<INowPlayingFile>();
            var nowPlayingTrack = Mock.Of<INowPlayingTrack>();
            var transmitterService = Mock.Of<ITransmitterService>();
            
            var target = new BwBroadcast.FMTransmitter.RDS.Services.RDS(logger, transmitterService, nowPlayingTrack, nowPlayingFile);
            
            // Act
            target.SetNowPlaying(CancellationToken.None);
            
            // Assert
            Mock.Get(transmitterService).Verify(v => v.SetRadioText(track), Times.Once);
        }
    }
}