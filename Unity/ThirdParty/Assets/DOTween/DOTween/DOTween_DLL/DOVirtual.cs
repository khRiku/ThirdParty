using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000009 RID: 9
	public static class DOVirtual
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00002D70 File Offset: 0x00000F70
		public static Tweener Float(float from, float to, float duration, TweenCallback<float> onVirtualUpdate)
		{
			return DOTween.To(() => from, delegate(float x)
			{
				from = x;
			}, to, duration).OnUpdate(delegate
			{
				onVirtualUpdate(from);
			});
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002DC1 File Offset: 0x00000FC1
		public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType)
		{
			return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1f, DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float overshoot)
		{
			return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1f, overshoot, DOTween.defaultEasePeriod);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002DFC File Offset: 0x00000FFC
		public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float amplitude, float period)
		{
			return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1f, amplitude, period);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002E15 File Offset: 0x00001015
		public static float EasedValue(float from, float to, float lifetimePercentage, AnimationCurve easeCurve)
		{
			return from + (to - from) * EaseManager.Evaluate(Ease.INTERNAL_Custom, new EaseFunction(new EaseCurve(easeCurve).Evaluate), lifetimePercentage, 1f, DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002E45 File Offset: 0x00001045
		public static Tween DelayedCall(float delay, TweenCallback callback, bool ignoreTimeScale = true)
		{
			return DOTween.Sequence().AppendInterval(delay).OnStepComplete(callback).SetUpdate(UpdateType.Normal, ignoreTimeScale).SetAutoKill(true);
		}
	}
}
