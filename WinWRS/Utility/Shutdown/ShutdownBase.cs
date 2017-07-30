namespace ErikvO.WinWRS.Utility.Shutdown
{
	public enum ShutdownType
	{
		WindowsSMB,
		WindowsWMI
	}

	public enum ShutdownMethod
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

	public abstract class ShutdownBase
	{
		/// <summary>
		/// This function attempts to execute the Win32_OperatingSystem.Win32Shutdown method on the target machine 
		/// </summary>
		/// <param name="target">The remote computers name or IP address</param>
		/// <param name="username">The username of a remote user that is allowed to shutdown the machine.</param>
		/// <param name="password">The password of the remote user that is allowed to shutdown the machine.</param>
		/// <param name="shutdownMethod">The type of shutdown to use.</param>
		/// <returns>An error code</returns>
		public abstract string Shutdown(string target, string username, string password, ShutdownMethod shutdownMethod);
	}
}