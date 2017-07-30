using System.Linq;
using System.Management;
using System.Runtime.InteropServices;

namespace ErikvO.WinWRS.Utility.Shutdown
{
	public class ShutdownWindowsWMI : ShutdownBase
	{
		public override string Shutdown(string target, string username, string password, ShutdownMethod shutdownMethod)
		{
			ConnectionOptions options = new ConnectionOptions()
			{
				EnablePrivileges = true,
				Username = username,
				Password = password
			};

			ManagementScope scope = new ManagementScope($@"\\{target}\root\cimv2", options);

			//Ensure target machine is online.
			try
			{
				scope.Connect();
			}
			catch (COMException e)
			{
				return e.ErrorCode.ToString();
			}
			catch
			{
				return "-1";
			}

			ManagementClass win32_OperatingSystem = new ManagementClass(scope, new ManagementPath(@"Win32_OperatingSystem"), null);
			ManagementBaseObject inParams = win32_OperatingSystem.GetMethodParameters("Win32Shutdown");
			inParams["Flags"] = (int)shutdownMethod;

			ManagementBaseObject outParams = win32_OperatingSystem
				.GetInstances()
				.Cast<ManagementObject>()
				.First()
				.InvokeMethod("Win32Shutdown", inParams, null);

			return outParams.Properties["ReturnValue"].Value.ToString();
		}
	}
}