using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace RONIN.Core.BackEnd.Auth
{
	// Token: 0x02000025 RID: 37
	internal class HWID
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x0000A658 File Offset: 0x00008858
		public static string Value()
		{
			if (string.IsNullOrEmpty(HWID._fingerPrint))
			{
				HWID._fingerPrint = HWID.GetHash(string.Concat(new string[]
				{
					"CPU >> ",
					HWID.CpuId(),
					"\nBIOS >> ",
					HWID.BiosId(),
					"\nBASE >> ",
					HWID.BaseId(),
					"\nDISK >> ",
					HWID.DiskId(),
					"\nVIDEO >> ",
					HWID.VideoId(),
					"\nMAC >> ",
					HWID.MacId()
				}));
			}
			return HWID._fingerPrint;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000A724 File Offset: 0x00008924
		private static string GetHash(string string_0)
		{
			MD5 md = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.ASCII.GetBytes(string_0);
			return HWID.GetHexString(md.ComputeHash(bytes));
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000A754 File Offset: 0x00008954
		private static string GetHexString(IList<byte> ilist_0)
		{
			string text = string.Empty;
			for (int i = 0; i < ilist_0.Count; i++)
			{
				byte b = ilist_0[i];
				int num = (int)b;
				int num2 = num & 15;
				int num3 = num >> 4 & 15;
				if (num3 > 9)
				{
					text += ((char)(num3 - 10 + 65)).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					text += num3.ToString(CultureInfo.InvariantCulture);
				}
				if (num2 > 9)
				{
					text += ((char)(num2 - 10 + 65)).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					text += num2.ToString(CultureInfo.InvariantCulture);
				}
				if (i + 1 != ilist_0.Count && (i + 1) % 2 == 0)
				{
					text += "-";
				}
			}
			return text;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000A870 File Offset: 0x00008A70
		private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
		{
			string text = "";
			ManagementClass managementClass = new ManagementClass(wmiClass);
			ManagementObjectCollection instances = managementClass.GetInstances();
			foreach (ManagementBaseObject managementBaseObject in instances)
			{
				if (!(managementBaseObject[wmiMustBeTrue].ToString() != "True") && !(text != ""))
				{
					try
					{
						text = managementBaseObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000A914 File Offset: 0x00008B14
		private static string Identifier(string wmiClass, string wmiProperty)
		{
			string text = "";
			ManagementClass managementClass = new ManagementClass(wmiClass);
			ManagementObjectCollection instances = managementClass.GetInstances();
			foreach (ManagementBaseObject managementBaseObject in instances)
			{
				if (!(text != ""))
				{
					try
					{
						text = managementBaseObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000A9A0 File Offset: 0x00008BA0
		private static string CpuId()
		{
			string text = HWID.Identifier("Win32_Processor", "UniqueId");
			string result;
			if (text != "")
			{
				result = text;
			}
			else
			{
				text = HWID.Identifier("Win32_Processor", "ProcessorId");
				if (text != "")
				{
					result = text;
				}
				else
				{
					text = HWID.Identifier("Win32_Processor", "Name");
					if (text == "")
					{
						text = HWID.Identifier("Win32_Processor", "Manufacturer");
					}
					text += HWID.Identifier("Win32_Processor", "MaxClockSpeed");
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000AA38 File Offset: 0x00008C38
		private static string BiosId()
		{
			return string.Concat(new string[]
			{
				HWID.Identifier("Win32_BIOS", "Manufacturer"),
				HWID.Identifier("Win32_BIOS", "SMBIOSBIOSVersion"),
				HWID.Identifier("Win32_BIOS", "IdentificationCode"),
				HWID.Identifier("Win32_BIOS", "SerialNumber"),
				HWID.Identifier("Win32_BIOS", "ReleaseDate"),
				HWID.Identifier("Win32_BIOS", "Version")
			});
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000AADC File Offset: 0x00008CDC
		private static string DiskId()
		{
			return HWID.Identifier("Win32_DiskDrive", "Model") + HWID.Identifier("Win32_DiskDrive", "Manufacturer") + HWID.Identifier("Win32_DiskDrive", "Signature") + HWID.Identifier("Win32_DiskDrive", "TotalHeads");
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000AB2C File Offset: 0x00008D2C
		private static string BaseId()
		{
			return HWID.Identifier("Win32_BaseBoard", "Model") + HWID.Identifier("Win32_BaseBoard", "Manufacturer") + HWID.Identifier("Win32_BaseBoard", "Name") + HWID.Identifier("Win32_BaseBoard", "SerialNumber");
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000AB7C File Offset: 0x00008D7C
		private static string VideoId()
		{
			return HWID.Identifier("Win32_VideoController", "DriverVersion") + HWID.Identifier("Win32_VideoController", "Name");
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000ABB0 File Offset: 0x00008DB0
		private static string MacId()
		{
			return HWID.Identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
		}

		// Token: 0x0400007A RID: 122
		private static string _fingerPrint = string.Empty;
	}
}
