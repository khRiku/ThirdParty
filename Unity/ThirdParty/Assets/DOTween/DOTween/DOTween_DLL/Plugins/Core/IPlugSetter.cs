using System;
using DG.Tweening.Core;

namespace DG.Tweening.Plugins.Core
{
	// Token: 0x0200003C RID: 60
	public interface IPlugSetter<T1, out T2, TPlugin, out TPlugOptions>
	{
		// Token: 0x06000214 RID: 532
		DOGetter<T1> Getter();

		// Token: 0x06000215 RID: 533
		DOSetter<T1> Setter();

		// Token: 0x06000216 RID: 534
		T2 EndValue();

		// Token: 0x06000217 RID: 535
		TPlugOptions GetOptions();
	}
}
