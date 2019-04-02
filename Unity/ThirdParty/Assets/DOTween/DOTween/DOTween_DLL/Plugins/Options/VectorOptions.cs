using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000039 RID: 57
	public struct VectorOptions : IPlugOptions
	{
		// Token: 0x0600020F RID: 527 RVA: 0x0000BD8E File Offset: 0x00009F8E
		public void Reset()
		{
			this.axisConstraint = AxisConstraint.None;
			this.snapping = false;
		}

		// Token: 0x040000EC RID: 236
		public AxisConstraint axisConstraint;

		// Token: 0x040000ED RID: 237
		public bool snapping;
	}
}
