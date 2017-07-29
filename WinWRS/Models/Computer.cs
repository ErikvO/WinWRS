namespace ErikvO.WinWRS.Models
{
	public class Computer
	{
		public int Id { get; internal set; }
		public string Name { get; internal set; }
		public string MAC { get; internal set; }
		public string IP { get; internal set; }
		public string UserName { get; internal set; }
		public string Password { get; internal set; }

		private Computer() { }

		public Computer(int id, string name, string mac, string ip, string userName, string password)
		{
			Id = id;
			Name = name;
			MAC = mac;
			IP = ip;
			UserName = userName;
			Password = password;
		}
	}
}