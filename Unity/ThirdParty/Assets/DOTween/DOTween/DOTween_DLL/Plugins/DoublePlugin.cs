using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200001D RID: 29
	public class DoublePlugin : ABSTweenPlugin<double, double, NoOptions>
	{
		// Token: 0x0600017C RID: 380 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<double, double, NoOptions> t)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000083F0 File Offset: 0x000065F0
		public override void SetFrom(TweenerCore<double, double, NoOptions> t, bool isRelative)
		{
			double endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000817D File Offset: 0x0000637D
		public override double ConvertToStartValue(TweenerCore<double, double, NoOptions> t, double value)
		{
			return value;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000843A File Offset: 0x0000663A
		public override void SetRelativeEndValue(TweenerCore<double, double, NoOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000844F File Offset: 0x0000664F
		public override void SetChangeValue(TweenerCore<double, double, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008464 File Offset: 0x00006664
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, double changeValue)
		{
			float num = (float)changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008484 File Offset: 0x00006684
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<double> getter, DOSetter<double> setter, float elapsed, double startValue, double changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (double)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (double)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (double)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter(startValue + changeValue * (double)EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
		}
	}
}
