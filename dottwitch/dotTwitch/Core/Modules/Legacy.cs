using System;
using Leaf.xNet;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch.Core.Modules
{
	// Token: 0x02000016 RID: 22
	internal class Legacy
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00008220 File Offset: 0x00006420
		public static void DoWork()
		{
			for (;;)
			{
				SlavCore.UpdateTitle(null, 0, null, 0);
				if (Var.comboQueue.Count != 0)
				{
					try
					{
						string text = Var.comboQueue.Take();
						for (;;)
						{
							try
							{
								if (text.Contains(":") || text.Contains(";") || text.Contains("|"))
								{
									string[] array = text.Split(new char[]
									{
										':',
										';',
										'|'
									});
									string text2 = array[0];
									string str = array[1];
									string text3 = text2;
									if (text2.Contains("@"))
									{
										text3 = text2.Split(new char[]
										{
											'@'
										})[0];
									}
									HttpRequest httpRequest = new HttpRequest();
									try
									{
										try
										{
											httpRequest.IgnoreProtocolErrors = true;
											httpRequest.KeepAlive = true;
											if (Var.proxies.Count != 0)
											{
												httpRequest.Proxy = SlavCore.LoadProxyType();
											}
											httpRequest.AddHeader("Client-ID", "jzkbprff40iqj646a697cyrvl0zt2m6");
											string text4 = httpRequest.Get("https://api.twitch.tv/kraken/channels/" + text3, null).ToString();
											if (text4.Contains("mature"))
											{
												SlavCore.Valid();
												SlavCore.SaveData(text3 + ":" + str, "dotTwitch_Legacy_Hits", null, true);
											}
											else
											{
												if (!text4.Contains("Unable to find channel for login") && !text4.Contains("is in an invalid format") && !text4.Contains("{\"error\":\"Unprocessable Entity\""))
												{
													SlavCore.Retry();
													continue;
												}
												SlavCore.Invalid();
											}
										}
										catch (HttpException)
										{
											SlavCore.Retry();
											continue;
										}
										catch (NullReferenceException)
										{
											SlavCore.Retry();
											continue;
										}
										catch (Exception ex)
										{
											Console.WriteLine(ex.ToString());
											SlavCore.Error();
											continue;
										}
										break;
									}
									finally
									{
										httpRequest.Dispose();
									}
								}
								SlavCore.Invalid();
							}
							catch (Exception ex2)
							{
								Console.WriteLine(ex2.ToString());
								SlavCore.Error();
								continue;
							}
							break;
						}
						continue;
					}
					catch (Exception ex3)
					{
						Console.WriteLine(ex3.ToString());
						continue;
					}
					break;
				}
				break;
			}
			Var.threads--;
		}
	}
}
