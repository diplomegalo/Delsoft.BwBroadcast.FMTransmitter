using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Domain
{
    public interface IRdsDomain
    {
        void Watch();
        WaitForChangedResult WaitForChange();
        Task SetNowPlaying(string nowPlaying, CancellationToken stoppingToken);
        Task<string> GetNowPlaying(CancellationToken stoppingToken);
        Task<string> ReadNowPlayingFile(CancellationToken cancellationToken);
    }
}