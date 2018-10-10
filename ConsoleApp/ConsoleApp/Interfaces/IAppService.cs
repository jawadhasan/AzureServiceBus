using System.Threading.Tasks;

namespace ConsoleApp.Interfaces
{
    public interface IAppService
    {
        Task RunAsync();
        Task StopAsync();
    }
}