using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Domain
{
    public interface IRdsDomain
    {
        void Watch();
        WaitForChangedResult WaitForChange();
        Task SetNowPlaying(CancellationToken stoppingToken);
    }
}