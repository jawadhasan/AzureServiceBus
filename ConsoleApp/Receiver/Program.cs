using System;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceBus;
using Receiver.Consumers;
using Receiver.Interfaces;
using Receiver.Services;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Configure Services
                var services = new ServiceCollection();
                ConfigureServices(services);

                //Create a service provider from the service collection
                var serviceProvider = services.BuildServiceProvider();

                //Resolve the service
                var appService = serviceProvider.GetService<IAppService>();

                //Run the application
                appService.RunAsync().Wait();

                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                appService.StopAsync().Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            //add logging
            serviceCollection.AddSingleton(new LoggerFactory().AddConsole());
            serviceCollection.AddLogging();

            //add services
            serviceCollection.AddTransient<IAppService, AppService>();

            //MassTransit
            serviceCollection.AddSingleton(provider => Bus.Factory.CreateUsingAzureServiceBus(sbc =>
            {
                var connectionString = "";
                var serviceBusSharedAccessKey = "";
                var serviceBusKeyName = "";
                var host = sbc.Host(connectionString, h =>
                {
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                        serviceBusKeyName,
                        serviceBusSharedAccessKey,
                        TimeSpan.FromDays(1),
                        TokenScope.Namespace);
                });

                sbc.ReceiveEndpoint(host, "SomeNewQueueXYZ", e =>
                {
                    //EndpointConvention.Map<SampleMessage>(e.InputAddress);
                    //e.Consumer<SampleMessageConsumer>();

                    var logger = provider.GetRequiredService<ILogger<SampleMessageConsumer>>();
                    e.Consumer<SampleMessageConsumer>(() => new SampleMessageConsumer(logger));

                });
            }));

            //add Services
            serviceCollection.AddSingleton<IMassTransitService, MassTransitService>();
        }
    }
}
