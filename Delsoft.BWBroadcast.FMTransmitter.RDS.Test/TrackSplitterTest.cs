using System;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Delsoft.BWBroadcast.FMTransmitter.RDS.Test
{
    public class TrackSplitterTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TrackSplitterTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Can_Next()
        {
            // Arrange
            var target = new TrackSplitter();
            var nowPlaying = "test";
            target.Init(nowPlaying);
            var expected = nowPlaying;

            // Act
            var actual = target.Next();

            // Assert
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void Can_First_Next()
        {
            // Arrange
            var target = new TrackSplitter();
            var nowPlaying = "JUSTIN TIMBERLAKE - CRY ME A RIVER";
            target.Init(nowPlaying);
            var expected = "JUSTIN T";

            // Act
            var actual = target.Next();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Can_Next_Complete_Round_Trip()
        {
            // Arrange
            var target = new TrackSplitter();
            var nowPlaying = "123456789";
            target.Init(nowPlaying);
            _testOutputHelper.WriteLine(target.Next()); // 12345678
            _testOutputHelper.WriteLine(target.Next()); // 9 123456
            _testOutputHelper.WriteLine(target.Next()); // 789 1234
            _testOutputHelper.WriteLine(target.Next()); // 56789 12
            _testOutputHelper.WriteLine(target.Next()); // 3456789_
            var expected = "12345678";

            // Act
            var actual = target.Next();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}