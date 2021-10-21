using System;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Delsoft.BWBroadcast.FMTransmitter.RDS.Test
{
    public class NowPlayingTrackTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NowPlayingTrackTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Cannot_Start_With_Empty_Track()
        {
            // Arrange
            var target = new NowPlayingTrack(8);
            
            // Act

            // Assert
            Should.Throw<ArgumentException>(() => target.StartWith(string.Empty));
        }
        
        [Fact]
        public void Cannot_Start_Null_Track()
        {
            // Arrange
            var target = new NowPlayingTrack(8);
            
            // Act

            // Assert
            Should.Throw<ArgumentException>(() => target.StartWith(null));
        }
        
        [Fact]
        public void Can_Get_NowPlaying_Without_Accent()
        {
            // Arrange
            const string track = "éèàù";
            const string expected = "EEAU";
            var target = new NowPlayingTrack(8);
            target.StartWith(track);
            
            // Act
            var actual = target.NowPlaying;
            
            // Assert
            actual.ShouldBe(expected);
        }
        
        [Fact]
        public void Can_Get_NowPlaying_Upper_Case()
        {
            // Arrange
            const string track = "éèàù";
            const string expected = "EEAU";
            var target = new NowPlayingTrack(8);
            target.StartWith(track);
            
            // Act
            var actual = target.NowPlaying;
            
            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Can_Get_Next_NowPlaying()
        {
            // Arrange
            const string expected = "TEST";
            const string trackName = "test";
            var target = new NowPlayingTrack(8);
            target.StartWith(trackName);
            
            // Act
            var actual = target.Next();
            var result = target.NowPlaying;

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Can_Get_Next_Substring_NowPlaying()
        {
            // Arrange
            var target = new NowPlayingTrack(8);
            const string trackName = "JUSTIN TIMBERLAKE - CRY ME A RIVER";
            target.StartWith(trackName);
            const string expected = "JUSTIN T";
                        
            // Act
            var result = target.Next();
            var actual = target.NowPlaying;

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Can_Get_Next_Several_Substring_NowPlaying()
        {
            // Arrange
            var target = new NowPlayingTrack(8);
            const string trackName = "123456789";
            target.StartWith(trackName);
            _testOutputHelper.WriteLine(target.Next()); // 12345678
            _testOutputHelper.WriteLine(target.Next()); // 9 123456
            _testOutputHelper.WriteLine(target.Next()); // 789 1234
            _testOutputHelper.WriteLine(target.Next()); // 56789 12
            _testOutputHelper.WriteLine(target.Next()); // 3456789
            _testOutputHelper.WriteLine(target.Next()); //  
            const string expected = "12345678";

            // Act
            var actual = target.NowPlaying;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Can_Is_Too_Large()
        {
            var target = new NowPlayingTrack(8);
            target.StartWith("123465789");
            
            // Act
            var actual = target.IsTooLarge();
            
            // Assert
            Assert.True(actual);
        }
    }
}