using System.Linq;
using System.Net;
using System.Net.Sockets;
using ErikvO.WinWRS.Models;

namespace ErikvO.WinWRS.Business
{
	public class ComputerActions
	{
		private Computer _computer;
		public ComputerActions(Computer computer)
		{
			_computer = computer;
		}

		public void Wake()
		{
			MACAddress mac = new MACAddress(_computer.MAC);
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

		public int Reboot()
		{
			return ShutdownHelper.Shutdown(_computer.IP, _computer.UserName, _computer.Password, ShutdownHelper.ShutdownType.ForcedReboot);
		}

		public int Shutdown()
		{
			return ShutdownHelper.Shutdown(_computer.IP, _computer.UserName, _computer.Password, ShutdownHelper.ShutdownType.ForcedShutdown);
		}

		public Computer FillByName()
		{
			_computer.IP = GetIpByHostName()?.ToString() ?? "";
			if (_computer.IP != "")
				_computer.MAC = MACAddress.GetByIp(_computer.IP).ToString();
			return _computer;
		}

		public Computer FillByMac()
		{
			_computer.IP = new MACAddress(_computer.MAC).GetIp()?.ToString() ?? "";
			if (_computer.IP != "")
				_computer.Name = GetHostNameByIp().ToString();
			return _computer;
		}

		public Computer FillByIp()
		{
			_computer.Name = GetHostNameByIp().ToString();
			_computer.MAC = MACAddress.GetByIp(_computer.IP).ToString();
			return _computer;
		}

		private IPAddress GetIpByHostName()
		{
			return Dns
				.GetHostAddresses(_computer.Name)
				.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
				.FirstOrDefault();
		}

		private string GetHostNameByIp()
		{
			return Dns.GetHostEntry(_computer.IP).HostName;
		}
	}
}