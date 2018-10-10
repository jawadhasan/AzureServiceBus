using System.Threading;
using System.Threading.Tasks;

namespace Receiver.Interfaces
{
    public interface IMassTransitService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
