using System;
using ConsoleApp.Infrastructure;
using ConsoleApp.Interfaces;
using ConsoleApp.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
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
                Console.Write(e.Message);
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
            var serviceBusSettings = new ServiceBusSettings
            {
                ServiceBusConnectionString = "",
                ServiceBusKeyName = "",
                ServiceBusSharedAccessKey = ""
            };
            serviceCollection.AddSingleton(provider => BusConfigurator.ConfigureBus(serviceBusSettings));
            serviceCollection.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            serviceCollection.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            serviceCollection.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());


            //add Services
            serviceCollection.AddSingleton<IMassTransitService, MassTransitService>();
            serviceCollection.AddTransient<IMessageSender, MessageSender>();
        }
    }
}
