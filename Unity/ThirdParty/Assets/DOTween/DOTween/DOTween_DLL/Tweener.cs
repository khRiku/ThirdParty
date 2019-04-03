// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Tweener
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

namespace DG.Tweening
{
  /// <summary>Animates a single value</summary>
  public abstract class Tweener : Tween
  {
    public bool isFromAllowed = true;
    public bool hasManuallySetStartValue;

    public Tweener()
    {
    }

    /// <summary>Changes the start value of a tween and rewinds it (without pausing it).
    /// Has no effect with tweens that are inside Sequences</summary>
    /// <param name="newStartValue">The new start value</param>
    /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
    public abstract Tweener ChangeStartValue(object newStartValue, float newDuration = -1f);

    /// <summary>Changes the end value of a tween and rewinds it (without pausing it).
    /// Has no effect with tweens that are inside Sequences</summary>
    /// <param name="newEndValue">The new end value</param>
    /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
    /// <param name="snapStartValue">If TRUE the start value will become the current target's value, otherwise it will stay the same</param>
    public abstract Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false);

    /// <summary>Changes the end value of a tween and rewinds it (without pausing it).
    /// Has no effect with tweens that are inside Sequences</summary>
    /// <param name="newEndValue">The new end value</param>
    /// <param name="snapStartValue">If TRUE the start value will become the current target's value, otherwise it will stay the same</param>
    public abstract Tweener ChangeEndValue(object newEndValue, bool snapStartValue);

    /// <summary>Changes the start and end value of a tween and rewinds it (without pausing it).
    /// Has no effect with tweens that are inside Sequences</summary>
    /// <param name="newStartValue">The new start value</param>
    /// <param name="newEndValue">The new end value</param>
    /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
    public abstract Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f);

    public abstract Tweener SetFrom(bool relative);

    public static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct, IPlugOptions
    {
      if (plugin != null)
      {
        t.tweenPlugin = plugin;
      }
      else
      {
        if (t.tweenPlugin == null)
          t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
        if (t.tweenPlugin == null)
        {
          Debugger.LogError((object) "No suitable plugin found for this type");
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
      t.isPlaying = DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlayTweeners;
      return true;
    }

    public static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct, IPlugOptions
    {
      float delay = t.delay;
      if ((double) elapsed > (double) delay)
      {
        t.elapsedDelay = delay;
        t.delayComplete = true;
        return elapsed - delay;
      }
      t.elapsedDelay = elapsed;
      return 0.0f;
    }

    public static bool DoStartup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
    {
      t.startupDone = true;
      if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
        return false;
      if (!t.hasManuallySetStartValue)
      {
        if (DOTween.useSafeMode)
        {
          try
          {
            t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
          }
          catch (Exception ex)
          {
            Debugger.LogWarning((object) string.Format("Tween startup failed (NULL target/property - {0}): the tween will now be killed ► {1}", (object) ex.TargetSite, (object) ex.Message));
            return false;
          }
        }
        else
          t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
      }
      if (t.isRelative)
        t.tweenPlugin.SetRelativeEndValue(t);
      t.tweenPlugin.SetChangeValue(t);
      Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
      if ((double) t.duration <= 0.0)
        t.easeType = Ease.INTERNAL_Zero;
      return true;
    }

    public static Tweener DoChangeStartValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, float newDuration) where TPlugOptions : struct, IPlugOptions
    {
      t.hasManuallySetStartValue = true;
      t.startValue = newStartValue;
      if (t.startupDone)
      {
        if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
          return (Tweener) null;
        t.tweenPlugin.SetChangeValue(t);
      }
      if ((double) newDuration > 0.0)
      {
        t.duration = newDuration;
        if (t.startupDone)
          Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
      }
      Tween.DoGoto((Tween) t, 0.0f, 0, UpdateMode.IgnoreOnUpdate);
      return (Tweener) t;
    }

    public static Tweener DoChangeEndValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue, float newDuration, bool snapStartValue) where TPlugOptions : struct, IPlugOptions
    {
      t.endValue = newEndValue;
      t.isRelative = false;
      if (t.startupDone)
      {
        if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
          return (Tweener) null;
        if (snapStartValue)
        {
          if (DOTween.useSafeMode)
          {
            try
            {
              t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
            }
            catch
            {
              TweenManager.Despawn((Tween) t, true);
              return (Tweener) null;
            }
          }
          else
            t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
        }
        t.tweenPlugin.SetChangeValue(t);
      }
      if ((double) newDuration > 0.0)
      {
        t.duration = newDuration;
        if (t.startupDone)
          Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
      }
      Tween.DoGoto((Tween) t, 0.0f, 0, UpdateMode.IgnoreOnUpdate);
      return (Tweener) t;
    }

    public static Tweener DoChangeValues<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, T2 newEndValue, float newDuration) where TPlugOptions : struct, IPlugOptions
    {
      t.hasManuallySetStartValue = true;
      t.isRelative = t.isFrom = false;
      t.startValue = newStartValue;
      t.endValue = newEndValue;
      if (t.startupDone)
      {
        if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
          return (Tweener) null;
        t.tweenPlugin.SetChangeValue(t);
      }
      if ((double) newDuration > 0.0)
      {
        t.duration = newDuration;
        if (t.startupDone)
          Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
      }
      Tween.DoGoto((Tween) t, 0.0f, 0, UpdateMode.IgnoreOnUpdate);
      return (Tweener) t;
    }

    private static bool DOStartupSpecials<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
    {
      try
      {
        switch (t.specialStartupMode)
        {
          case SpecialStartupMode.SetLookAt:
            if (!SpecialPluginsUtils.SetLookAt(t as TweenerCore<Quaternion, Vector3, QuaternionOptions>))
              return false;
            break;
          case SpecialStartupMode.SetShake:
            if (!SpecialPluginsUtils.SetShake(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
              return false;
            break;
          case SpecialStartupMode.SetPunch:
            if (!SpecialPluginsUtils.SetPunch(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
              return false;
            break;
          case SpecialStartupMode.SetCameraShakePosition:
            if (!SpecialPluginsUtils.SetCameraShakePosition(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
              return false;
            break;
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    private static void DOStartupDurationBased<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
    {
      if (t.isSpeedBased)
        t.duration = t.tweenPlugin.GetSpeedBasedDuration(t.plugOptions, t.duration, t.changeValue);
      t.fullDuration = t.loops > -1 ? t.duration * (float) t.loops : float.PositiveInfinity;
    }
  }
}
