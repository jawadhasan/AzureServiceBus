using System.Threading.Tasks;
using Messages;

namespace ConsoleApp.Interfaces
{
    public interface IMessageSender
    {
        Task SendAsync(ISampleMessage sampleMessage, string queueAddress);
    }
}