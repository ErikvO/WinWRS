using System.Linq;
using System.Net;
using System.Net.Sockets;
using ErikvO.WinWRS.Models;

namespace ErikvO.WinWRS.Business
{
	public static class ComputerExtensions
	{
		public static void Wake(this Computer computer)
		{
			MACAddress mac = new MACAddress(computer.MAC);
			byte[] packet = new byte[17 * 6];

			for (int i = 0; i < 6; i++)
				packet[i] = 0xff;

			for (int i = 1; i <= 16; i++)
				for (int j = 0; j < 6; j++)
					packet[i * 6 + j] = mac[j];

			UdpClient client = new UdpClient();
			client.Connect(IPAddress.Broadcast, 0);
			client.Send(packet, packet.Length);
		}

		public static int Reboot(this Computer computer)
		{
			return ShutdownHelper.Shutdown(computer.IP, computer.UserName, computer.Password, ShutdownHelper.ShutdownType.ForcedReboot);
		}

		public static int Shutdown(this Computer computer)
		{
			return ShutdownHelper.Shutdown(computer.IP, computer.UserName, computer.Password, ShutdownHelper.ShutdownType.ForcedShutdown);
		}

		public static Computer FillByName(this Computer computer)
		{
			computer.IP = computer.GetIpByHostName()?.ToString() ?? "";
			if (computer.IP != "")
				computer.MAC = MACAddress.GetByIp(computer.IP).ToString();
			return computer;
		}

		public static Computer FillByMac(this Computer computer)
		{
			computer.IP = new MACAddress(computer.MAC).GetIp()?.ToString() ?? "";
			if (computer.IP != "")
				computer.Name = computer.GetHostNameByIp().ToString();
			return computer;
		}

		public static Computer FillByIp(this Computer computer)
		{
			computer.Name = computer.GetHostNameByIp().ToString();
			computer.MAC = MACAddress.GetByIp(computer.IP).ToString();
			return computer;
		}

		private static IPAddress GetIpByHostName(this Computer computer)
		{
			return Dns
				.GetHostAddresses(computer.Name)
				.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
				.FirstOrDefault();
		}

		private static string GetHostNameByIp(this Computer computer)
		{
			return Dns.GetHostEntry(computer.IP).HostName;
		}
	}
}
