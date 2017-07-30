using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ErikvO.WinWRS.Utility
{
	public static class Encryption
	{
		private static readonly byte[] _key = new byte[] { 0xd5, 0xb4, 0x39, 0x12, 0xd4, 0x96, 0xbc, 0xf1, 0x0f, 0xde, 0xbf, 0x54, 0xb0, 0x3c, 0xce, 0xc3, 0x48, 0x03, 0x26, 0x86, 0x3d, 0xe7, 0xb1, 0x3f, 0x13, 0xc3, 0x30, 0xfb, 0xcf, 0xba, 0xd0, 0xf9 }; //32 bytes
		private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

		public static string Encrypt(string data)
		{
			byte[] decryptedBytes = Encoding.UTF8.GetBytes(data);
			byte[] iv = GenerateRandomBytes(16);
			byte[] encryptedBytes = DoCryptoOperation(decryptedBytes, _key, iv, true);
			return $"{Convert.ToBase64String(iv)}:{Convert.ToBase64String(encryptedBytes)}";
		}

		public static string Decrypt(string encryptedData)
		{
			string[] inputs = encryptedData.Split(':');
			byte[] iv = Convert.FromBase64String(inputs[0]);
			byte[] encryptedBytes = Convert.FromBase64String(inputs[1]);
			byte[] decryptedBytes = DoCryptoOperation(encryptedBytes, _key, iv, false);
			return Encoding.UTF8.GetString(decryptedBytes);
		}

		private static byte[] GenerateRandomBytes(int lengthBytes)
		{
			byte[] bytes = new byte[lengthBytes];
			_rng.GetBytes(bytes);
			return bytes;
		}

		private static byte[] DoCryptoOperation(byte[] inputData, byte[] key, byte[] iv, bool encrypt)
		{
			byte[] output;

			using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					ICryptoTransform cryptoTransform = encrypt ? aes.CreateEncryptor(key, iv) : aes.CreateDecryptor(key, iv);

					using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
						cs.Write(inputData, 0, inputData.Length);

					output = ms.ToArray();
				}
			}
			return output;
		}
	}
}