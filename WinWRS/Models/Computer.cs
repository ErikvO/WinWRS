using ErikvO.WinWRS.Utility;
using ErikvO.WinWRS.Utility.Shutdown;
using LiteDB;
using Newtonsoft.Json;

namespace ErikvO.WinWRS.Models
{
	public class Computer
	{
		public int Id { get; internal set; }
		public string Name { get; internal set; }
		public string MAC { get; internal set; }
		public string IP { get; internal set; }
		public string UserName { get; internal set; }

		[BsonIgnore]
		public string Password { get; internal set; }

		public ShutdownType ShutdownType { get; internal set; }

		private string _encryptedPassword = null;
		[JsonIgnore]
		public string EncryptedPassword
		{
			get
			{
				if (_encryptedPassword == null && Password != null && Password != "")
					_encryptedPassword = Encryption.Encrypt(Password);

				return _encryptedPassword;
			}
			internal set { _encryptedPassword = value; }
		}

		public string GetDecryptedPassword()
		{
			string result = null;
			if (EncryptedPassword != null)
				result = Encryption.Decrypt(EncryptedPassword);
			return result;
		}

		private Computer() { }

		public Computer(int id, string name, string mac, string ip, string userName, string password, ShutdownType shutdownType)
		{
			Id = id;
			Name = name;
			MAC = mac;
			IP = ip;
			UserName = userName;
			Password = password;
			ShutdownType = shutdownType;
		}
	}
}