using System;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Tracks
{
    public class TrackNotInitializedException : ApplicationException
    {
        private const string message = "Track is not initialized. Please first initiliaze the track value.";
        
        public TrackNotInitializedException() : base(message)
        {
        }
    }
}