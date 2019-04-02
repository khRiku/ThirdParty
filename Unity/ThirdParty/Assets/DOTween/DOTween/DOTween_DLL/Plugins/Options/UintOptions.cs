using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000032 RID: 50
	public struct UintOptions : IPlugOptions
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000BD20 File Offset: 0x00009F20
		public void Reset()
		{
			this.isNegativeChangeValue = false;
		}

		// Token: 0x040000E0 RID: 224
		public bool isNegativeChangeValue;
	}
}
