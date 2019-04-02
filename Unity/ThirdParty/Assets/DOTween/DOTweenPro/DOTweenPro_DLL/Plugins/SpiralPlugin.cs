using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200000A RID: 10
	public class SpiralPlugin : ABSTweenPlugin<Vector3, Vector3, SpiralOptions>
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002B94 File Offset: 0x00000D94
		public override void Reset(TweenerCore<Vector3, Vector3, SpiralOptions> t)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B94 File Offset: 0x00000D94
		public override void SetFrom(TweenerCore<Vector3, Vector3, SpiralOptions> t, bool isRelative)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B96 File Offset: 0x00000D96
		public static ABSTweenPlugin<Vector3, Vector3, SpiralOptions> Get()
		{
			return PluginsManager.GetCustomPlugin<SpiralPlugin, Vector3, Vector3, SpiralOptions>();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B9D File Offset: 0x00000D9D
		public override Vector3 ConvertToStartValue(TweenerCore<Vector3, Vector3, SpiralOptions> t, Vector3 value)
		{
			return value;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B94 File Offset: 0x00000D94
		public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3, SpiralOptions> t)
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002BA0 File Offset: 0x00000DA0
		public override void SetChangeValue(TweenerCore<Vector3, Vector3, SpiralOptions> t)
		{
			t.plugOptions.speed = t.plugOptions.speed * (10f / t.plugOptions.frequency);
			t.plugOptions.axisQ = Quaternion.LookRotation(t.endValue, Vector3.up);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B9D File Offset: 0x00000D9D
		public override float GetSpeedBasedDuration(SpiralOptions options, float unitsXSecond, Vector3 changeValue)
		{
			return unitsXSecond;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public override void EvaluateAndApply(SpiralOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			float num = EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			float num2 = (options.mode == SpiralMode.ExpandThenContract && num > 0.5f) ? (0.5f - (num - 0.5f)) : num;
			if (t.loopType == LoopType.Incremental)
			{
				num += (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			float num3 = duration * options.speed * num;
			options.unit = duration * options.speed * num2;
			Vector3 vector = new Vector3(options.unit * Mathf.Cos(num3 * options.frequency), options.unit * Mathf.Sin(num3 * options.frequency), options.depth * num);
			vector = options.axisQ * vector + startValue;
			if (options.snapping)
			{
				vector.x = (float)Math.Round((double)vector.x);
				vector.y = (float)Math.Round((double)vector.y);
				vector.z = (float)Math.Round((double)vector.z);
			}
			setter(vector);
		}

		// Token: 0x04000043 RID: 67
		public static readonly Vector3 DefaultDirection = Vector3.forward;
	}
}
