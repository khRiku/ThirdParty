using System;

namespace DG.Tweening.Core.Easing
{
	// Token: 0x02000058 RID: 88
	public static class Bounce
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000F730 File Offset: 0x0000D930
		public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return 1f - Bounce.EaseOut(duration - time, duration, -1f, -1f);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000F74C File Offset: 0x0000D94C
		public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration) < 0.363636374f)
			{
				return 7.5625f * time * time;
			}
			if (time < 0.727272749f)
			{
				return 7.5625f * (time -= 0.545454562f) * time + 0.75f;
			}
			if (time < 0.909090936f)
			{
				return 7.5625f * (time -= 0.8181818f) * time + 0.9375f;
			}
			return 7.5625f * (time -= 0.954545438f) * time + 0.984375f;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time < duration * 0.5f)
			{
				return Bounce.EaseIn(time * 2f, duration, -1f, -1f) * 0.5f;
			}
			return Bounce.EaseOut(time * 2f - duration, duration, -1f, -1f) * 0.5f + 0.5f;
		}
	}
}
