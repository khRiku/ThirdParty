using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins.Core
{
	// Token: 0x0200003E RID: 62
	public abstract class ABSTweenPlugin<T1, T2, TPlugOptions> : ITweenPlugin where TPlugOptions : struct, IPlugOptions
	{
		// Token: 0x06000218 RID: 536
		public abstract void Reset(TweenerCore<T1, T2, TPlugOptions> t);

		// Token: 0x06000219 RID: 537
		public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, bool isRelative);

		// Token: 0x0600021A RID: 538
		public abstract T2 ConvertToStartValue(TweenerCore<T1, T2, TPlugOptions> t, T1 value);

		// Token: 0x0600021B RID: 539
		public abstract void SetRelativeEndValue(TweenerCore<T1, T2, TPlugOptions> t);

		// Token: 0x0600021C RID: 540
		public abstract void SetChangeValue(TweenerCore<T1, T2, TPlugOptions> t);

		// Token: 0x0600021D RID: 541
		public abstract float GetSpeedBasedDuration(TPlugOptions options, float unitsXSecond, T2 changeValue);

		// Token: 0x0600021E RID: 542
		public abstract void EvaluateAndApply(TPlugOptions options, Tween t, bool isRelative, DOGetter<T1> getter, DOSetter<T1> setter, float elapsed, T2 startValue, T2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice);
	}
}
