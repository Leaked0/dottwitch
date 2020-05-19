using System;
using System.Drawing;
using Colorful;
using Figgle;
using RONIN.Core.BackEnd.Design;
using RONIN.Core.BackEnd.SlavLib;
using RONIN.Core.BackEnd.Variables;

namespace RONIN.Core.BackEnd.Auth
{
	// Token: 0x02000027 RID: 39
	internal class MainFunction
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000AD28 File Offset: 0x00008F28
		public static void SetDesign(string title, string version, string author, Color color, bool useProxies = true, string dataType = "accounts")
		{
			RDesign.ConsoleTitle(title, version, author);
			RDesign.FigletTitle(FiggleFonts.Univers.Render(title, null), Var.color);
			if (Authentication.Auth(color))
			{
				int int_ = 0;
				if (useProxies)
				{
					RDesign.TimeText("Would you like to add an auto-updating proxy url? (y/n):", color);
					RDesign.UserInput(color);
					string a = Colorful.Console.ReadLine();
					RDesign.ClearLastLine(2);
					if (a == "y")
					{
						MainFunction.autoUpdate = true;
						do
						{
							RDesign.TimeText("Enter your proxy URL:", color);
							RDesign.UserInput(color);
							Var.proxyUrl = Colorful.Console.ReadLine();
							RDesign.ClearLastLine(2);
						}
						while (!Uri.IsWellFormedUriString(Var.proxyUrl, UriKind.Absolute));
						for (;;)
						{
							try
							{
								RDesign.TimeText("Interval between grabbing proxies from URL (minutes):", color);
								RDesign.UserInput(color);
								int_ = Convert.ToInt32(Colorful.Console.ReadLine());
								RDesign.ClearLastLine(2);
								break;
							}
							catch (FormatException)
							{
								RDesign.ClearLastLine(2);
							}
						}
					}
				}
				SlavCore.Checker(color, dataType, useProxies);
				SlavCore.StartThreading();
				if (MainFunction.autoUpdate)
				{
					SlavCore.StartTimer2(int_);
				}
				SlavCore.StartTimer();
				SlavCore.CheckProgress();
			}
		}

		// Token: 0x0400007B RID: 123
		private static bool autoUpdate;
	}
}
