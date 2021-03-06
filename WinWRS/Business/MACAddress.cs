﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using ErikvO.WinWRS.Utility;

namespace ErikvO.WinWRS.Business
{
	public class MACAddress
	{
		private byte[] _bytes;

		public MACAddress(string hex)
			: this(StringToByteArray(hex)) { }

		public MACAddress(byte[] bytes)
		{
			if (bytes.Length != 6)
				throw new ArgumentException("MAC address must have 6 bytes");
			_bytes = bytes;
		}

		public byte this[int i]
		{
			get { return _bytes[i]; }
			set { _bytes[i] = value; }
		}

		public static byte[] StringToByteArray(string hex)
		{
			hex = hex
				.Replace(":", "")
				.Replace("-", "");

			return Enumerable.Range(0, hex.Length)
								  .Where(x => x % 2 == 0)
								  .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
								  .ToArray();
		}

		public override string ToString()
		{
			return BitConverter.ToString(_bytes);
		}

		public IPAddress GetIp()
		{
			String macString = ToString();

			IPAddress result = new Commandline()
				.Execute("arp.exe", "-a")
				.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Trim())
				.Where(line => line.IndexOf(macString, StringComparison.OrdinalIgnoreCase) >= 0)
				.Select(line => IPAddress.Parse(line.Substring(0, line.IndexOf(" "))))
				.FirstOrDefault();

			return result;
		}

		[DllImport("iphlpapi.dll", ExactSpelling = true)]
		private static extern int SendARP(uint DestIP, uint SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

		internal static MACAddress GetByIp(string ip)
		{
			return GetByIp(IPAddress.Parse(ip));
		}

		public static MACAddress GetByIp(IPAddress ipAddress)
		{
			uint uintAddress = BitConverter.ToUInt32(ipAddress.GetAddressBytes(), 0);
			byte[] macAddr = new byte[6];
			int macAddrLen = macAddr.Length;
			int retValue = SendARP(uintAddress, 0, macAddr, ref macAddrLen);
			if (retValue != 0)
				throw new Win32Exception(retValue, "SendARP failed.");

			return new MACAddress(macAddr);
		}
	}
}