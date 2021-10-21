using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services
{
    public interface IRDS
    {
        void Watch();
        WaitForChangedResult WaitForChange();
        Task SetNowPlaying(CancellationToken cancellationToken);
        Task<string> GetNowPlaying();

    }
}