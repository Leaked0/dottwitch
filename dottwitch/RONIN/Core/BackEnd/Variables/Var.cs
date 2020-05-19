using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace RONIN.Core.BackEnd.Variables
{
	// Token: 0x0200001C RID: 28
	internal class Var
	{
		// Token: 0x04000054 RID: 84
		public static string title = null;

		// Token: 0x04000055 RID: 85
		public static string currentVersion = null;

		// Token: 0x04000056 RID: 86
		public static int retryCnt;

		// Token: 0x04000057 RID: 87
		public static int checkedCnt;

		// Token: 0x04000058 RID: 88
		public static int oldCheckedCnt;

		// Token: 0x04000059 RID: 89
		public static int hitCnt;

		// Token: 0x0400005A RID: 90
		public static int invalidCnt;

		// Token: 0x0400005B RID: 91
		public static int cpm1;

		// Token: 0x0400005C RID: 92
		public static int cpm2;

		// Token: 0x0400005D RID: 93
		public static int threads;

		// Token: 0x0400005E RID: 94
		public static int errorCnt = 0;

		// Token: 0x0400005F RID: 95
		public static string fileName = null;

		// Token: 0x04000060 RID: 96
		public static string dateNow = null;

		// Token: 0x04000061 RID: 97
		public static bool loggedIn = false;

		// Token: 0x04000062 RID: 98
		public static string folder = null;

		// Token: 0x04000063 RID: 99
		public static string authUrl = null;

		// Token: 0x04000064 RID: 100
		public static string encryptionKey = null;

		// Token: 0x04000065 RID: 101
		public static string proxyUrl = null;

		// Token: 0x04000066 RID: 102
		public static Random rnd = new Random();

		// Token: 0x04000067 RID: 103
		public static ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();

		// Token: 0x04000068 RID: 104
		public static BlockingCollection<string> comboQueue = new BlockingCollection<string>();

		// Token: 0x04000069 RID: 105
		public static int proxyType = 0;

		// Token: 0x0400006A RID: 106
		public static List<string> proxies = new List<string>();

		// Token: 0x0400006B RID: 107
		public static Color color;

		// Token: 0x0400006C RID: 108
		public static Var.FireDelegate firingMethod;

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x0600008B RID: 139
		public delegate void FireDelegate();
	}
}
