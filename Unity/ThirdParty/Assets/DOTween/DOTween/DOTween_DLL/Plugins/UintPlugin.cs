using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000027 RID: 39
	public class UintPlugin : ABSTweenPlugin<uint, uint, UintOptions>
	{
		// Token: 0x060001CF RID: 463 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<uint, uint, UintOptions> t)
		{
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A54C File Offset: 0x0000874C
		public override void SetFrom(TweenerCore<uint, uint, UintOptions> t, bool isRelative)
		{
			uint endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000817D File Offset: 0x0000637D
		public override uint ConvertToStartValue(TweenerCore<uint, uint, UintOptions> t, uint value)
		{
			return value;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000A596 File Offset: 0x00008796
		public override void SetRelativeEndValue(TweenerCore<uint, uint, UintOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A5AC File Offset: 0x000087AC
		public override void SetChangeValue(TweenerCore<uint, uint, UintOptions> t)
		{
			t.plugOptions.isNegativeChangeValue = (t.endValue < t.startValue);
			t.changeValue = (t.plugOptions.isNegativeChangeValue ? (t.startValue - t.endValue) : (t.endValue - t.startValue));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000A604 File Offset: 0x00008804
		public override float GetSpeedBasedDuration(UintOptions options, float unitsXSecond, uint changeValue)
		{
			float num = changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A624 File Offset: 0x00008824
		public override void EvaluateAndApply(UintOptions options, Tween t, bool isRelative, DOGetter<uint> getter, DOSetter<uint> setter, float elapsed, uint startValue, uint changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			uint num;
			if (t.loopType == LoopType.Incremental)
			{
				num = (uint)((ulong)changeValue * (ulong)((long)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops)));
				if (options.isNegativeChangeValue)
				{
					startValue -= num;
				}
				else
				{
					startValue += num;
				}
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				num = (uint)((ulong)changeValue * (ulong)((long)((t.loopType == LoopType.Incremental) ? t.loops : 1)) * (ulong)((long)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops)));
				if (options.isNegativeChangeValue)
				{
					startValue -= num;
				}
				else
				{
					startValue += num;
				}
			}
			num = (uint)Math.Round((double)(changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
			if (options.isNegativeChangeValue)
			{
				setter(startValue - num);
				return;
			}
			setter(startValue + num);
		}
	}
}
