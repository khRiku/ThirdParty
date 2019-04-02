using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public struct ControlPoint
	{
		// Token: 0x06000223 RID: 547 RVA: 0x0000C31C File Offset: 0x0000A51C
		public ControlPoint(Vector3 a, Vector3 b)
		{
			this.a = a;
			this.b = b;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C32C File Offset: 0x0000A52C
		public static ControlPoint operator +(ControlPoint cp, Vector3 v)
		{
			return new ControlPoint(cp.a + v, cp.b + v);
		}

		// Token: 0x04000100 RID: 256
		public Vector3 a;

		// Token: 0x04000101 RID: 257
		public Vector3 b;
	}
}
