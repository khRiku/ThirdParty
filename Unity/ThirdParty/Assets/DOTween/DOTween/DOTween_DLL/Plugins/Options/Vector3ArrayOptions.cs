using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000033 RID: 51
	public struct Vector3ArrayOptions : IPlugOptions
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000BD29 File Offset: 0x00009F29
		public void Reset()
		{
			this.axisConstraint = AxisConstraint.None;
			this.snapping = false;
			this.durations = null;
		}

		// Token: 0x040000E1 RID: 225
		public AxisConstraint axisConstraint;

		// Token: 0x040000E2 RID: 226
		public bool snapping;

		// Token: 0x040000E3 RID: 227
		internal float[] durations;
	}
}
