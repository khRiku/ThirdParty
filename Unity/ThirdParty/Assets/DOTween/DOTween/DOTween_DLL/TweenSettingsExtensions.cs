using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000016 RID: 22
	public static class TweenSettingsExtensions
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00006412 File Offset: 0x00004612
		public static T SetAutoKill<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.autoKill = true;
			return t;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006445 File Offset: 0x00004645
		public static T SetAutoKill<T>(this T t, bool autoKillOnCompletion) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.autoKill = autoKillOnCompletion;
			return t;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006478 File Offset: 0x00004678
		public static T SetId<T>(this T t, object objectId) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.id = objectId;
			return t;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000649E File Offset: 0x0000469E
		public static T SetId<T>(this T t, string stringId) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.stringId = stringId;
			return t;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000064C4 File Offset: 0x000046C4
		public static T SetId<T>(this T t, int intId) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.intId = intId;
			return t;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000064EA File Offset: 0x000046EA
		public static T SetTarget<T>(this T t, object target) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.target = target;
			return t;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006510 File Offset: 0x00004710
		public static T SetLoops<T>(this T t, int loops) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			t.loops = loops;
			if (t.tweenType == TweenType.Tweener)
			{
				if (loops > -1)
				{
					t.fullDuration = t.duration * (float)loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			return t;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000659C File Offset: 0x0000479C
		public static T SetLoops<T>(this T t, int loops, LoopType loopType) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			t.loops = loops;
			t.loopType = loopType;
			if (t.tweenType == TweenType.Tweener)
			{
				if (loops > -1)
				{
					t.fullDuration = t.duration * (float)loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			return t;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006634 File Offset: 0x00004834
		public static T SetEase<T>(this T t, Ease ease) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = ease;
			if (EaseManager.IsFlashEase(ease))
			{
				t.easeOvershootOrAmplitude = (float)((int)t.easeOvershootOrAmplitude);
			}
			t.customEase = null;
			return t;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006694 File Offset: 0x00004894
		public static T SetEase<T>(this T t, Ease ease, float overshoot) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = ease;
			if (EaseManager.IsFlashEase(ease))
			{
				overshoot = (float)((int)overshoot);
			}
			t.easeOvershootOrAmplitude = overshoot;
			t.customEase = null;
			return t;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000066EC File Offset: 0x000048EC
		public static T SetEase<T>(this T t, Ease ease, float amplitude, float period) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = ease;
			if (EaseManager.IsFlashEase(ease))
			{
				amplitude = (float)((int)amplitude);
			}
			t.easeOvershootOrAmplitude = amplitude;
			t.easePeriod = period;
			t.customEase = null;
			return t;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006750 File Offset: 0x00004950
		public static T SetEase<T>(this T t, AnimationCurve animCurve) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = Ease.INTERNAL_Custom;
			t.customEase = new EaseFunction(new EaseCurve(animCurve).Evaluate);
			return t;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000679E File Offset: 0x0000499E
		public static T SetEase<T>(this T t, EaseFunction customEase) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.easeType = Ease.INTERNAL_Custom;
			t.customEase = customEase;
			return t;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000067D1 File Offset: 0x000049D1
		public static T SetRecyclable<T>(this T t) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.isRecyclable = true;
			return t;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000067F7 File Offset: 0x000049F7
		public static T SetRecyclable<T>(this T t, bool recyclable) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.isRecyclable = recyclable;
			return t;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000681D File Offset: 0x00004A1D
		public static T SetUpdate<T>(this T t, bool isIndependentUpdate) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, DOTween.defaultUpdateType, isIndependentUpdate);
			return t;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006848 File Offset: 0x00004A48
		public static T SetUpdate<T>(this T t, UpdateType updateType) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, updateType, DOTween.defaultTimeScaleIndependent);
			return t;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006873 File Offset: 0x00004A73
		public static T SetUpdate<T>(this T t, UpdateType updateType, bool isIndependentUpdate) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, updateType, isIndependentUpdate);
			return t;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000689A File Offset: 0x00004A9A
		public static T OnStart<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onStart = action;
			return t;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000068C0 File Offset: 0x00004AC0
		public static T OnPlay<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onPlay = action;
			return t;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000068E6 File Offset: 0x00004AE6
		public static T OnPause<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onPause = action;
			return t;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000690C File Offset: 0x00004B0C
		public static T OnRewind<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onRewind = action;
			return t;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006932 File Offset: 0x00004B32
		public static T OnUpdate<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onUpdate = action;
			return t;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006958 File Offset: 0x00004B58
		public static T OnStepComplete<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onStepComplete = action;
			return t;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000697E File Offset: 0x00004B7E
		public static T OnComplete<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onComplete = action;
			return t;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000069A4 File Offset: 0x00004BA4
		public static T OnKill<T>(this T t, TweenCallback action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onKill = action;
			return t;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000069CA File Offset: 0x00004BCA
		public static T OnWaypointChange<T>(this T t, TweenCallback<int> action) where T : Tween
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.onWaypointChange = action;
			return t;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000069F0 File Offset: 0x00004BF0
		public static T SetAs<T>(this T t, Tween asTween) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.timeScale = asTween.timeScale;
			t.isBackwards = asTween.isBackwards;
			TweenManager.SetUpdateType(t, asTween.updateType, asTween.isIndependentUpdate);
			t.id = asTween.id;
			t.onStart = asTween.onStart;
			t.onPlay = asTween.onPlay;
			t.onRewind = asTween.onRewind;
			t.onUpdate = asTween.onUpdate;
			t.onStepComplete = asTween.onStepComplete;
			t.onComplete = asTween.onComplete;
			t.onKill = asTween.onKill;
			t.onWaypointChange = asTween.onWaypointChange;
			t.isRecyclable = asTween.isRecyclable;
			t.isSpeedBased = asTween.isSpeedBased;
			t.autoKill = asTween.autoKill;
			t.loops = asTween.loops;
			t.loopType = asTween.loopType;
			if (t.tweenType == TweenType.Tweener)
			{
				if (t.loops > -1)
				{
					t.fullDuration = t.duration * (float)t.loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			t.delay = asTween.delay;
			t.delayComplete = (t.delay <= 0f);
			t.isRelative = asTween.isRelative;
			t.easeType = asTween.easeType;
			t.customEase = asTween.customEase;
			t.easeOvershootOrAmplitude = asTween.easeOvershootOrAmplitude;
			t.easePeriod = asTween.easePeriod;
			return t;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006C20 File Offset: 0x00004E20
		public static T SetAs<T>(this T t, TweenParams tweenParams) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			TweenManager.SetUpdateType(t, tweenParams.updateType, tweenParams.isIndependentUpdate);
			t.id = tweenParams.id;
			t.onStart = tweenParams.onStart;
			t.onPlay = tweenParams.onPlay;
			t.onRewind = tweenParams.onRewind;
			t.onUpdate = tweenParams.onUpdate;
			t.onStepComplete = tweenParams.onStepComplete;
			t.onComplete = tweenParams.onComplete;
			t.onKill = tweenParams.onKill;
			t.onWaypointChange = tweenParams.onWaypointChange;
			t.isRecyclable = tweenParams.isRecyclable;
			t.isSpeedBased = tweenParams.isSpeedBased;
			t.autoKill = tweenParams.autoKill;
			t.loops = tweenParams.loops;
			t.loopType = tweenParams.loopType;
			if (t.tweenType == TweenType.Tweener)
			{
				if (t.loops > -1)
				{
					t.fullDuration = t.duration * (float)t.loops;
				}
				else
				{
					t.fullDuration = float.PositiveInfinity;
				}
			}
			t.delay = tweenParams.delay;
			t.delayComplete = (t.delay <= 0f);
			t.isRelative = tweenParams.isRelative;
			if (tweenParams.easeType == Ease.Unset)
			{
				if (t.tweenType == TweenType.Sequence)
				{
					t.easeType = Ease.Linear;
				}
				else
				{
					t.easeType = DOTween.defaultEaseType;
				}
			}
			else
			{
				t.easeType = tweenParams.easeType;
			}
			t.customEase = tweenParams.customEase;
			t.easeOvershootOrAmplitude = tweenParams.easeOvershootOrAmplitude;
			t.easePeriod = tweenParams.easePeriod;
			return t;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006E63 File Offset: 0x00005063
		public static Sequence Append(this Sequence s, Tween t)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (t == null || !t.active || t.isSequenced)
			{
				return s;
			}
			Sequence.DoInsert(s, t, s.duration);
			return s;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006E9E File Offset: 0x0000509E
		public static Sequence Prepend(this Sequence s, Tween t)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (t == null || !t.active || t.isSequenced)
			{
				return s;
			}
			Sequence.DoPrepend(s, t);
			return s;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006ED3 File Offset: 0x000050D3
		public static Sequence Join(this Sequence s, Tween t)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (t == null || !t.active || t.isSequenced)
			{
				return s;
			}
			Sequence.DoInsert(s, t, s.lastTweenInsertTime);
			return s;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006F0E File Offset: 0x0000510E
		public static Sequence Insert(this Sequence s, float atPosition, Tween t)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (t == null || !t.active || t.isSequenced)
			{
				return s;
			}
			Sequence.DoInsert(s, t, atPosition);
			return s;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006F44 File Offset: 0x00005144
		public static Sequence AppendInterval(this Sequence s, float interval)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			Sequence.DoAppendInterval(s, interval);
			return s;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006F64 File Offset: 0x00005164
		public static Sequence PrependInterval(this Sequence s, float interval)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			Sequence.DoPrependInterval(s, interval);
			return s;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006F84 File Offset: 0x00005184
		public static Sequence AppendCallback(this Sequence s, TweenCallback callback)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (callback == null)
			{
				return s;
			}
			Sequence.DoInsertCallback(s, callback, s.duration);
			return s;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006FAF File Offset: 0x000051AF
		public static Sequence PrependCallback(this Sequence s, TweenCallback callback)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (callback == null)
			{
				return s;
			}
			Sequence.DoInsertCallback(s, callback, 0f);
			return s;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006FD9 File Offset: 0x000051D9
		public static Sequence InsertCallback(this Sequence s, float atPosition, TweenCallback callback)
		{
			if (s == null || !s.active || s.creationLocked)
			{
				return s;
			}
			if (callback == null)
			{
				return s;
			}
			Sequence.DoInsertCallback(s, callback, atPosition);
			return s;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007000 File Offset: 0x00005200
		public static T From<T>(this T t) where T : Tweener
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			t.SetFrom(false);
			return t;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007058 File Offset: 0x00005258
		public static T From<T>(this T t, bool isRelative) where T : Tweener
		{
			if (t == null || !t.active || t.creationLocked || !t.isFromAllowed)
			{
				return t;
			}
			t.isFrom = true;
			if (!isRelative)
			{
				t.SetFrom(false);
			}
			else
			{
				t.SetFrom(!t.isBlendable);
			}
			return t;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000070D0 File Offset: 0x000052D0
		public static T SetDelay<T>(this T t, float delay) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			if (t.tweenType == TweenType.Sequence)
			{
				(t as Sequence).PrependInterval(delay);
			}
			else
			{
				t.delay = delay;
				t.delayComplete = (delay <= 0f);
			}
			return t;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007148 File Offset: 0x00005348
		public static T SetRelative<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked || t.isFrom || t.isBlendable)
			{
				return t;
			}
			t.isRelative = true;
			return t;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000071A0 File Offset: 0x000053A0
		public static T SetRelative<T>(this T t, bool isRelative) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked || t.isFrom || t.isBlendable)
			{
				return t;
			}
			t.isRelative = isRelative;
			return t;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000071F8 File Offset: 0x000053F8
		public static T SetSpeedBased<T>(this T t) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.isSpeedBased = true;
			return t;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000722B File Offset: 0x0000542B
		public static T SetSpeedBased<T>(this T t, bool isSpeedBased) where T : Tween
		{
			if (t == null || !t.active || t.creationLocked)
			{
				return t;
			}
			t.isSpeedBased = isSpeedBased;
			return t;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000725E File Offset: 0x0000545E
		public static Tweener SetOptions(this TweenerCore<float, float, FloatOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000727A File Offset: 0x0000547A
		public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, VectorOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007296 File Offset: 0x00005496
		public static Tweener SetOptions(this TweenerCore<Vector2, Vector2, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000072BE File Offset: 0x000054BE
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, VectorOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000072DA File Offset: 0x000054DA
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007302 File Offset: 0x00005502
		public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, VectorOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000731E File Offset: 0x0000551E
		public static Tweener SetOptions(this TweenerCore<Vector4, Vector4, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007346 File Offset: 0x00005546
		public static Tweener SetOptions(this TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool useShortest360Route = true)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.rotateMode = (useShortest360Route ? RotateMode.Fast : RotateMode.FastBeyond360);
			return t;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007368 File Offset: 0x00005568
		public static Tweener SetOptions(this TweenerCore<Color, Color, ColorOptions> t, bool alphaOnly)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.alphaOnly = alphaOnly;
			return t;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007384 File Offset: 0x00005584
		public static Tweener SetOptions(this TweenerCore<Rect, Rect, RectOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000073A0 File Offset: 0x000055A0
		public static Tweener SetOptions(this TweenerCore<string, string, StringOptions> t, bool richTextEnabled, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.richTextEnabled = richTextEnabled;
			t.plugOptions.scrambleMode = scrambleMode;
			if (!string.IsNullOrEmpty(scrambleChars))
			{
				if (scrambleChars.Length <= 1)
				{
					scrambleChars += scrambleChars;
				}
				t.plugOptions.scrambledChars = scrambleChars.ToCharArray();
				t.plugOptions.scrambledChars.ScrambleChars();
			}
			return t;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000740E File Offset: 0x0000560E
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool snapping)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000742A File Offset: 0x0000562A
		public static Tweener SetOptions(this TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, AxisConstraint axisConstraint, bool snapping = false)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.axisConstraint = axisConstraint;
			t.plugOptions.snapping = snapping;
			return t;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007452 File Offset: 0x00005652
		public static TweenerCore<Vector3, Path, PathOptions> SetOptions(this TweenerCore<Vector3, Path, PathOptions> t, AxisConstraint lockPosition, AxisConstraint lockRotation = AxisConstraint.None)
		{
			return t.SetOptions(false, lockPosition, lockRotation);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000745D File Offset: 0x0000565D
		public static TweenerCore<Vector3, Path, PathOptions> SetOptions(this TweenerCore<Vector3, Path, PathOptions> t, bool closePath, AxisConstraint lockPosition = AxisConstraint.None, AxisConstraint lockRotation = AxisConstraint.None)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.isClosedPath = closePath;
			t.plugOptions.lockPositionAxis = lockPosition;
			t.plugOptions.lockRotationAxis = lockRotation;
			return t;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007491 File Offset: 0x00005691
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, Vector3? forwardDirection = null, Vector3? up = null)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.orientType = OrientType.LookAtPosition;
			t.plugOptions.lookAtPosition = lookAtPosition;
			t.SetPathForwardDirection(forwardDirection, up);
			return t;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000074C1 File Offset: 0x000056C1
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, Vector3? forwardDirection = null, Vector3? up = null)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.orientType = OrientType.LookAtTransform;
			t.plugOptions.lookAtTransform = lookAtTransform;
			t.SetPathForwardDirection(forwardDirection, up);
			return t;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000074F1 File Offset: 0x000056F1
		public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(this TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, Vector3? forwardDirection = null, Vector3? up = null)
		{
			if (t == null || !t.active)
			{
				return t;
			}
			t.plugOptions.orientType = OrientType.ToPath;
			if (lookAhead < 0.0001f)
			{
				lookAhead = 0.0001f;
			}
			t.plugOptions.lookAhead = lookAhead;
			t.SetPathForwardDirection(forwardDirection, up);
			return t;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007530 File Offset: 0x00005730
		private static void SetPathForwardDirection(this TweenerCore<Vector3, Path, PathOptions> t, Vector3? forwardDirection = null, Vector3? up = null)
		{
			if (t == null || !t.active)
			{
				return;
			}
			bool hasCustomForwardDirection;
			if (forwardDirection != null)
			{
				Vector3? vector = forwardDirection;
				Vector3 zero = Vector3.zero;
				if (vector == null || (vector != null && vector.GetValueOrDefault() != zero))
				{
					hasCustomForwardDirection = true;
					goto IL_86;
				}
			}
			if (up != null)
			{
				Vector3? vector = up;
				Vector3 zero = Vector3.zero;
				hasCustomForwardDirection = (vector == null || (vector != null && vector.GetValueOrDefault() != zero));
			}
			else
			{
				hasCustomForwardDirection = false;
			}
			IL_86:
			t.plugOptions.hasCustomForwardDirection = hasCustomForwardDirection;
			if (t.plugOptions.hasCustomForwardDirection)
			{
				Vector3? vector = forwardDirection;
				Vector3 zero = Vector3.zero;
				if (vector != null && (vector == null || vector.GetValueOrDefault() == zero))
				{
					forwardDirection = new Vector3?(Vector3.forward);
				}
				t.plugOptions.forward = Quaternion.LookRotation((forwardDirection == null) ? Vector3.forward : forwardDirection.Value, (up == null) ? Vector3.up : up.Value);
			}
		}
	}
}
