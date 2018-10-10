using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Receiver.Interfaces;

namespace Receiver.Services
{
    public class AppService : IAppService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IMassTransitService _massTransitService;

        public AppService(ILogger<AppService> logger, IMassTransitService massTransitService)
        {
            _logger = logger;
            _massTransitService = massTransitService;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation($"Starting the Receiver AppService...");
            await _massTransitService.StartAsync(CancellationToken.None);
            _logger.LogInformation($"Service Bus Started at {DateTime.UtcNow}");
            _logger.LogInformation($"RunAsync Completed at {DateTime.UtcNow}");
        }

        public async Task StopAsync()
        {
            await _massTransitService.StopAsync(CancellationToken.None);
            _logger.LogInformation($"Service Bus Stopped at {DateTime.UtcNow}");
        }
    }
}
