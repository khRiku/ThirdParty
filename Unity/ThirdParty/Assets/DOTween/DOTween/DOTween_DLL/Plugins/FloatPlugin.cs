using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200002C RID: 44
	public class FloatPlugin : ABSTweenPlugin<float, float, FloatOptions>
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<float, float, FloatOptions> t)
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B6B0 File Offset: 0x000098B0
		public override void SetFrom(TweenerCore<float, float, FloatOptions> t, bool isRelative)
		{
			float endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter((!t.plugOptions.snapping) ? t.startValue : ((float)Math.Round((double)t.startValue)));
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000817D File Offset: 0x0000637D
		public override float ConvertToStartValue(TweenerCore<float, float, FloatOptions> t, float value)
		{
			return value;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B716 File Offset: 0x00009916
		public override void SetRelativeEndValue(TweenerCore<float, float, FloatOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B72B File Offset: 0x0000992B
		public override void SetChangeValue(TweenerCore<float, float, FloatOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B740 File Offset: 0x00009940
		public override float GetSpeedBasedDuration(FloatOptions options, float unitsXSecond, float changeValue)
		{
			float num = changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B760 File Offset: 0x00009960
		public override void EvaluateAndApply(FloatOptions options, Tween t, bool isRelative, DOGetter<float> getter, DOSetter<float> setter, float elapsed, float startValue, float changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter((!options.snapping) ? (startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)) : ((float)Math.Round((double)(startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)))));
		}
	}
}
