using System;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;

namespace ConsoleApp.Infrastructure
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(ServiceBusSettings settings,
            Action<IServiceBusBusFactoryConfigurator, IServiceBusHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(sbc =>
            {
                var host = sbc.Host(settings.ServiceBusConnectionString, h =>
                {
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(settings.ServiceBusKeyName,
                        settings.ServiceBusSharedAccessKey,
                        TimeSpan.FromDays(1),
                        TokenScope.Namespace);
                });
                registrationAction?.Invoke(sbc, host);
            });
        }
    }
}
