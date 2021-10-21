using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public interface IRds
    {
        void Watch();
        WaitForChangedResult WaitForChange();
        Task SetNowPlaying(CancellationToken cancellationToken);
        Task<string> GetNowPlaying();

    }
}