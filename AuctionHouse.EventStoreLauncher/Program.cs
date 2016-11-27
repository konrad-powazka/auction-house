using System;
using EventStore.ClientAPI.Embedded;

namespace AuctionHouse.EventStoreLauncher
{
    public class Program
    {
        private static void Main()
        {
            Console.Title = "AuctionHouse.EventStoreLauncher";

            var nodeBuilder = EmbeddedVNodeBuilder
                .AsSingleNode()
                .OnDefaultEndpoints()
                .RunInMemory();

            var node = nodeBuilder.Build();
            node.StartAndWaitUntilReady().Wait();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            node.Stop();
        }
    }
}