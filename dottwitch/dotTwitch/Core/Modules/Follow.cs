using System;
using System.Text.RegularExpressions;
using System.Threading;
using Leaf.xNet;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch.Core.Modules
{
	// Token: 0x02000015 RID: 21
	internal class Follow
	{
		// Token: 0x06000062 RID: 98 RVA: 0x0000374E File Offset: 0x0000194E
		public static void Set()
		{
			Console.WriteLine("[What's the name of the channel you'd like to send followers to?]");
			Follow.channel = Console.ReadLine();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00007F70 File Offset: 0x00006170
		public static void DoWork()
		{
			for (;;)
			{
				SlavCore.UpdateTitle("Followers Sent", Follow.bTotal, null, 0);
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
									string text4 = array[2];
									if (text2.Contains("@"))
									{
										string text5 = text2.Split(new char[]
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
											string value = Regex.Match(httpRequest.Get(string.Concat(new string[]
											{
												"https://api.twitch.tv/api/channels/",
												Follow.channel,
												"/access_token?need_https=true&oauth_token=",
												text4,
												"&platform=web&player_backend=mediaplayer&player_type=site"
											}), null).ToString(), "channel_id\\\\\":(.*?),\\\\\"chansub").Groups[1].Value;
											httpRequest.AddHeader("Authorization", "OAuth " + text4);
											string str = "[{\"operationName\":\"FollowButton_FollowUser\",\"variables\":{\"input\":{\"disableNotifications\":false,\"targetID\":\"" + value + "\"}},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"51956f0c469f54e60211ea4e6a34b597d45c1c37b9664d4b62096a1ac03be9e6\"}}}]";
											if (httpRequest.Post("https://gql.twitch.tv/gql", str, "text/plain;charset=UTF-8").ToString().Contains("token is invalid."))
											{
												SlavCore.Invalid();
											}
											else
											{
												SlavCore.Custom();
												Interlocked.Increment(ref Follow.bTotal);
												SlavCore.SaveData(text, "Follow_Accounts", null, true);
												Console.WriteLine(text + " has followed");
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

		// Token: 0x04000046 RID: 70
		public static int bTotal = 0;

		// Token: 0x04000047 RID: 71
		public static string channel = string.Empty;
	}
}
