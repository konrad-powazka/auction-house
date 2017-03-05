using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AuctionHouse.ElasticsearchLauncher
{
	public class Program
	{
		private static void Main()
		{
			Console.Title = "AuctionHouse.ElasticsearchLauncher";

			var elasticsearchExecutablePath = Path.Combine(Environment.CurrentDirectory,
				"ElasticsearchFiles/bin/elasticsearch.bat --silent");

			var elasticsearchCmdProcess = new Process
			{
				StartInfo = new ProcessStartInfo("cmd.exe", $@"/C {elasticsearchExecutablePath}")
			{
					UseShellExecute = true,
					CreateNoWindow = false
				}
			};

			try
			{
				elasticsearchCmdProcess.Start();
				Console.WriteLine("Press any key to exit");
				Console.ReadKey();
			}
			finally
			{
				elasticsearchCmdProcess.CloseMainWindow();
			}
		}
	}
}