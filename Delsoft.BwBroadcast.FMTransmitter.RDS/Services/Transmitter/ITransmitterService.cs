using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Services.Transmitter
{
    public interface ITransmitterService
    {
        Task SetRadioText(string nowPlaying);
        Task<IEnumerable<string>> GetRadioText();
    }
}