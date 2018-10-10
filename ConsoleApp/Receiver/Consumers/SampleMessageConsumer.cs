using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;

namespace Receiver.Consumers
{
    public class SampleMessageConsumer : IConsumer<ISampleMessage>
    {
        private static int _count= 0;
        private readonly int _consumerId;
        private readonly ILogger<SampleMessageConsumer> _logger;

        public SampleMessageConsumer(ILogger<SampleMessageConsumer> logger)
        {
            _logger = logger;
            _count++;
            _consumerId = _count;
        }
        public async Task Consume(ConsumeContext<ISampleMessage> context)
        {
            try
            {
                _logger.LogWarning($"Consumer Counter {_count}");

                var message = context.Message;
                await Console.Out.WriteLineAsync($"Message Recieved: {message.GetType().Name} at {DateTime.UtcNow}");
                _logger.LogInformation($"Context.CorrelationId: {context.CorrelationId}");
                Thread.Sleep(5000);
                _logger.LogInformation($"Processing Completed for Consumer: {_consumerId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
