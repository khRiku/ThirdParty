using System;
using UnityEngine;

namespace DG.Tweening.Core.Easing
{
	// Token: 0x0200005A RID: 90
	public class EaseCurve
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x000104B5 File Offset: 0x0000E6B5
		public EaseCurve(AnimationCurve animCurve)
		{
			this._animCurve = animCurve;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000104C4 File Offset: 0x0000E6C4
		public float Evaluate(float time, float duration, float unusedOvershoot, float unusedPeriod)
		{
			float time2 = this._animCurve[this._animCurve.length - 1].time;
			float num = time / duration;
			return this._animCurve.Evaluate(num * time2);
		}

		// Token: 0x0400018F RID: 399
		private readonly AnimationCurve _animCurve;
	}
}
