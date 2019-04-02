using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200001E RID: 30
	public class LongPlugin : ABSTweenPlugin<long, long, NoOptions>
	{
		// Token: 0x06000184 RID: 388 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<long, long, NoOptions> t)
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000854C File Offset: 0x0000674C
		public override void SetFrom(TweenerCore<long, long, NoOptions> t, bool isRelative)
		{
			long endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000817D File Offset: 0x0000637D
		public override long ConvertToStartValue(TweenerCore<long, long, NoOptions> t, long value)
		{
			return value;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008596 File Offset: 0x00006796
		public override void SetRelativeEndValue(TweenerCore<long, long, NoOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000085AB File Offset: 0x000067AB
		public override void SetChangeValue(TweenerCore<long, long, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000085C0 File Offset: 0x000067C0
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, long changeValue)
		{
			float num = (float)changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000085E0 File Offset: 0x000067E0
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<long> getter, DOSetter<long> setter, float elapsed, long startValue, long changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (long)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (long)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (long)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter((long)Math.Round((double)((float)startValue + (float)changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod))));
		}
	}
}
