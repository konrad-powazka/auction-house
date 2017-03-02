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
				"ElasticsearchFiles/bin/elasticsearch.bat");

			var elasticsearchCmdProcess = new Process
			{
				StartInfo = new ProcessStartInfo("cmd.exe", $@"/C {elasticsearchExecutablePath}")
			{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			try
			{
				elasticsearchCmdProcess.Start();

				Task.Run(() =>
				{
					while (!elasticsearchCmdProcess.StandardOutput.EndOfStream)
					{
						var line = elasticsearchCmdProcess.StandardOutput.ReadLine();
						Console.WriteLine(line);
					}
				});

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