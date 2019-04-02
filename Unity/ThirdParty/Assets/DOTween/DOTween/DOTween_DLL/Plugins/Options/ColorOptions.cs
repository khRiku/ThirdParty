using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000035 RID: 53
	public struct ColorOptions : IPlugOptions
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0000BD40 File Offset: 0x00009F40
		public void Reset()
		{
			this.alphaOnly = false;
		}

		// Token: 0x040000E4 RID: 228
		public bool alphaOnly;
	}
}
