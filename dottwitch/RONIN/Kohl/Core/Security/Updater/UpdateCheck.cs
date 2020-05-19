using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using RONIN.Core.BackEnd.Variables;

namespace RONIN.Kohl.Core.Security.Updater
{
	// Token: 0x02000018 RID: 24
	internal class UpdateCheck
	{
		// Token: 0x0600006C RID: 108 RVA: 0x0000379E File Offset: 0x0000199E
		public static string GetLastVersion()
		{
			return new WebClient().DownloadString(Var.authUrl + "/api/version");
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00008800 File Offset: 0x00006A00
		public static void Update()
		{
			UpdateCheck.Write("Downloading new update, please wait ... 0% ", ConsoleColor.White);
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadProgressChanged += UpdateCheck.DownloadProgressChanged;
				webClient.DownloadFileAsync(new Uri(Var.authUrl + "/api/" + Var.title + ".exe"), "_" + Var.title + ".exe");
			}
			Thread.Sleep(100);
			if (UpdateCheck.updateDownloaded)
			{
				Process.Start(new ProcessStartInfo
				{
					Arguments = string.Concat(new string[]
					{
						"/c ping localhost -n 4 >nul & del \"",
						Application.ExecutablePath,
						"\" & ren _",
						Var.title,
						".exe ",
						Var.title,
						".exe & ",
						Var.title,
						".exe"
					}),
					FileName = "cmd.exe"
				});
				UpdateCheck.Exit("New update downloaded", 2500);
				return;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000037B9 File Offset: 0x000019B9
		private static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			UpdateCheck.Write("\rDownloading new update, please wait ... " + e.ProgressPercentage + "% ", ConsoleColor.White);
			if (e.ProgressPercentage == 100)
			{
				UpdateCheck.updateDownloaded = true;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00008918 File Offset: 0x00006B18
		public static void Write(string text, ConsoleColor color = ConsoleColor.White)
		{
			object obj = UpdateCheck.consoleLock;
			lock (obj)
			{
				Console.ForegroundColor = color;
				Console.Write(text);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000037EC File Offset: 0x000019EC
		public static void Exit(string message, int delay = 2500)
		{
			Console.Clear();
			Console.Write(message + ", exiting ... ");
			Thread.Sleep(delay);
			Environment.Exit(0);
		}

		// Token: 0x0400004A RID: 74
		private static bool updateDownloaded = false;

		// Token: 0x0400004B RID: 75
		private static readonly object consoleLock = new object();
	}
}
