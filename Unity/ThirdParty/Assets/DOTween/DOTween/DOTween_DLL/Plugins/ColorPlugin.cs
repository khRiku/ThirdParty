using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000022 RID: 34
	public class ColorPlugin : ABSTweenPlugin<Color, Color, ColorOptions>
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<Color, Color, ColorOptions> t)
		{
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009464 File Offset: 0x00007664
		public override void SetFrom(TweenerCore<Color, Color, ColorOptions> t, bool isRelative)
		{
			Color endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			Color pNewValue = t.endValue;
			if (!t.plugOptions.alphaOnly)
			{
				pNewValue = t.startValue;
			}
			else
			{
				pNewValue.a = t.startValue.a;
			}
			t.setter(pNewValue);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000817D File Offset: 0x0000637D
		public override Color ConvertToStartValue(TweenerCore<Color, Color, ColorOptions> t, Color value)
		{
			return value;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000094DC File Offset: 0x000076DC
		public override void SetRelativeEndValue(TweenerCore<Color, Color, ColorOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000094F5 File Offset: 0x000076F5
		public override void SetChangeValue(TweenerCore<Color, Color, ColorOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000081B2 File Offset: 0x000063B2
		public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color changeValue)
		{
			return 1f / unitsXSecond;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009510 File Offset: 0x00007710
		public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color> getter, DOSetter<Color> setter, float elapsed, Color startValue, Color changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			if (!options.alphaOnly)
			{
				startValue.r += changeValue.r * num;
				startValue.g += changeValue.g * num;
				startValue.b += changeValue.b * num;
				startValue.a += changeValue.a * num;
				setter(startValue);
				return;
			}
			Color pNewValue = getter();
			pNewValue.a = startValue.a + changeValue.a * num;
			setter(pNewValue);
		}
	}
}
