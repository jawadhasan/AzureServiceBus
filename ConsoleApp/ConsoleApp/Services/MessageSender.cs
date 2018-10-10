using System;
using System.Threading.Tasks;
using ConsoleApp.Interfaces;
using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Services
{
    public class MessageSender : IMessageSender
    {
        private readonly IBusControl _bus;
        private readonly ILogger<MessageSender> _logger;
        private readonly string _queueName = "SomeNewQueueXYZ";
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessageSender(IBusControl bus, ISendEndpointProvider sendEndpointProvider, ILogger<MessageSender> logger)
        {
            _bus = bus;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SendAsyncV2(ISampleMessage sampleMessage, string queueAddress)
        {
            var endPoint = await _sendEndpointProvider.GetSendEndpoint(GetSendAddressUri(_queueName));
            await endPoint.Send(sampleMessage);
            _logger.LogInformation($"Message sent to AddressUri: {_queueName} ");
        }


        public async Task SendAsync(ISampleMessage sampleMessage, string queueAddress)
        {
            var addressUri = GetSendAddressUri(queueAddress);
            var endPoint = await _bus.GetSendEndpoint(addressUri);
            await endPoint.Send(sampleMessage);
            _logger.LogInformation($"Message sent to AddressUri: {addressUri} ");
        }
        private Uri GetSendAddressUri(string queueName)
        {
            var hostName = @"sb://" + _bus.Address.Host;
            var hostUri = new Uri(hostName);
            var address = new Uri(hostUri + queueName);
            return address;
        }
    }
}
