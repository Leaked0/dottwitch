using System;
using System.Drawing;
using System.Threading;
using dotTwitch.Core.Modules;
using RONIN.Core.BackEnd.Auth;
using RONIN.Core.BackEnd.Design;
using RONIN.Core.BackEnd.Variables;

namespace dotTwitch
{
	// Token: 0x02000011 RID: 17
	internal class Program
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00006E88 File Offset: 0x00005088
		private static void Main(string[] args)
		{
			Var.color = Color.SkyBlue;
			Var.authUrl = "https://CRACKEDBYCRANKCRACK.SX";
			Var.encryptionKey = "CRANKISGAI";
			Var.firingMethod = Program.SelectMenu();
			MainFunction.SetDesign("dotTwitch", "1.0.0", "Crank [Crack.sx]", Var.color, Program.useProxy, "accounts");
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00006EE0 File Offset: 0x000050E0
		public static Var.FireDelegate SelectMenu()
		{
			for (;;)
			{
				try
				{
					Program.DisplayMenuOptions("Choose Mode", 200);
					Program.DisplayMenuOptions("[1] - Legacy Checker | Proxyless ( VPN Recommended )", 200);
					Program.DisplayMenuOptions("[2] - Brute Check | Proxies needed", 200);
					Program.DisplayMenuOptions("[3] - Capture | Proxyless ( VPN Recommended ) | Needs hits from Mode 2 to work", 200);
					Program.DisplayMenuOptions("[4] - Bits | Proxyless ( VPN Recommended )", 200);
					Program.DisplayMenuOptions("[5] - Follow | Proxyless ( VPN Recommended )", 200);
					Program.DisplayMenuOptions("[6] - Sub | Proxyless ( VPN Recommended )", 250);
					Console.WriteLine();
					RDesign.Field("Your chosen option is: ", Var.color);
					int int_ = Convert.ToInt32(Console.ReadLine());
					switch (int_)
					{
					case 1:
					{
						Var.FireDelegate fireDelegate = Program.OptionChosenQuery(new Var.FireDelegate(Legacy.DoWork), int_);
						if (fireDelegate != null)
						{
							return fireDelegate;
						}
						break;
					}
					case 2:
					{
						Var.FireDelegate fireDelegate2 = Program.OptionChosenQuery(new Var.FireDelegate(Brute.DoWork), int_);
						if (fireDelegate2 != null)
						{
							Program.useProxy = true;
							return fireDelegate2;
						}
						break;
					}
					case 3:
					{
						Var.FireDelegate fireDelegate3 = Program.OptionChosenQuery(new Var.FireDelegate(Checker.DoWork), int_);
						if (fireDelegate3 != null)
						{
							return fireDelegate3;
						}
						break;
					}
					case 4:
					{
						Bits.Set();
						Var.FireDelegate fireDelegate4 = Program.OptionChosenQuery(new Var.FireDelegate(Bits.DoWork), int_);
						if (fireDelegate4 != null)
						{
							return fireDelegate4;
						}
						break;
					}
					case 5:
					{
						Follow.Set();
						Var.FireDelegate fireDelegate5 = Program.OptionChosenQuery(new Var.FireDelegate(Follow.DoWork), int_);
						if (fireDelegate5 != null)
						{
							return fireDelegate5;
						}
						break;
					}
					case 6:
					{
						Sub.Set();
						Var.FireDelegate fireDelegate6 = Program.OptionChosenQuery(new Var.FireDelegate(Sub.DoWork), int_);
						if (fireDelegate6 != null)
						{
							return fireDelegate6;
						}
						break;
					}
					default:
						Console.WriteLine("Please choose a valid option.");
						Console.ReadKey();
						Console.Clear();
						break;
					}
					continue;
				}
				catch (Exception)
				{
					RDesign.Error("Please only use numbers to select an option.");
					Thread.Sleep(2000);
					Console.Clear();
					continue;
				}
				break;
			}
			Var.FireDelegate result;
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003711 File Offset: 0x00001911
		public static void DisplayMenuOptions(string text, int time)
		{
			RDesign.TimeText(text, Var.color);
			Thread.Sleep(time);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000070D8 File Offset: 0x000052D8
		public static Var.FireDelegate OptionChosenQuery(Var.FireDelegate method, int int_0)
		{
			Console.WriteLine();
			RDesign.TimeText("Confirmation needed. You have selected module: " + int_0 + ". To continue press Y else press any other key to return.");
			Var.FireDelegate result;
			if (Console.ReadKey().Key == ConsoleKey.Y)
			{
				Console.Clear();
				result = method;
			}
			else
			{
				Console.Clear();
				result = null;
			}
			return result;
		}

		// Token: 0x0400003B RID: 59
		private static bool useProxy;
	}
}
