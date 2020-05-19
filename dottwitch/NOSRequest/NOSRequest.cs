using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace NOSRequest
{
	// Token: 0x02000010 RID: 16
	public class NOSRequest
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000036C0 File Offset: 0x000018C0
		public NOSRequest(string key, byte[] byte_0 = null)
		{
			this.encryption = new Encryption(key);
			this.SetIV(byte_0);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000036F1 File Offset: 0x000018F1
		private void SetIV(byte[] byte_0)
		{
			if (byte_0 != null && byte_0.Count<byte>() == 16)
			{
				this.encryption.SetIV(byte_0);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00006CD4 File Offset: 0x00004ED4
		public string EncryptString(string plainText)
		{
			return this.encryption.EncryptString(plainText);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00006CF0 File Offset: 0x00004EF0
		public string DecryptString(string cipherText)
		{
			return this.encryption.DecryptString(cipherText);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00006D0C File Offset: 0x00004F0C
		public Response Request(string url, NameValueCollection values = null)
		{
			Response response = new Response(this.encryption);
			try
			{
				WebClient client = this.GetClient();
				if (values == null)
				{
					values = new NameValueCollection();
				}
				Dictionary<string, object> dictionary = NOSRequest.GetDictionary(values);
				if (dictionary.ContainsKey("authentication_key"))
				{
					throw new Exception("Key \"authentication_key\" must not be defined.");
				}
				dictionary.Add("authentication_key", response.EncryptedAuth());
				string plainText = JsonConvert.SerializeObject(dictionary);
				string data = this.encryption.EncryptString(plainText);
				string cipherText = client.UploadString(url, data);
				string raw = this.encryption.DecryptString(cipherText);
				response.Initialize(raw);
			}
			catch (Exception ex)
			{
				response.status = false;
				response.message = ex.Message;
			}
			return response;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00006DD4 File Offset: 0x00004FD4
		public WebClient GetClient()
		{
			WebClient webClient = new WebClient();
			if (!string.IsNullOrEmpty(this.UserAgent))
			{
				webClient.Headers.Add(HttpRequestHeader.UserAgent, this.UserAgent);
			}
			if (this.NullifyProxy)
			{
				webClient.Proxy = null;
			}
			return webClient;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00006E1C File Offset: 0x0000501C
		private static Dictionary<string, object> GetDictionary(NameValueCollection values)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (object obj in values.Keys)
			{
				string text = (string)obj;
				dictionary.Add(text, values[text]);
			}
			return dictionary;
		}

		// Token: 0x04000038 RID: 56
		public string UserAgent = "SafeRequest";

		// Token: 0x04000039 RID: 57
		public bool NullifyProxy = true;

		// Token: 0x0400003A RID: 58
		private Encryption encryption;
	}
}
