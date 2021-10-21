using System;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Extensions;
using Delsoft.BwBroadcast.FMTransmitter.RDS.Utils.Options;
using Microsoft.Extensions.Options;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks
{
    public class NowPlayingTrack : INowPlayingTrack
    {
        private int _startIndex;
        private int MaxLength { get; }

        public string Track { get; private set; }
        public string NowPlaying { get; private set; }

        public NowPlayingTrack(int maxLength)
        {
            this.MaxLength = maxLength;
        }

        public NowPlayingTrack(IOptions<TrackSplitterOptions> options)
            : this(options.Value.MaxLength)
        {
        }

        public string StartWith(string trackName)
        {
            if (string.IsNullOrWhiteSpace(trackName))
            {
                throw new ArgumentException("Unexpected null or empty value.", nameof(trackName));
            }
            
            this.Track = trackName;
            this.NowPlaying = trackName.Length > MaxLength 
                ? trackName.ToUpper().CleanAccent()[..MaxLength]
                : trackName.ToUpper().CleanAccent();
            
            this._startIndex = 0;

            return NowPlaying;
        }

        public string Next()
        {
            if (string.IsNullOrWhiteSpace(this.Track))
            {
                throw new TrackNotInitializedException();
            }

            if (this.Track.Length <= MaxLength)
            {
                return this.NowPlaying;
            }

            if (this.Track.Length - _startIndex >= MaxLength)
            {
                this.NowPlaying = this.Track.Substring(_startIndex, MaxLength);
                this._startIndex += MaxLength;
                return this.NowPlaying;
            }
            
            var result = this.Track.Substring(_startIndex, this.Track.Length - _startIndex);
            var offset =  MaxLength - result.Length - 1;

            this.NowPlaying = $" {result} {this.NowPlaying[..offset]}";
            
            _startIndex = offset;
            
            return this.NowPlaying;
        }
    }
}