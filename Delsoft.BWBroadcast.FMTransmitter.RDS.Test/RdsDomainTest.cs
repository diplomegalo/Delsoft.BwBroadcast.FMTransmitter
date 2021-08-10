using System.Threading;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Domain;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services;
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
            
            const string targetParam = "éèàù";
            const string expected = "EEAU";
            
            var logger = Mock.Of<ILogger<RdsDomain>>();
            var options = Mock.Of<IOptions<NowPlayingOptions>>();
            
            var transmitterService = new Mock<ITransmitterService>();
            transmitterService
                .Setup(s => s.SetRadioText(It.IsAny<string>()))
                .Callback<string>(param => actual = param);
            
            var target = new RdsDomain(logger, transmitterService.Object, options);
            
            // Act
            target.SetNowPlaying(targetParam, CancellationToken.None);
            
            // Assert
            actual.ShouldBe(expected);
        }
    }
}