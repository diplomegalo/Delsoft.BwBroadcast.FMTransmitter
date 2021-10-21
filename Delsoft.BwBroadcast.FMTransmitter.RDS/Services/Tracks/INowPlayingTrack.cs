namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks
{
    public interface INowPlayingTrack
    {
        string StartWith(string trackName);
        string Next();
        string Track { get; }
        string NowPlaying { get; }
    }
}