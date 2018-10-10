using System.Threading;
using System.Threading.Tasks;
using ConsoleApp.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Services
{
    public class MassTransitService : IMassTransitService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<MassTransitService> _logger;

        public MassTransitService(IBusControl busControl, ILogger<MassTransitService> logger)
        {
            _busControl = busControl;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting the ServiceBus...");
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stoping the ServiceBus...");
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
