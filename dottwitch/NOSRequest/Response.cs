using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NOSRequest
{
	// Token: 0x0200000F RID: 15
	public class Response
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00003675 File Offset: 0x00001875
		public Response(Encryption encryption)
		{
			this._encryption = encryption;
			this.authenticationKey = Convert.ToString(new Random().Next(10000, 100000));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00006BC8 File Offset: 0x00004DC8
		public T GetData<T>(string key)
		{
			return (T)((object)Convert.ChangeType(this.data[key], typeof(T)));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000036A3 File Offset: 0x000018A3
		public void AddData(string key, object value)
		{
			this.data.Add(key, value);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000036B2 File Offset: 0x000018B2
		public bool DataExists(string key)
		{
			return this.data.ContainsKey(key);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00006BF8 File Offset: 0x00004DF8
		public string EncryptedAuth()
		{
			return this._encryption.EncryptString(this.authenticationKey);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00006C18 File Offset: 0x00004E18
		public void Initialize(string _raw)
		{
			this.raw = _raw;
			this.data = JsonConvert.DeserializeObject<Dictionary<string, object>>(this.raw);
			if (!this.DataExists("authentication_key") || this.GetData<string>("authentication_key") != this.authenticationKey)
			{
				throw new Exception("Response authentication failed.");
			}
			if (!this.DataExists("status") || !this.DataExists("message"))
			{
				throw new Exception("Response is missing required data.");
			}
			this.status = this.GetData<bool>("status");
			this.message = this.GetData<string>("message").Replace("\\n", Environment.NewLine);
		}

		// Token: 0x04000032 RID: 50
		public bool status;

		// Token: 0x04000033 RID: 51
		public string message;

		// Token: 0x04000034 RID: 52
		public string raw;

		// Token: 0x04000035 RID: 53
		private Dictionary<string, object> data;

		// Token: 0x04000036 RID: 54
		private Encryption _encryption;

		// Token: 0x04000037 RID: 55
		private string authenticationKey;
	}
}
