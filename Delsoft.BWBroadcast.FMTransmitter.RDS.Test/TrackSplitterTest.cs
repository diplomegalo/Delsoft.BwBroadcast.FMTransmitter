using System;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Domain;
using Xunit;

namespace Delsoft.BWBroadcast.FMTransmitter.RDS.Test
{
    public class TrackSplitterTest
    {
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
            Console.WriteLine(target.Next()); // 12345678
            Console.WriteLine(target.Next()); // 9 123456
            Console.WriteLine(target.Next()); // 789 1234
            Console.WriteLine(target.Next()); // 56789 12
            Console.WriteLine(target.Next()); // 3456789_
            var expected = "12345678";

            // Act
            var actual = target.Next(); // 3456789

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}