// RONIN.Kohl.Core.Security.Protection.Globals
using NOSRequest;
using RONIN.Core.BackEnd.Variables;
using RONIN.Kohl.Core.Security.Protection;
using RONIN.Kohl.Core.Security.Updater;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;

internal class Globals
{
	public static string[] urlArray = null;

	public static string urlString = null;

	public static string key = null;

	public static void Fill(string module)
	{
		key = Checksum.ARChecksum;
		NameValueCollection nameValueCollection = new NameValueCollection();
		nameValueCollection["key"] = key;
		nameValueCollection["config"] = module;
		NOSRequest.NOSRequest nOSRequest = new NOSRequest.NOSRequest(Var.encryptionKey);
		Response response = nOSRequest.Request(Var.authUrl + "/api/responder.php", nameValueCollection);
		string message = response.message;
		if (message.Contains("Invalid key"))
		{
			if (UpdateCheck.GetLastVersion() != Var.currentVersion)
			{
				UpdateCheck.Update();
				return;
			}
			Console.WriteLine("Modifications detected...");
			Thread.Sleep(5000);
			Process.GetCurrentProcess().Kill();
		}
		else if (message.Contains("|"))
		{
			urlArray = message.Split('|');
		}
		else
		{
			urlString = message;
		}
	}
}
