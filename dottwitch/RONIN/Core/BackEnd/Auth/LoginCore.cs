// RONIN.Core.BackEnd.Auth.LoginCore
using NOSRequest;
using RONIN.Core.BackEnd.Auth;
using RONIN.Core.BackEnd.Design;
using RONIN.Core.BackEnd.Variables;
using System.Collections.Specialized;
using System.Drawing;
using System.Threading;

internal class LoginCore
{
	public static bool Login(string user, string pass, Color color)
	{
		NameValueCollection values = new NameValueCollection
		{
			["username"] = user,
			["password"] = pass,
			["hwid"] = HWID.Value()
		};
		NOSRequest.NOSRequest nOSRequest = new NOSRequest.NOSRequest(Var.encryptionKey);
		Response response = nOSRequest.Request(Var.authUrl + "/api/login_api.php", values);
		string message = response.message;
		if (message.Contains("Login Successful"))
		{
			RDesign.TimeText("You have logged in successfully! Welcome, " + user + ".", color);
			Thread.Sleep(2000);
			Var.loggedIn = true;
			return true;
		}
		if (message.Contains("Invalid HWID"))
		{
			RDesign.TimeText("Your HWID, " + HWID.Value() + ", is invalid!", color);
			Thread.Sleep(2000);
			return false;
		}
		if (message.Contains("Invalid Credentials"))
		{
			RDesign.TimeText("Your credentials are invalid!", color);
			Thread.Sleep(1000);
			return false;
		}
		if (message.Contains("Subscription expired on"))
		{
			RDesign.TimeText("Your license has expired!", color);
			Thread.Sleep(1000);
			return false;
		}
		RDesign.TimeText(message, color);
		Thread.Sleep(5000);
		return false;
	}
}
