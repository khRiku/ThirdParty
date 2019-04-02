using System;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000009 RID: 9
	public struct SpiralOptions : IPlugOptions
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002B5C File Offset: 0x00000D5C
		public void Reset()
		{
			this.depth = (this.frequency = (this.speed = 0f));
			this.mode = SpiralMode.Expand;
			this.snapping = false;
		}

		// Token: 0x0400003C RID: 60
		public float depth;

		// Token: 0x0400003D RID: 61
		public float frequency;

		// Token: 0x0400003E RID: 62
		public float speed;

		// Token: 0x0400003F RID: 63
		public SpiralMode mode;

		// Token: 0x04000040 RID: 64
		public bool snapping;

		// Token: 0x04000041 RID: 65
		internal float unit;

		// Token: 0x04000042 RID: 66
		internal Quaternion axisQ;
	}
}
