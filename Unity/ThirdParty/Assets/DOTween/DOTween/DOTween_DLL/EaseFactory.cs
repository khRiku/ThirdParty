using System;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x0200000B RID: 11
	public class EaseFactory
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00002E68 File Offset: 0x00001068
		public static EaseFunction StopMotion(int motionFps, Ease? ease = null)
		{
			EaseFunction customEase = EaseManager.ToEaseFunction((ease == null) ? DOTween.defaultEaseType : ease.Value);
			return EaseFactory.StopMotion(motionFps, customEase);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002E99 File Offset: 0x00001099
		public static EaseFunction StopMotion(int motionFps, AnimationCurve animCurve)
		{
			return EaseFactory.StopMotion(motionFps, new EaseFunction(new EaseCurve(animCurve).Evaluate));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002EB2 File Offset: 0x000010B2
		public static EaseFunction StopMotion(int motionFps, EaseFunction customEase)
		{
			float motionDelay = 1f / (float)motionFps;
			return delegate(float time, float duration, float overshootOrAmplitude, float period)
			{
				float time2 = (time < duration) ? (time - time % motionDelay) : time;
				return customEase(time2, duration, overshootOrAmplitude, period);
			};
		}
	}
}
