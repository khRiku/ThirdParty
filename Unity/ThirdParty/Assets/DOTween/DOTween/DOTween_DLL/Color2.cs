using System;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000004 RID: 4
	public struct Color2
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Color2(Color ca, Color cb)
		{
			this.ca = ca;
			this.cb = cb;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public static Color2 operator +(Color2 c1, Color2 c2)
		{
			return new Color2(c1.ca + c2.ca, c1.cb + c2.cb);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002089 File Offset: 0x00000289
		public static Color2 operator -(Color2 c1, Color2 c2)
		{
			return new Color2(c1.ca - c2.ca, c1.cb - c2.cb);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020B2 File Offset: 0x000002B2
		public static Color2 operator *(Color2 c1, float f)
		{
			return new Color2(c1.ca * f, c1.cb * f);
		}

		// Token: 0x0400000C RID: 12
		public Color ca;

		// Token: 0x0400000D RID: 13
		public Color cb;
	}
}
