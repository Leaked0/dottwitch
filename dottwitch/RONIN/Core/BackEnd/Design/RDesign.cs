using System;
using System.Drawing;
using Colorful;
using RONIN.Core.BackEnd.Variables;

namespace RONIN.Core.BackEnd.Design
{
	// Token: 0x02000021 RID: 33
	internal class RDesign
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x0000A2BC File Offset: 0x000084BC
		public static void ConsoleTitle(string toolName, string version, string creator)
		{
			Var.title = toolName;
			Var.currentVersion = version;
			Colorful.Console.Title = string.Concat(new string[]
			{
				toolName,
				" | ",
				version,
				" | ",
				creator
			});
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000A31C File Offset: 0x0000851C
		public static void FigletTitle(string title, Color color)
		{
			Colorful.Console.WriteLine();
			foreach (string text in title.Split(new string[]
			{
				Environment.NewLine
			}, StringSplitOptions.None))
			{
				Colorful.Console.SetCursorPosition((Colorful.Console.WindowWidth - text.Length) / 2, Colorful.Console.CursorTop);
				Colorful.Console.WriteLine(text, color);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003A0E File Offset: 0x00001C0E
		public static void CenterWrite(string text, Color color)
		{
			Colorful.Console.Write(new string(' ', (Colorful.Console.WindowWidth - text.Length) / 2));
			Colorful.Console.WriteLine(text, color);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000A378 File Offset: 0x00008578
		public static void ClearLastLine(int int_0)
		{
			for (int i = 0; i < int_0; i++)
			{
				Colorful.Console.SetCursorPosition(0, Colorful.Console.CursorTop - 1);
				Colorful.Console.Write(new string(' ', Colorful.Console.BufferWidth));
				Colorful.Console.SetCursorPosition(0, Colorful.Console.CursorTop - 1);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003A31 File Offset: 0x00001C31
		public static void TimeText(string text)
		{
			Colorful.Console.Write(string.Format("{0:HH:mm:ss}", DateTime.Now) ?? "", Var.color);
			Colorful.Console.WriteLine(" | " + text, Color.White);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003A6F File Offset: 0x00001C6F
		public static void TimeText(string text, Color color)
		{
			Colorful.Console.Write(string.Format("{0:HH:mm:ss}", DateTime.Now) ?? "", color);
			Colorful.Console.WriteLine(" | " + text, Color.White);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003AA9 File Offset: 0x00001CA9
		public static void Info(string text, Color color)
		{
			Colorful.Console.Write("[~] ", color);
			Colorful.Console.WriteLine(text, Color.White);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003AC1 File Offset: 0x00001CC1
		public static void Error(string text)
		{
			Colorful.Console.Write("        [-] ", Color.Red);
			Colorful.Console.WriteLine(text, Color.White);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003ADD File Offset: 0x00001CDD
		public static void Exclamation(string text)
		{
			Colorful.Console.Write("        [!] ", Color.Yellow);
			Colorful.Console.WriteLine(text, Color.White);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003AF9 File Offset: 0x00001CF9
		public static void Success(string message)
		{
			Colorful.Console.Write("        [+] ", Color.Green);
			Colorful.Console.WriteLine(message, Color.White);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003B15 File Offset: 0x00001D15
		public static void Option(string number, string text, Color color)
		{
			Colorful.Console.Write("[" + number + "] ", color);
			Colorful.Console.WriteLine(text, Color.White);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003B38 File Offset: 0x00001D38
		public static void Valid(string message)
		{
			Colorful.Console.Write("[Valid] ", Color.Green);
			Colorful.Console.WriteLine(message, Color.White);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003B54 File Offset: 0x00001D54
		public static void Write(string text, Color color)
		{
			Colorful.Console.Write(text, color);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003B5D File Offset: 0x00001D5D
		public static void WriteLine(string text, Color color)
		{
			Colorful.Console.WriteLine(text, color);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003B66 File Offset: 0x00001D66
		public static void UserInput(Color color)
		{
			Colorful.Console.Write(string.Format("{0:HH:mm:ss}", DateTime.Now), color);
			Colorful.Console.Write(" | ", Color.White);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003B91 File Offset: 0x00001D91
		public static void Field(string text, Color color)
		{
			Colorful.Console.Write(string.Format("{0:HH:mm:ss}", DateTime.Now) ?? "", color);
			Colorful.Console.Write(" | " + text + " ");
		}
	}
}
