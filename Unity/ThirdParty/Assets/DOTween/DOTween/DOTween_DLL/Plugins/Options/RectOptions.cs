using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000037 RID: 55
	public struct RectOptions : IPlugOptions
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000BD52 File Offset: 0x00009F52
		public void Reset()
		{
			this.snapping = false;
		}

		// Token: 0x040000E6 RID: 230
		public bool snapping;
	}
}
