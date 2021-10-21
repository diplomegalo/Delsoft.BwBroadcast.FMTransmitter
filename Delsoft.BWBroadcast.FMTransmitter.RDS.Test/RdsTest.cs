using System.Threading;
using System.Threading.Tasks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Data;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Delsoft.BWBroadcast.FMTransmitter.RDS.Test
{
    public class RdsTest
    {
        [Fact]
        public void Can_SetNowPlaying()
        {
            // Arrange
            const string track = "éèà123456789";
            const string nowPlaying = "EEA123456789";

            var logger = Mock.Of<ILogger<Rds>>();

            var nowPlayingFile = new Mock<INowPlayingFile>();
            nowPlayingFile
                .Setup(s => s.ReadNowPlayingFile(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(track));

            var nowPlayingTrack = new Mock<INowPlayingTrack>();
            nowPlayingTrack
                .Setup(s => s.StartWith(track))
                .Returns(nowPlaying);

            var transmitterService = Mock.Of<ITransmitterService>();
            var target = new Rds(logger, transmitterService, nowPlayingTrack.Object, nowPlayingFile.Object);

            // Act
            target.SetNowPlaying(CancellationToken.None);

            // Assert
            Mock.Get(transmitterService).Verify(v => v.SetRadioText(nowPlaying), Times.Once);
        }
    }
}