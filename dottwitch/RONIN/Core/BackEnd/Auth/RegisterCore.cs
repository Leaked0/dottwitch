using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using RONIN.Core.BackEnd.Design;
using RONIN.Core.BackEnd.Variables;

namespace RONIN.Core.BackEnd.Auth
{
	// Token: 0x02000028 RID: 40
	internal class RegisterCore
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x0000AE50 File Offset: 0x00009050
		public static bool Register(string user, string pass, string key, Color color)
		{
			WebRequest webRequest = WebRequest.Create(Var.authUrl + "/api/register_api.php");
			webRequest.Method = "POST";
			string s = string.Concat(new string[]
			{
				"username=",
				user,
				"&password=",
				pass,
				"&hwid=",
				HWID.Value(),
				"&license=",
				key
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = (long)bytes.Length;
			Stream stream = webRequest.GetRequestStream();
			stream.Write(bytes, 0, bytes.Length);
			stream.Close();
			WebResponse response = webRequest.GetResponse();
			stream = response.GetResponseStream();
			StreamReader streamReader = new StreamReader(stream);
			string text = streamReader.ReadToEnd();
			bool result;
			if (text.Contains("successfully"))
			{
				RDesign.TimeText("You have registered successfully! Welcome, " + user + ".", color);
				Thread.Sleep(2000);
				result = true;
			}
			else if (text.Contains("already been used"))
			{
				RDesign.TimeText("The license, " + key + ", has already been used.", color);
				Thread.Sleep(1000);
				result = false;
			}
			else if (text.Contains("does not exist"))
			{
				RDesign.TimeText("The license, " + key + ", does not exist.", color);
				Thread.Sleep(1000);
				result = false;
			}
			else
			{
				streamReader.Close();
				stream.Close();
				response.Close();
				RDesign.TimeText(text, color);
				Thread.Sleep(5000);
				result = false;
			}
			return result;
		}
	}
}
