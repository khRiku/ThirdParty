using System;

namespace DG.DOTweenEditor
{
	// Token: 0x02000002 RID: 2
	internal class WpHandle
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public WpHandle(int wpIndex)
		{
			this.wpIndex = wpIndex;
		}

		// Token: 0x04000001 RID: 1
		internal int controlId;

		// Token: 0x04000002 RID: 2
		internal int wpIndex;
	}
}
