// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.UintPlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;

namespace DG.Tweening.Plugins
{
  public class UintPlugin : ABSTweenPlugin<uint, uint, UintOptions>
  {
    public override void Reset(TweenerCore<uint, uint, UintOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<uint, uint, UintOptions> t, bool isRelative)
    {
      uint endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      t.setter(t.startValue);
    }

    public override uint ConvertToStartValue(TweenerCore<uint, uint, UintOptions> t, uint value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<uint, uint, UintOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<uint, uint, UintOptions> t)
    {
      t.plugOptions.isNegativeChangeValue = t.endValue < t.startValue;
      t.changeValue = t.plugOptions.isNegativeChangeValue ? t.startValue - t.endValue : t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(UintOptions options, float unitsXSecond, uint changeValue)
    {
      float num = (float) changeValue / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(UintOptions options, Tween t, bool isRelative, DOGetter<uint> getter, DOSetter<uint> setter, float elapsed, uint startValue, uint changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
      {
        uint num = (uint) ((long) changeValue * (t.isComplete ? (long) (t.completedLoops - 1) : (long) t.completedLoops));
        if (options.isNegativeChangeValue)
          startValue -= num;
        else
          startValue += num;
      }
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
      {
        uint num = (uint) ((long) changeValue * (t.loopType == LoopType.Incremental ? (long) t.loops : 1L) * (t.sequenceParent.isComplete ? (long) (t.sequenceParent.completedLoops - 1) : (long) t.sequenceParent.completedLoops));
        if (options.isNegativeChangeValue)
          startValue -= num;
        else
          startValue += num;
      }
      uint num1 = (uint) Math.Round((double) changeValue * (double) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
      if (options.isNegativeChangeValue)
        setter(startValue - num1);
      else
        setter(startValue + num1);
    }
  }
}
