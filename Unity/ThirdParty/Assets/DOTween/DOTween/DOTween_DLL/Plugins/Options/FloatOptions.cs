using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000036 RID: 54
	public struct FloatOptions : IPlugOptions
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000BD49 File Offset: 0x00009F49
		public void Reset()
		{
			this.snapping = false;
		}

		// Token: 0x040000E5 RID: 229
		public bool snapping;
	}
}
