using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000019 RID: 25
	public abstract class Tweener : Tween
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00007C0C File Offset: 0x00005E0C
		internal Tweener()
		{
		}

		// Token: 0x06000167 RID: 359
		public abstract Tweener ChangeStartValue(object newStartValue, float newDuration = -1f);

		// Token: 0x06000168 RID: 360
		public abstract Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false);

		// Token: 0x06000169 RID: 361
		public abstract Tweener ChangeEndValue(object newEndValue, bool snapStartValue);

		// Token: 0x0600016A RID: 362
		public abstract Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f);

		// Token: 0x0600016B RID: 363
		internal abstract Tweener SetFrom(bool relative);

		// Token: 0x0600016C RID: 364 RVA: 0x00007C1C File Offset: 0x00005E1C
		internal static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct, IPlugOptions
		{
			if (plugin != null)
			{
				t.tweenPlugin = plugin;
			}
			else
			{
				if (t.tweenPlugin == null)
				{
					t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
				}
				if (t.tweenPlugin == null)
				{
					Debugger.LogError("No suitable plugin found for this type");
					return false;
				}
			}
			t.getter = getter;
			t.setter = setter;
			t.endValue = endValue;
			t.duration = duration;
			t.autoKill = DOTween.defaultAutoKill;
			t.isRecyclable = DOTween.defaultRecyclable;
			t.easeType = DOTween.defaultEaseType;
			t.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			t.easePeriod = DOTween.defaultEasePeriod;
			t.loopType = DOTween.defaultLoopType;
			t.isPlaying = (DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlayTweeners);
			return true;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007CD8 File Offset: 0x00005ED8
		internal static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct, IPlugOptions
		{
			float delay = t.delay;
			if (elapsed > delay)
			{
				t.elapsedDelay = delay;
				t.delayComplete = true;
				return elapsed - delay;
			}
			t.elapsedDelay = elapsed;
			return 0f;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007D10 File Offset: 0x00005F10
		internal static bool DoStartup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			t.startupDone = true;
			if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
			{
				return false;
			}
			if (!t.hasManuallySetStartValue)
			{
				if (DOTween.useSafeMode)
				{
					try
					{
						t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
						goto IL_84;
					}
					catch (Exception ex)
					{
						Debugger.LogWarning(string.Format("Tween startup failed (NULL target/property - {0}): the tween will now be killed ► {1}", ex.TargetSite, ex.Message));
						return false;
					}
				}
				t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
			}
			IL_84:
			if (t.isRelative)
			{
				t.tweenPlugin.SetRelativeEndValue(t);
			}
			t.tweenPlugin.SetChangeValue(t);
			Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
			if (t.duration <= 0f)
			{
				t.easeType = Ease.INTERNAL_Zero;
			}
			return true;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007DF0 File Offset: 0x00005FF0
		internal static Tweener DoChangeStartValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, float newDuration) where TPlugOptions : struct, IPlugOptions
		{
			t.hasManuallySetStartValue = true;
			t.startValue = newStartValue;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007E60 File Offset: 0x00006060
		internal static Tweener DoChangeEndValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue, float newDuration, bool snapStartValue) where TPlugOptions : struct, IPlugOptions
		{
			t.endValue = newEndValue;
			t.isRelative = false;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				if (snapStartValue)
				{
					if (DOTween.useSafeMode)
					{
						try
						{
							t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
							goto IL_7A;
						}
						catch
						{
							TweenManager.Despawn(t, true);
							return null;
						}
					}
					t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
				}
				IL_7A:
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007F34 File Offset: 0x00006134
		internal static Tweener DoChangeValues<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, T2 newEndValue, float newDuration) where TPlugOptions : struct, IPlugOptions
		{
			t.hasManuallySetStartValue = true;
			t.isRelative = (t.isFrom = false);
			t.startValue = newStartValue;
			t.endValue = newEndValue;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007FB8 File Offset: 0x000061B8
		private static bool DOStartupSpecials<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			bool result;
			try
			{
				switch (t.specialStartupMode)
				{
				case SpecialStartupMode.SetLookAt:
					if (!SpecialPluginsUtils.SetLookAt(t as TweenerCore<Quaternion, Vector3, QuaternionOptions>))
					{
						return false;
					}
					break;
				case SpecialStartupMode.SetShake:
					if (!SpecialPluginsUtils.SetShake(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						return false;
					}
					break;
				case SpecialStartupMode.SetPunch:
					if (!SpecialPluginsUtils.SetPunch(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						return false;
					}
					break;
				case SpecialStartupMode.SetCameraShakePosition:
					if (!SpecialPluginsUtils.SetCameraShakePosition(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						return false;
					}
					break;
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008044 File Offset: 0x00006244
		private static void DOStartupDurationBased<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			if (t.isSpeedBased)
			{
				t.duration = t.tweenPlugin.GetSpeedBasedDuration(t.plugOptions, t.duration, t.changeValue);
			}
			t.fullDuration = ((t.loops > -1) ? (t.duration * (float)t.loops) : float.PositiveInfinity);
		}

		// Token: 0x040000B5 RID: 181
		internal bool hasManuallySetStartValue;

		// Token: 0x040000B6 RID: 182
		internal bool isFromAllowed = true;
	}
}
