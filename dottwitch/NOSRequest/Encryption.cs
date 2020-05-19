using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NOSRequest
{
	// Token: 0x0200000E RID: 14
	public class Encryption
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00006A18 File Offset: 0x00004C18
		public Encryption(string key)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(key);
			using (SHA256 sha = SHA256.Create())
			{
				this._key = sha.ComputeHash(bytes);
			}
			this._key = this._key.Take(32).ToArray<byte>();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000366C File Offset: 0x0000186C
		public void SetIV(byte[] byte_0)
		{
			this._iv = byte_0;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00006A8C File Offset: 0x00004C8C
		public string EncryptString(string plainText)
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Key = this._key;
			aes.IV = this._iv;
			MemoryStream memoryStream = new MemoryStream();
			ICryptoTransform transform = aes.CreateEncryptor();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			byte[] bytes = Encoding.ASCII.GetBytes(plainText);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] array = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(array, 0, array.Length);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00006B1C File Offset: 0x00004D1C
		public string DecryptString(string cipherText)
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Key = this._key;
			aes.IV = this._iv;
			MemoryStream memoryStream = new MemoryStream();
			ICryptoTransform transform = aes.CreateDecryptor();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			string result = string.Empty;
			try
			{
				byte[] array = Convert.FromBase64String(cipherText);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				byte[] array2 = memoryStream.ToArray();
				result = Encoding.ASCII.GetString(array2, 0, array2.Length);
			}
			finally
			{
				memoryStream.Close();
				cryptoStream.Close();
			}
			return result;
		}

		// Token: 0x04000030 RID: 48
		private byte[] _key;

		// Token: 0x04000031 RID: 49
		private byte[] _iv = new byte[16];
	}
}
