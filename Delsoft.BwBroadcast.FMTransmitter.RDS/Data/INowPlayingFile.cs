using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Data
{
    public interface INowPlayingFile
    {
        void Watch();
        WaitForChangedResult WaitForChange();
        Task<string> ReadNowPlayingFile(CancellationToken cancellationToken);
    }
}