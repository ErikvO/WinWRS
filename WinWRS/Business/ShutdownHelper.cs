using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;

namespace ErikvO.WinWRS.Business
{
	public class ShutdownHelper
	{
		public enum ShutdownType
		{
			LogOff = 0,
			ShutDown = 1,
			Reboot = 2,
			ForcedLogOff = 4,
			ForcedShutdown = 5,
			ForcedReboot = 6,
			PowerOff = 8,
			ForcedPowerOff = 12
		}

		// 
		// Machine is 
		// or IP address

		/// <summary>
		/// This function attempts to execute the Win32_OperatingSystem.Win32Shutdown method on the target machine 
		/// </summary>
		/// <param name="target">The remote computers name or IP address</param>
		/// <param name="username">The username of a remote user that is allowed to shutdown the machine.</param>
		/// <param name="password">The password of the remote user that is allowed to shutdown the machine.</param>
		/// <param name="shutdownType">The type of shutdown to use.</param>
		/// <returns>An error code</returns>
		static public int Shutdown(string target, string username, string password, ShutdownType shutdownType)
		{
			//ToDo: Add option to either use WMI or SMB.
			//net use \\computer\IPC$ password /USER:username
			//shutdown /r /f /t 60 /m \\computer /c "Rebooting computer. Use shutdown -a to cancel"
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
				return e.ErrorCode;
			}
			catch
			{
				return -1;
			}

			ManagementClass win32_OperatingSystem = new ManagementClass(scope, new ManagementPath(@"Win32_OperatingSystem"), null);
			ManagementBaseObject inParams = win32_OperatingSystem.GetMethodParameters("Win32Shutdown");
			inParams["Flags"] = (int)shutdownType;

			ManagementBaseObject outParams = win32_OperatingSystem
				.GetInstances()
				.Cast<ManagementObject>()
				.First()
				.InvokeMethod("Win32Shutdown", inParams, null);

			return Convert.ToInt32(outParams.Properties["ReturnValue"].Value);
		}
	}
}