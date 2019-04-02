using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000023 RID: 35
	public class IntPlugin : ABSTweenPlugin<int, int, NoOptions>
	{
		// Token: 0x060001AE RID: 430 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<int, int, NoOptions> t)
		{
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000966C File Offset: 0x0000786C
		public override void SetFrom(TweenerCore<int, int, NoOptions> t, bool isRelative)
		{
			int endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000817D File Offset: 0x0000637D
		public override int ConvertToStartValue(TweenerCore<int, int, NoOptions> t, int value)
		{
			return value;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000096B6 File Offset: 0x000078B6
		public override void SetRelativeEndValue(TweenerCore<int, int, NoOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000096CB File Offset: 0x000078CB
		public override void SetChangeValue(TweenerCore<int, int, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000096E0 File Offset: 0x000078E0
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, int changeValue)
		{
			float num = (float)changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009700 File Offset: 0x00007900
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<int> getter, DOSetter<int> setter, float elapsed, int startValue, int changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * ((t.loopType == LoopType.Incremental) ? t.loops : 1) * (t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter((int)Math.Round((double)((float)startValue + (float)changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod))));
		}
	}
}
