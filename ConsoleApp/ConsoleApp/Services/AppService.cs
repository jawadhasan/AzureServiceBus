using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp.Interfaces;
using ConsoleApp.Messages;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Services
{
    public class AppService : IAppService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IMassTransitService _massTransitService;
        private readonly IMessageSender _messageSender;

        public AppService(ILogger<AppService> logger, IMassTransitService massTransitService, IMessageSender messageSender)
        {
            _logger = logger;
            _massTransitService = massTransitService;
            _messageSender = messageSender;
        }
        public async Task RunAsync()
        {
            _logger.LogInformation($"Starting the AppService...");

            await _massTransitService.StartAsync(CancellationToken.None);
            _logger.LogInformation($"Service Bus Started at {DateTime.UtcNow}");

            //Sending Messages
            for (int i = 0; i < 15; i++)
            {
                _logger.LogInformation($"Sending Message {i + 1}");
                await _messageSender.SendAsync(new SampleMessage($"Message {i + 1} was created at {DateTime.UtcNow}"), "SomeNewQueueXYZ");
                Thread.Sleep(1000);
            }
            _logger.LogInformation($"RunAsync Completed at {DateTime.UtcNow}");
        }
        public async Task StopAsync()
        {
            await _massTransitService.StopAsync(CancellationToken.None);
            _logger.LogInformation($"Service Bus Stopped at {DateTime.UtcNow}");
        }
    }
}
