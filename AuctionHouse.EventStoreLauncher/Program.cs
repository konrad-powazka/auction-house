using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AuctionHouse.EventStoreLauncher
{
    // TODO: Use client instead
    public class Program
    {
        private static void Main()
        {
            var eventStoreProcessWorkingDirectory = Path.Combine(Environment.CurrentDirectory, "EventStoreFiles");
            var eventStoreExecPath = Path.Combine(eventStoreProcessWorkingDirectory, "EventStore.ClusterNode.exe");

            var eventStoreProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = eventStoreProcessWorkingDirectory,
                    FileName = eventStoreExecPath,
                    Arguments = "--db ./db --log ./logs",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                eventStoreProcess.Start();

                Task.Run(() =>
                {
                    while (!eventStoreProcess.StandardOutput.EndOfStream)
                    {
                        var line = eventStoreProcess.StandardOutput.ReadLine();
                        Console.WriteLine(line);
                    }
                });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                eventStoreProcess.Kill();
            }
        }
    }
}