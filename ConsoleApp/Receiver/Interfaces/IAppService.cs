using System.Threading.Tasks;

namespace Receiver.Interfaces
{
    public interface IAppService
    {
        Task RunAsync();
        Task StopAsync();
    }
}
