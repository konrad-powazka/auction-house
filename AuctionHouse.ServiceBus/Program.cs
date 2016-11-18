using System;
using AuctionHouse.Application;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Reflection;
using Autofac;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;

namespace AuctionHouse.ServiceBus
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "AuctionHouse.ServiceBus";
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.ServiceBus");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
                .DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(typeof(ApplicationAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>)).AsImplementedInterfaces();

            var container = containerBuilder.Build();

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations => { customizations.ExistingLifetimeScope(container); });

            var endpointInstance = Endpoint.Start(endpointConfiguration).Result;

            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                endpointInstance.Stop().Wait();
            }
        }
    }
}