using System;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000015 RID: 21
	public class TweenParams
	{
		// Token: 0x060000FB RID: 251 RVA: 0x0000619F File Offset: 0x0000439F
		public TweenParams()
		{
			this.Clear();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000061B0 File Offset: 0x000043B0
		public TweenParams Clear()
		{
			this.id = (this.target = null);
			this.updateType = DOTween.defaultUpdateType;
			this.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
			this.onStart = (this.onPlay = (this.onRewind = (this.onUpdate = (this.onStepComplete = (this.onComplete = (this.onKill = null))))));
			this.onWaypointChange = null;
			this.isRecyclable = DOTween.defaultRecyclable;
			this.isSpeedBased = false;
			this.autoKill = DOTween.defaultAutoKill;
			this.loops = 1;
			this.loopType = DOTween.defaultLoopType;
			this.delay = 0f;
			this.isRelative = false;
			this.easeType = Ease.Unset;
			this.customEase = null;
			this.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			this.easePeriod = DOTween.defaultEasePeriod;
			return this;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000628D File Offset: 0x0000448D
		public TweenParams SetAutoKill(bool autoKillOnCompletion = true)
		{
			this.autoKill = autoKillOnCompletion;
			return this;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006297 File Offset: 0x00004497
		public TweenParams SetId(object id)
		{
			this.id = id;
			return this;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000062A1 File Offset: 0x000044A1
		public TweenParams SetTarget(object target)
		{
			this.target = target;
			return this;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000062AB File Offset: 0x000044AB
		public TweenParams SetLoops(int loops, LoopType? loopType = null)
		{
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			this.loops = loops;
			if (loopType != null)
			{
				this.loopType = loopType.Value;
			}
			return this;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000062DC File Offset: 0x000044DC
		public TweenParams SetEase(Ease ease, float? overshootOrAmplitude = null, float? period = null)
		{
			this.easeType = ease;
			this.easeOvershootOrAmplitude = ((overshootOrAmplitude != null) ? overshootOrAmplitude.Value : DOTween.defaultEaseOvershootOrAmplitude);
			this.easePeriod = ((period != null) ? period.Value : DOTween.defaultEasePeriod);
			this.customEase = null;
			return this;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006334 File Offset: 0x00004534
		public TweenParams SetEase(AnimationCurve animCurve)
		{
			this.easeType = Ease.INTERNAL_Custom;
			this.customEase = new EaseFunction(new EaseCurve(animCurve).Evaluate);
			return this;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006356 File Offset: 0x00004556
		public TweenParams SetEase(EaseFunction customEase)
		{
			this.easeType = Ease.INTERNAL_Custom;
			this.customEase = customEase;
			return this;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006368 File Offset: 0x00004568
		public TweenParams SetRecyclable(bool recyclable = true)
		{
			this.isRecyclable = recyclable;
			return this;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006372 File Offset: 0x00004572
		public TweenParams SetUpdate(bool isIndependentUpdate)
		{
			this.updateType = DOTween.defaultUpdateType;
			this.isIndependentUpdate = isIndependentUpdate;
			return this;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006387 File Offset: 0x00004587
		public TweenParams SetUpdate(UpdateType updateType, bool isIndependentUpdate = false)
		{
			this.updateType = updateType;
			this.isIndependentUpdate = isIndependentUpdate;
			return this;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006398 File Offset: 0x00004598
		public TweenParams OnStart(TweenCallback action)
		{
			this.onStart = action;
			return this;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000063A2 File Offset: 0x000045A2
		public TweenParams OnPlay(TweenCallback action)
		{
			this.onPlay = action;
			return this;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000063AC File Offset: 0x000045AC
		public TweenParams OnRewind(TweenCallback action)
		{
			this.onRewind = action;
			return this;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000063B6 File Offset: 0x000045B6
		public TweenParams OnUpdate(TweenCallback action)
		{
			this.onUpdate = action;
			return this;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000063C0 File Offset: 0x000045C0
		public TweenParams OnStepComplete(TweenCallback action)
		{
			this.onStepComplete = action;
			return this;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000063CA File Offset: 0x000045CA
		public TweenParams OnComplete(TweenCallback action)
		{
			this.onComplete = action;
			return this;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000063D4 File Offset: 0x000045D4
		public TweenParams OnKill(TweenCallback action)
		{
			this.onKill = action;
			return this;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000063DE File Offset: 0x000045DE
		public TweenParams OnWaypointChange(TweenCallback<int> action)
		{
			this.onWaypointChange = action;
			return this;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000063E8 File Offset: 0x000045E8
		public TweenParams SetDelay(float delay)
		{
			this.delay = delay;
			return this;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000063F2 File Offset: 0x000045F2
		public TweenParams SetRelative(bool isRelative = true)
		{
			this.isRelative = isRelative;
			return this;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000063FC File Offset: 0x000045FC
		public TweenParams SetSpeedBased(bool isSpeedBased = true)
		{
			this.isSpeedBased = isSpeedBased;
			return this;
		}

		// Token: 0x04000068 RID: 104
		public static readonly TweenParams Params = new TweenParams();

		// Token: 0x04000069 RID: 105
		internal object id;

		// Token: 0x0400006A RID: 106
		internal object target;

		// Token: 0x0400006B RID: 107
		internal UpdateType updateType;

		// Token: 0x0400006C RID: 108
		internal bool isIndependentUpdate;

		// Token: 0x0400006D RID: 109
		internal TweenCallback onStart;

		// Token: 0x0400006E RID: 110
		internal TweenCallback onPlay;

		// Token: 0x0400006F RID: 111
		internal TweenCallback onRewind;

		// Token: 0x04000070 RID: 112
		internal TweenCallback onUpdate;

		// Token: 0x04000071 RID: 113
		internal TweenCallback onStepComplete;

		// Token: 0x04000072 RID: 114
		internal TweenCallback onComplete;

		// Token: 0x04000073 RID: 115
		internal TweenCallback onKill;

		// Token: 0x04000074 RID: 116
		internal TweenCallback<int> onWaypointChange;

		// Token: 0x04000075 RID: 117
		internal bool isRecyclable;

		// Token: 0x04000076 RID: 118
		internal bool isSpeedBased;

		// Token: 0x04000077 RID: 119
		internal bool autoKill;

		// Token: 0x04000078 RID: 120
		internal int loops;

		// Token: 0x04000079 RID: 121
		internal LoopType loopType;

		// Token: 0x0400007A RID: 122
		internal float delay;

		// Token: 0x0400007B RID: 123
		internal bool isRelative;

		// Token: 0x0400007C RID: 124
		internal Ease easeType;

		// Token: 0x0400007D RID: 125
		internal EaseFunction customEase;

		// Token: 0x0400007E RID: 126
		internal float easeOvershootOrAmplitude;

		// Token: 0x0400007F RID: 127
		internal float easePeriod;
	}
}
