// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.SpiralPlugin
// Assembly: DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0FFB13C2-E5DA-4737-82C8-2ACE533F01F7
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\DOTweenPro.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using System;
using UnityEngine;

namespace DG.Tweening.Plugins
{
  /// <summary>
  /// Tweens a Vector3 along a spiral.
  /// EndValue represents the direction of the spiral
  /// </summary>
  public class SpiralPlugin : ABSTweenPlugin<Vector3, Vector3, SpiralOptions>
  {
    public static readonly Vector3 DefaultDirection = Vector3.forward;

    public override void Reset(TweenerCore<Vector3, Vector3, SpiralOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<Vector3, Vector3, SpiralOptions> t, bool isRelative)
    {
    }

    public static ABSTweenPlugin<Vector3, Vector3, SpiralOptions> Get()
    {
      return PluginsManager.GetCustomPlugin<SpiralPlugin, Vector3, Vector3, SpiralOptions>();
    }

    public override Vector3 ConvertToStartValue(TweenerCore<Vector3, Vector3, SpiralOptions> t, Vector3 value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3, SpiralOptions> t)
    {
    }

    public override void SetChangeValue(TweenerCore<Vector3, Vector3, SpiralOptions> t)
    {
      t.plugOptions.speed *= 10f / t.plugOptions.frequency;
      t.plugOptions.axisQ = Quaternion.LookRotation(t.endValue, Vector3.up);
    }

    public override float GetSpeedBasedDuration(SpiralOptions options, float unitsXSecond, Vector3 changeValue)
    {
      return unitsXSecond;
    }

    public override void EvaluateAndApply(SpiralOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      float num1 = EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      float num2 = options.mode != SpiralMode.ExpandThenContract || (double) num1 <= 0.5 ? num1 : (float) (0.5 - ((double) num1 - 0.5));
      if (t.loopType == LoopType.Incremental)
        num1 += t.isComplete ? (float) (t.completedLoops - 1) : (float) t.completedLoops;
      float num3 = duration * options.speed * num1;
      options.unit = duration * options.speed * num2;
      Vector3 pNewValue = new Vector3(options.unit * Mathf.Cos(num3 * options.frequency), options.unit * Mathf.Sin(num3 * options.frequency), options.depth * num1);
      pNewValue = options.axisQ * pNewValue + startValue;
      if (options.snapping)
      {
        pNewValue.x = (float) Math.Round((double) pNewValue.x);
        pNewValue.y = (float) Math.Round((double) pNewValue.y);
        pNewValue.z = (float) Math.Round((double) pNewValue.z);
      }
      setter(pNewValue);
    }
  }
}
