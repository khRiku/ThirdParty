using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000031 RID: 49
	public struct QuaternionOptions : IPlugOptions
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0000BD05 File Offset: 0x00009F05
		public void Reset()
		{
			this.rotateMode = RotateMode.Fast;
			this.axisConstraint = AxisConstraint.None;
			this.up = Vector3.zero;
		}

		// Token: 0x040000DD RID: 221
		public RotateMode rotateMode;

		// Token: 0x040000DE RID: 222
		public AxisConstraint axisConstraint;

		// Token: 0x040000DF RID: 223
		public Vector3 up;
	}
}
