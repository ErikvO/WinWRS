using System;
using System.Web;

namespace ErikvO.WinWRS.Utility.Shutdown
{
	public class ShutdownWindowsSMB : ShutdownBase
	{
		public override string Shutdown(string target, string username, string password, ShutdownMethod shutdownMethod)
		{
			String shutdownParameters;
			switch (shutdownMethod)
			{
				case ShutdownMethod.LogOff:
					shutdownParameters = "/l";
					break;
				case ShutdownMethod.ShutDown:
					shutdownParameters = "/s";
					break;
				case ShutdownMethod.Reboot:
					shutdownParameters = "/r";
					break;
				case ShutdownMethod.ForcedLogOff:
					shutdownParameters = "/l /f";
					break;
				case ShutdownMethod.ForcedShutdown:
					shutdownParameters = "/s /f";
					break;
				case ShutdownMethod.ForcedReboot:
					shutdownParameters = "/r /f";
					break;
				case ShutdownMethod.PowerOff:
					shutdownParameters = "/p";
					break;
				case ShutdownMethod.ForcedPowerOff:
					shutdownParameters = "/p /f";
					break;
				default:
					throw new NotImplementedException($"ShutdownType {shutdownMethod} is not supported");
			}

			String fileName = HttpContext.Current.Server.MapPath("~/Commandline/ShutdownWindowsSMB.bat");
			String arguments = $"{target} {username} \"{password}\" \"{shutdownParameters}\"";

			return new Commandline().Execute(fileName, arguments);
		}
	}
}