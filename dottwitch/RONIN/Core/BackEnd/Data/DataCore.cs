using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using RONIN.Core.BackEnd.Design;
using RONIN.Core.BackEnd.Variables;

namespace RONIN.Core.BackEnd.Data
{
	// Token: 0x02000022 RID: 34
	internal class DataCore
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x0000A3BC File Offset: 0x000085BC
		public void loadDataList(Color color, string type)
		{
			int num = 0;
			if (!Directory.Exists("Progress"))
			{
				Directory.CreateDirectory("Progress");
			}
			for (;;)
			{
				RDesign.TimeText("Enter the name of your " + type + " base (.txt format, must be in the tool's file directory):", color);
				RDesign.UserInput(color);
				this.fileName = Console.ReadLine();
				if (File.Exists(this.fileName + ".txt"))
				{
					try
					{
						foreach (string path in Directory.GetFiles("Progress"))
						{
							if (Path.GetFileNameWithoutExtension(path).Equals(Path.GetFileName(this.fileName)))
							{
								RDesign.ClearLastLine(2);
								RDesign.TimeText("A previous session using the same filename has been found. Would you like to resume? (y/n):", color);
								RDesign.UserInput(color);
								if (Console.ReadLine() == "y")
								{
									num = int.Parse(File.ReadLines("Progress\\" + Path.GetFileName(path)).First<string>());
									Var.oldCheckedCnt = num;
								}
							}
						}
					}
					catch (Exception)
					{
						Console.WriteLine("The progressions file is corrupt. Proceeding to start from line 0.");
						Thread.Sleep(2000);
						RDesign.ClearLastLine(1);
					}
					try
					{
						using (StreamReader streamReader = new StreamReader(string.Format("{0}.txt", this.fileName)))
						{
							if (num != 0)
							{
								for (int j = 0; j < num; j++)
								{
									streamReader.ReadLine();
								}
							}
							while (streamReader.Peek() != -1)
							{
								this.Combo.Add(streamReader.ReadLine());
							}
						}
						break;
					}
					catch (Exception)
					{
						RDesign.ClearLastLine(2);
					}
				}
				else
				{
					RDesign.ClearLastLine(2);
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000A5B0 File Offset: 0x000087B0
		public void loadProxyConsole()
		{
			try
			{
				foreach (string item in File.ReadLines("proxies.txt"))
				{
					this.Proxies.Add(item);
				}
			}
			catch (Exception)
			{
				Console.WriteLine("proxies.txt was not found.");
				Console.WriteLine("The file has been automatically created. Please put your proxies in this file." + Environment.NewLine + "Press any key to exit...");
				File.CreateText("proxies.txt");
				Console.ReadKey();
				Process.GetCurrentProcess().Kill();
			}
		}

		// Token: 0x04000075 RID: 117
		public string fileName;

		// Token: 0x04000076 RID: 118
		public HashSet<string> Combo = new HashSet<string>();

		// Token: 0x04000077 RID: 119
		public List<string> Proxies = new List<string>();
	}
}
