using System.ComponentModel;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Domain
{
    public class TrackSplitter
    {
        private const int MaxLength = 8;

        private string _nowPlaying;
        private int startIndex;

        public void Init(string nowPlaying)
        {
            this._nowPlaying = nowPlaying;
            startIndex = 0;
        }

        public string Next()
        {
            if (this._nowPlaying.Length <= MaxLength)
            {
                return this._nowPlaying;
            }

            if (this._nowPlaying.Length - startIndex >= MaxLength)
            {
                var result = _nowPlaying.Substring(startIndex, MaxLength);
                this.startIndex += MaxLength;
                return result;
            }
            else
            {
                var result = _nowPlaying.Substring(startIndex, _nowPlaying.Length - startIndex);
                var offset =  MaxLength - result.Length - 1;

                result += $" {_nowPlaying.Substring(0, (offset))}";
                startIndex = offset;
                return result;
            }
        }
    }
}