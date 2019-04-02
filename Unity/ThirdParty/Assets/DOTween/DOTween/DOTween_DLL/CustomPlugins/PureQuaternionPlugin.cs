using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.CustomPlugins
{
	// Token: 0x02000045 RID: 69
	public class PureQuaternionPlugin : ABSTweenPlugin<Quaternion, Quaternion, NoOptions>
	{
		// Token: 0x06000240 RID: 576 RVA: 0x0000D105 File Offset: 0x0000B305
		public static PureQuaternionPlugin Plug()
		{
			if (PureQuaternionPlugin._plug == null)
			{
				PureQuaternionPlugin._plug = new PureQuaternionPlugin();
			}
			return PureQuaternionPlugin._plug;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<Quaternion, Quaternion, NoOptions> t)
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D120 File Offset: 0x0000B320
		public override void SetFrom(TweenerCore<Quaternion, Quaternion, NoOptions> t, bool isRelative)
		{
			Quaternion endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue * endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000817D File Offset: 0x0000637D
		public override Quaternion ConvertToStartValue(TweenerCore<Quaternion, Quaternion, NoOptions> t, Quaternion value)
		{
			return value;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000D16E File Offset: 0x0000B36E
		public override void SetRelativeEndValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
		{
			t.endValue *= t.startValue;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D188 File Offset: 0x0000B388
		public override void SetChangeValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
		{
			t.changeValue.x = t.endValue.x - t.startValue.x;
			t.changeValue.y = t.endValue.y - t.startValue.y;
			t.changeValue.z = t.endValue.z - t.startValue.z;
			t.changeValue.w = t.endValue.w - t.startValue.w;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000D220 File Offset: 0x0000B420
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Quaternion changeValue)
		{
			return changeValue.eulerAngles.magnitude / unitsXSecond;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000D240 File Offset: 0x0000B440
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Quaternion startValue, Quaternion changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			startValue.x += changeValue.x * num;
			startValue.y += changeValue.y * num;
			startValue.z += changeValue.z * num;
			startValue.w += changeValue.w * num;
			setter(startValue);
		}

		// Token: 0x04000117 RID: 279
		private static PureQuaternionPlugin _plug;
	}
}
