using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace RONIN.Kohl.Core.Security.Protection
{
	// Token: 0x02000019 RID: 25
	internal class Checksum
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00008960 File Offset: 0x00006B60
		public static object GetFileChecksum()
		{
			return RuntimeHelpers.GetObjectValue(Checksum.hash_generator(Application.ExecutablePath));
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00008980 File Offset: 0x00006B80
		private static object hash_generator(string fileName)
		{
			object objectValue;
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				fileStream.Position = 0L;
				object obj = NewLateBinding.LateGet(SHA256.Create(), null, "ComputeHash", new object[]
				{
					fileStream
				}, null, null, new bool[]
				{
					true
				});
				byte[] array = (byte[])obj;
				objectValue = RuntimeHelpers.GetObjectValue(Checksum.PrintByteArray(array));
			}
			return objectValue;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00008A14 File Offset: 0x00006C14
		private static object PrintByteArray(byte[] array)
		{
			string text = "";
			checked
			{
				int num = array.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					text += array[i].ToString("X2");
				}
				return text.ToUpper();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00008A70 File Offset: 0x00006C70
		public static string MD5Hash(string string_0)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (byte b in md.ComputeHash(Encoding.UTF8.GetBytes(string_0)))
				{
					stringBuilder.Append(b.ToString("x2").ToLower());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x0400004C RID: 76
		public static string ARChecksum = Checksum.GetFileChecksum().ToString();
	}
}
