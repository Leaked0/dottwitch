using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using Leaf.xNet;
using RONIN.Core.BackEnd.Data;
using RONIN.Core.BackEnd.Design;
using RONIN.Core.BackEnd.Variables;

namespace RONIN.Core.BackEnd.SlavLib
{
	// Token: 0x0200001F RID: 31
	internal class SlavCore
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00009244 File Offset: 0x00007444
		public static void Checker(Color color, string dataType, bool useProxies)
		{
			DataCore dataCore = new DataCore();
			dataCore.loadDataList(color, dataType);
			if (useProxies)
			{
				dataCore.loadProxyConsole();
			}
			Var.dateNow = DateTime.Now.ToString("dd.MM_hh.mm");
			Var.folder = Environment.CurrentDirectory + "\\Results\\Results_" + Var.dateNow + "\\";
			if (!Directory.Exists(Var.folder))
			{
				Directory.CreateDirectory(Var.folder);
			}
			Var.fileName = Path.GetFileNameWithoutExtension(dataCore.fileName);
			RDesign.ClearLastLine(2);
			for (;;)
			{
				try
				{
					RDesign.TimeText("Amount of threads to use:", color);
					RDesign.UserInput(color);
					Var.threads = Convert.ToInt32(Console.ReadLine());
					break;
				}
				catch (OverflowException)
				{
					RDesign.Error("Please choose a smaller number for threads.");
					Thread.Sleep(2000);
					RDesign.ClearLastLine(3);
				}
				catch (FormatException)
				{
					RDesign.Error("Please write a number for threads.");
					Thread.Sleep(2000);
					RDesign.ClearLastLine(3);
				}
			}
			RDesign.ClearLastLine(2);
			for (;;)
			{
				try
				{
					if (dataCore.Proxies.Count != 0)
					{
						for (;;)
						{
							RDesign.TimeText("Choose your proxy type | 0 for HTTP | 1 for SOCKS4 | 2 for SOCKS5", color);
							Var.proxyType = Convert.ToInt32(Console.ReadLine());
							if (Var.proxyType == 0 || Var.proxyType == 1 || Var.proxyType == 2 || Var.proxyType == 3)
							{
								break;
							}
							RDesign.Error("Please choose a valid option");
							Thread.Sleep(2000);
							RDesign.ClearLastLine(3);
						}
						RDesign.ClearLastLine(2);
						foreach (string item in dataCore.Proxies)
						{
							Var.proxies.Add(item);
						}
						dataCore.Proxies.Clear();
					}
					break;
				}
				catch (OverflowException)
				{
					RDesign.Error("Please choose a valid option.");
					Thread.Sleep(2000);
					RDesign.ClearLastLine(3);
				}
				catch (FormatException)
				{
					RDesign.Error("Please use a number to select an option.");
					Thread.Sleep(2000);
					RDesign.ClearLastLine(3);
				}
			}
			foreach (string item2 in dataCore.Combo)
			{
				Var.comboQueue.Add(item2);
			}
			dataCore.Combo.Clear();
			RDesign.TimeText(string.Format("Loaded: {0} {1}!", Var.comboQueue.Count, dataType), color);
			if (Var.proxies.Count != 0)
			{
				RDesign.TimeText(string.Format("Loaded: {0} proxies!", Var.proxies.Count), color);
			}
			RDesign.TimeText(string.Format("Threads: {0}", Var.threads), color);
			RDesign.TimeText("Starting...", color);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000957C File Offset: 0x0000777C
		public static ProxyClient LoadProxyType()
		{
			string proxyAddress = Var.proxies.ElementAt(Var.rnd.Next(Var.proxies.Count));
			ProxyClient result;
			switch (Var.proxyType)
			{
			case 0:
				result = HttpProxyClient.Parse(proxyAddress);
				break;
			case 1:
				result = Socks4ProxyClient.Parse(proxyAddress);
				break;
			case 2:
				result = Socks5ProxyClient.Parse(proxyAddress);
				break;
			case 3:
				result = null;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000095E8 File Offset: 0x000077E8
		public static Tuple<Check, string> Get(HttpRequest req, string url, List<string> headers = null, List<string> successKeys = null, List<string> failureKeys = null, List<string> retryKeys = null, string userAgent = null)
		{
			Tuple<Check, string> result;
			try
			{
				req.IgnoreProtocolErrors = true;
				req.KeepAlive = true;
				req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback, new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
				successKeys = (successKeys ?? new List<string>());
				failureKeys = (failureKeys ?? new List<string>());
				retryKeys = (retryKeys ?? new List<string>());
				headers = (headers ?? new List<string>());
				foreach (string text in headers)
				{
					req.AddHeader(text.Split(new char[]
					{
						':'
					})[0], text.Split(new char[]
					{
						':'
					})[1]);
				}
				if (userAgent != null)
				{
					req.UserAgent = userAgent;
				}
				else
				{
					req.UserAgent = Http.ChromeUserAgent();
				}
				string text2 = req.Get(url, null).ToString();
				foreach (string value in successKeys)
				{
					if (text2.Contains(value))
					{
						return new Tuple<Check, string>(Check.Valid, text2);
					}
				}
				foreach (string value2 in failureKeys)
				{
					if (text2.Contains(value2))
					{
						return new Tuple<Check, string>(Check.Invalid, text2);
					}
				}
				foreach (string value3 in retryKeys)
				{
					if (text2.Contains(value3))
					{
						return new Tuple<Check, string>(Check.Retry, text2);
					}
				}
				if (successKeys.Count == 0 && failureKeys.Count == 0 && retryKeys.Count == 0)
				{
					result = new Tuple<Check, string>(Check.Valid, text2);
				}
				else
				{
					result = new Tuple<Check, string>(Check.Retry, "No keys founds.");
				}
			}
			catch (HttpException ex)
			{
				result = new Tuple<Check, string>(Check.Retry, ex.ToString());
			}
			catch (NullReferenceException ex2)
			{
				result = new Tuple<Check, string>(Check.Retry, ex2.ToString());
			}
			catch (Exception ex3)
			{
				result = new Tuple<Check, string>(Check.Error, ex3.ToString());
			}
			return result;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00009928 File Offset: 0x00007B28
		public static Tuple<Check, string> Post(HttpRequest req, string url, string data, string contentType, List<string> headers = null, List<string> successKeys = null, List<string> failureKeys = null, List<string> retryKeys = null, string userAgent = null)
		{
			Tuple<Check, string> result;
			try
			{
				req.IgnoreProtocolErrors = true;
				req.KeepAlive = true;
				req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback, new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
				successKeys = (successKeys ?? new List<string>());
				failureKeys = (failureKeys ?? new List<string>());
				retryKeys = (retryKeys ?? new List<string>());
				headers = (headers ?? new List<string>());
				foreach (string text in headers)
				{
					req.AddHeader(text.Split(new char[]
					{
						':'
					})[0], text.Split(new char[]
					{
						':'
					})[1]);
				}
				if (userAgent != null)
				{
					req.UserAgent = userAgent;
				}
				else
				{
					req.UserAgent = Http.ChromeUserAgent();
				}
				string text2 = req.Post(url, data, contentType).ToString();
				foreach (string value in successKeys)
				{
					if (text2.Contains(value))
					{
						return new Tuple<Check, string>(Check.Valid, text2);
					}
				}
				foreach (string value2 in failureKeys)
				{
					if (text2.Contains(value2))
					{
						return new Tuple<Check, string>(Check.Invalid, text2);
					}
				}
				foreach (string value3 in retryKeys)
				{
					if (text2.Contains(value3))
					{
						return new Tuple<Check, string>(Check.Retry, text2);
					}
				}
				if (successKeys.Count == 0 && failureKeys.Count == 0 && retryKeys.Count == 0)
				{
					result = new Tuple<Check, string>(Check.Valid, text2);
				}
				else
				{
					result = new Tuple<Check, string>(Check.Retry, "No keys founds.");
				}
			}
			catch (HttpException ex)
			{
				result = new Tuple<Check, string>(Check.Retry, ex.ToString());
			}
			catch (NullReferenceException ex2)
			{
				result = new Tuple<Check, string>(Check.Retry, ex2.ToString());
			}
			catch (Exception ex3)
			{
				result = new Tuple<Check, string>(Check.Error, ex3.ToString());
			}
			return result;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00009C6C File Offset: 0x00007E6C
		private static void SaveProgress(int num)
		{
			num += Var.oldCheckedCnt;
			Var.lock_.EnterWriteLock();
			try
			{
				using (StreamWriter streamWriter = new StreamWriter("Progress\\" + Path.GetFileName(Var.fileName) + ".txt", false))
				{
					streamWriter.WriteLine(num);
				}
			}
			finally
			{
				Var.lock_.ExitWriteLock();
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00009CEC File Offset: 0x00007EEC
		private static void t_Elapsed2(object sender, ElapsedEventArgs e)
		{
			Var.proxies.Clear();
			using (Stream stream = new WebClient().OpenRead(Var.proxyUrl))
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					string item;
					while ((item = streamReader.ReadLine()) != null)
					{
						Var.proxies.Add(item);
					}
				}
			}
			RDesign.TimeText("Loaded proxies:" + Var.proxies.Count, Var.color);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000389D File Offset: 0x00001A9D
		private static void t_Elapsed(object sender, ElapsedEventArgs e)
		{
			Var.cpm2 = Var.cpm1;
			Var.cpm1 = 0;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00009D88 File Offset: 0x00007F88
		public static void UpdateTitle(string customField1 = null, int customInt1 = 0, string customField2 = null, int customInt2 = 0)
		{
			if (string.IsNullOrEmpty(customField1))
			{
				Console.Title = string.Concat(new object[]
				{
					string.Format("{0} | Hits: {1} | Invalid: {2} | Checked: {3} | Remaining: {4} | Retries: {5} | Errors: {6} | CPM: {7}", new object[]
					{
						Var.title,
						Var.hitCnt,
						Var.invalidCnt,
						Var.checkedCnt,
						Var.comboQueue.Count,
						Var.retryCnt,
						Var.errorCnt,
						Var.cpm2 * 12
					})
				});
			}
			else if (!string.IsNullOrEmpty(customField1) && string.IsNullOrEmpty(customField2))
			{
				Console.Title = string.Concat(new object[]
				{
					string.Format("{0} | Hits: {1} | {2}: {3} | Invalid: {4} | Checked: {5} | Remaining: {6} | Retries: {7} | Errors: {8} | CPM: {9}", new object[]
					{
						Var.title,
						Var.hitCnt,
						customField1,
						customInt1,
						Var.invalidCnt,
						Var.checkedCnt,
						Var.comboQueue.Count,
						Var.retryCnt,
						Var.errorCnt,
						Var.cpm2 * 12
					})
				});
			}
			else
			{
				Console.Title = string.Concat(new object[]
				{
					string.Format("{0} | Hits: {1} | {2}: {3} | {4}: {5} | Invalid: {6} | Checked: {7} | Remaining: {8} | Retries: {9} | Errors: {10} | CPM: {11}", new object[]
					{
						Var.title,
						Var.hitCnt,
						customField1,
						customInt1,
						customField2,
						customInt2,
						Var.invalidCnt,
						Var.checkedCnt,
						Var.comboQueue.Count,
						Var.retryCnt,
						Var.errorCnt,
						Var.cpm2 * 12
					})
				});
			}
			SlavCore.SaveProgress(Var.checkedCnt);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000038AF File Offset: 0x00001AAF
		public static void StartTimer()
		{
			System.Timers.Timer timer = new System.Timers.Timer(5000.0);
			timer.AutoReset = true;
			timer.Elapsed += SlavCore.t_Elapsed;
			timer.Start();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000038DD File Offset: 0x00001ADD
		public static void StartTimer2(int int_0)
		{
			int_0 = SlavCore.ConvertMinutesToMilliseconds(int_0);
			System.Timers.Timer timer = new System.Timers.Timer((double)int_0);
			timer.AutoReset = true;
			timer.Elapsed += SlavCore.t_Elapsed2;
			timer.Start();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000A044 File Offset: 0x00008244
		public static int ConvertMinutesToMilliseconds(int minutes)
		{
			return (int)TimeSpan.FromMinutes((double)minutes).TotalMilliseconds;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000390C File Offset: 0x00001B0C
		public static void CheckProgress()
		{
			for (;;)
			{
				if (Var.threads == 0)
				{
					Console.WriteLine(Environment.NewLine);
					Console.WriteLine("Done Checking! Press any key to exit.");
					Console.ReadKey();
					Process.GetCurrentProcess().Kill();
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000A064 File Offset: 0x00008264
		public static void SaveData(string account, string textfileName, List<string> capture = null, bool appendLines = true)
		{
			capture = (capture ?? new List<string>());
			Var.lock_.EnterWriteLock();
			try
			{
				using (FileStream fileStream = new FileStream(string.Concat(new string[]
				{
					Var.folder,
					Var.fileName,
					"_",
					textfileName,
					".txt"
				}), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						if (capture.Count == 0)
						{
							streamWriter.WriteLine(account);
						}
						else if (appendLines)
						{
							streamWriter.WriteLine("------------------" + Var.title + "------------------");
							streamWriter.WriteLine("Account: " + account);
							foreach (string value in capture)
							{
								streamWriter.Write(value);
							}
							streamWriter.WriteLine();
							streamWriter.WriteLine("------------------" + Var.title + "------------------");
							streamWriter.WriteLine();
						}
						else
						{
							streamWriter.Write(account + " | ");
							foreach (string str in capture)
							{
								streamWriter.Write(str + " | ");
							}
							streamWriter.WriteLine();
						}
					}
				}
			}
			finally
			{
				Var.lock_.ExitWriteLock();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000A284 File Offset: 0x00008484
		public static void StartThreading()
		{
			for (int i = 1; i <= Var.threads; i++)
			{
				new Thread(new ThreadStart(Var.firingMethod.Invoke)).Start();
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003941 File Offset: 0x00001B41
		public static MatchCollection MultiParse(string textToParseFrom, string textToMatchFrom)
		{
			textToMatchFrom = Regex.Escape(textToMatchFrom);
			textToMatchFrom = textToMatchFrom.Replace("\\(\\.\\*\\?\\)", "(.*?)");
			return Regex.Matches(textToParseFrom, textToMatchFrom);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003964 File Offset: 0x00001B64
		public static Match Parse(string textToParseFrom, string textToMatchFrom)
		{
			textToMatchFrom = Regex.Escape(textToMatchFrom);
			textToMatchFrom = textToMatchFrom.Replace("\\(\\.\\*\\?\\)", "(.*?)");
			return Regex.Match(textToParseFrom, textToMatchFrom);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003987 File Offset: 0x00001B87
		public static void Custom()
		{
			Interlocked.Increment(ref Var.cpm1);
			Interlocked.Increment(ref Var.checkedCnt);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000399F File Offset: 0x00001B9F
		public static void Valid()
		{
			Interlocked.Increment(ref Var.hitCnt);
			Interlocked.Increment(ref Var.cpm1);
			Interlocked.Increment(ref Var.checkedCnt);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000039C2 File Offset: 0x00001BC2
		public static void Invalid()
		{
			Interlocked.Increment(ref Var.cpm1);
			Interlocked.Increment(ref Var.invalidCnt);
			Interlocked.Increment(ref Var.checkedCnt);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000039E5 File Offset: 0x00001BE5
		public static void Retry()
		{
			Interlocked.Increment(ref Var.retryCnt);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000039F2 File Offset: 0x00001BF2
		public static void Error()
		{
			Interlocked.Increment(ref Var.errorCnt);
		}
	}
}
