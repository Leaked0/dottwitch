using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace RONIN.Kohl.Core.Security.Protection
{
	// Token: 0x0200001B RID: 27
	internal class Protection
	{
		//// Token: 0x0600007C RID: 124
		//[DllImport("kernel32.dll")]
		//private static extern IntPtr ZeroMemory(IntPtr addr, IntPtr size);

		//// Token: 0x0600007D RID: 125
		//[DllImport("kernel32.dll")]
		//private static extern IntPtr VirtualProtect(IntPtr lpAddress, IntPtr dwSize, IntPtr flNewProtect, ref IntPtr lpflOldProtect);

		//// Token: 0x0600007E RID: 126 RVA: 0x00008BD4 File Offset: 0x00006DD4
		//private static void EraseSection(IntPtr address, int size)
		//{
		//	IntPtr intPtr = (IntPtr)size;
		//	IntPtr flNewProtect = 0;
		//	Protection.VirtualProtect(address, intPtr, (IntPtr)64, ref flNewProtect);
		//	Protection.ZeroMemory(address, intPtr);
		//	IntPtr intPtr2 = 0;
		//	Protection.VirtualProtect(address, intPtr, flNewProtect, ref intPtr2);
		//}

		//// Token: 0x0600007F RID: 127 RVA: 0x00003846 File Offset: 0x00001A46
		//public static void Antis()
		//{
		//	Protection.DefaultDependencyAttribute();
		//	Protection.AssemblyHashAlgorithm();
		//}

		//// Token: 0x06000080 RID: 128 RVA: 0x00008C1C File Offset: 0x00006E1C
		//internal static void AssemblyHashAlgorithm()
		//{
		//	int num = new Random().Next(3000, 10000);
		//	DateTime now = DateTime.Now;
		//	Thread.Sleep(num);
		//	if ((DateTime.Now - now).TotalMilliseconds < (double)num)
		//	{
		//		Protection.MemberFilter("START CMD /C \"ECHO Emulation detected! (HResult 0x04) && PAUSE\" ");
		//		Process.GetCurrentProcess().Kill();
		//	}
		//}

		//// Token: 0x06000081 RID: 129 RVA: 0x00003852 File Offset: 0x00001A52
		//internal static void MemberFilter(string A_0)
		//{
		//	Process.Start(new ProcessStartInfo("cmd.exe", "/c " + A_0)
		//	{
		//		CreateNoWindow = true,
		//		UseShellExecute = false
		//	});
		//}

		//// Token: 0x06000082 RID: 130 RVA: 0x00008C80 File Offset: 0x00006E80
		//public static bool MEMORYBASICINFORMATION()
		//{
		//	bool flag = false;
		//	foreach (object obj in Process.GetCurrentProcess().Modules)
		//	{
		//		ProcessModule processModule = (ProcessModule)obj;
		//		if (processModule.ModuleName.EndsWith(".ni.dll"))
		//		{
		//			if (!flag)
		//			{
		//			}
		//			flag = true;
		//		}
		//	}
		//	return Debugger.IsAttached | !flag;
		//}

		//// Token: 0x06000083 RID: 131 RVA: 0x00003885 File Offset: 0x00001A85
		//public static void DefaultDependencyAttribute()
		//{
		//	new Thread(new ThreadStart(Protection.ByteEqualityComparer)).Start();
		//}

		//// Token: 0x06000084 RID: 132 RVA: 0x00008D14 File Offset: 0x00006F14
		//internal static void ByteEqualityComparer()
		//{
		//	string[] array = new string[]
		//	{
		//		"codecracker",
		//		"x32dbg",
		//		"x64dbg",
		//		"ida -",
		//		"charles",
		//		"dnspy",
		//		"simpleassembly",
		//		"peek",
		//		"httpanalyzer",
		//		"ieinspector",
		//		"httpdebug",
		//		"fiddler",
		//		"eaz",
		//		"dumper",
		//		"pc-ret",
		//		"krawk",
		//		"cracked.to",
		//		"patch",
		//		"wireshark",
		//		"proxifier",
		//		"hacker",
		//		"de4dot",
		//		"mitmproxy",
		//		"titanium"
		//	};
		//	Debugger.Log(0, null, "%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s%s");
		//	for (;;)
		//	{
		//		if (Debugger.IsAttached || Debugger.IsLogging() || Protection.MEMORYBASICINFORMATION())
		//		{
		//			Environment.Exit(0);
		//		}
		//		foreach (Process process in Process.GetProcesses())
		//		{
		//			if (process != Process.GetCurrentProcess())
		//			{
		//				for (int j = 0; j < array.Length; j++)
		//				{
		//					int id = Process.GetCurrentProcess().Id;
		//					if (process.ProcessName.ToLower().Contains(array[j]))
		//					{
		//						process.Kill();
		//						Protection.MemberFilter(string.Concat(new object[]
		//						{
		//							"START CMD /C \"ECHO Debugger program detected! (HResult 0x04)",
		//							process.ProcessName,
		//							" : {",
		//							j,
		//							"} && PAUSE\" "
		//						}));
		//						Process.GetCurrentProcess().Kill();
		//					}
		//					if (process.MainWindowTitle.ToLower().Contains(array[j]))
		//					{
		//						process.Kill();
		//						Protection.MemberFilter(string.Concat(new object[]
		//						{
		//							"START CMD /C \"ECHO Debugger program detected! (HResult 0x04)",
		//							process.ProcessName,
		//							" : {",
		//							j,
		//							"} && PAUSE\" "
		//						}));
		//						Process.GetCurrentProcess().Kill();
		//					}
		//				}
		//			}
		//		}
		//		Thread.Sleep(1000);
		//	}
		//}

		//// Token: 0x06000085 RID: 133 RVA: 0x00008FF8 File Offset: 0x000071F8
		//internal static void AntiDump()
		//{
		//	IntPtr baseAddress = Process.GetCurrentProcess().MainModule.BaseAddress;
		//	int num = Marshal.ReadInt32((IntPtr)(baseAddress.ToInt32() + 60));
		//	short num2 = Marshal.ReadInt16((IntPtr)(baseAddress.ToInt32() + num + 6));
		//	Protection.EraseSection(baseAddress, 30);
		//	for (int i = 0; i < Protection.peheaderdwords.Length; i++)
		//	{
		//		Protection.EraseSection((IntPtr)(baseAddress.ToInt32() + num + Protection.peheaderdwords[i]), 4);
		//	}
		//	for (int j = 0; j < Protection.peheaderwords.Length; j++)
		//	{
		//		Protection.EraseSection((IntPtr)(baseAddress.ToInt32() + num + Protection.peheaderwords[j]), 2);
		//	}
		//	for (int k = 0; k < Protection.peheaderbytes.Length; k++)
		//	{
		//		Protection.EraseSection((IntPtr)(baseAddress.ToInt32() + num + Protection.peheaderbytes[k]), 1);
		//	}
		//	int l = 0;
		//	int num3 = 0;
		//	while (l <= (int)num2)
		//	{
		//		if (num3 == 0)
		//		{
		//			Protection.EraseSection((IntPtr)(baseAddress.ToInt32() + num + 250 + 40 * l + 32), 2);
		//		}
		//		Protection.EraseSection((IntPtr)(baseAddress.ToInt32() + num + 250 + 40 * l + Protection.sectiontabledwords[num3]), 4);
		//		num3++;
		//		if (num3 == Protection.sectiontabledwords.Length)
		//		{
		//			l++;
		//			num3 = 0;
		//		}
		//	}
		//}

		//// Token: 0x04000050 RID: 80
		//private static int[] sectiontabledwords = new int[]
		//{
		//	8,
		//	12,
		//	16,
		//	20,
		//	24,
		//	28,
		//	36
		//};

		//// Token: 0x04000051 RID: 81
		//private static int[] peheaderbytes = new int[]
		//{
		//	26,
		//	27
		//};

		//// Token: 0x04000052 RID: 82
		//private static int[] peheaderwords = new int[]
		//{
		//	4,
		//	22,
		//	24,
		//	64,
		//	66,
		//	68,
		//	70,
		//	72,
		//	74,
		//	76,
		//	92,
		//	94
		//};

		//// Token: 0x04000053 RID: 83
		//private static int[] peheaderdwords = new int[]
		//{
		//	0,
		//	8,
		//	12,
		//	16,
		//	22,
		//	28,
		//	32,
		//	40,
		//	44,
		//	52,
		//	60,
		//	76,
		//	80,
		//	84,
		//	88,
		//	96,
		//	100,
		//	104,
		//	108,
		//	112,
		//	116,
		//	260,
		//	264,
		//	268,
		//	272,
		//	276,
		//	284
		//};
	}
}
