using System;

namespace ErikvO.WinWRS.Utility.Shutdown
{
	public class ShutdownFactory
	{
		public ShutdownBase GetShutdown(ShutdownType shutdownType)
		{
			ShutdownBase result;
			switch (shutdownType)
			{
				case ShutdownType.WindowsSMB:
					result = new ShutdownWindowsSMB();
					break;
				case ShutdownType.WindowsWMI:
					result = new ShutdownWindowsWMI();
					break;
				default:
					throw new NotImplementedException($"Unable to create an instance for ShutdownType {shutdownType}.");
			}
			return result;
		}
	}
}