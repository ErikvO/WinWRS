using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ErikvO.WinWRS.Utility
{
	public class Commandline
	{
		public string Execute(string fileName, string arguments, Int32 timeout = 60000)
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = arguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,         //Make standard out available in code
				CreateNoWindow = true,                 //Don't create a window when running the batch file
			};

			//Start the process and wait for it to finish.
			using (Process process = Process.Start(psi))
			{
				//Handle StandardOutput output in separate thread, so that it doesn't block the started program when the buffers are full.
				StringBuilder outputSb = HandleOutput(process.StandardOutput);

				if (!process.WaitForExit(timeout))
				{
					process.CloseMainWindow();
					throw new TimeoutException($"Command: '{psi.FileName} {psi.Arguments}' did not complete within the set timeout ({timeout / 1000} sec.){Environment.NewLine}");
				}

				return outputSb.ToString();
			}
		}

		private StringBuilder HandleOutput(StreamReader outputStream)
		{
			StringBuilder outputSb = new StringBuilder();
			ThreadPool.QueueUserWorkItem(state =>
			{
				while (!outputStream.EndOfStream)
				{
					String line = outputStream.ReadLine();
					outputSb.AppendLine(line);
				}
			});
			return outputSb;
		}
	}
}