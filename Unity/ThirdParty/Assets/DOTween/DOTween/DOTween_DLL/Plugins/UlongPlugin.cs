// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.UlongPlugin
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
  public class UlongPlugin : ABSTweenPlugin<ulong, ulong, NoOptions>
  {
    public override void Reset(TweenerCore<ulong, ulong, NoOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<ulong, ulong, NoOptions> t, bool isRelative)
    {
      ulong endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      t.setter(t.startValue);
    }

    public override ulong ConvertToStartValue(TweenerCore<ulong, ulong, NoOptions> t, ulong value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<ulong, ulong, NoOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<ulong, ulong, NoOptions> t)
    {
      t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, ulong changeValue)
    {
      float num = (float) changeValue / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<ulong> getter, DOSetter<ulong> setter, float elapsed, ulong startValue, ulong changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? (ulong) (uint) (t.completedLoops - 1) : (ulong) (uint) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += (ulong) ((long) changeValue * (t.loopType == LoopType.Incremental ? (long) (uint) t.loops : 1L) * (t.sequenceParent.isComplete ? (long) (uint) (t.sequenceParent.completedLoops - 1) : (long) (uint) t.sequenceParent.completedLoops));
      setter((ulong) ((Decimal) startValue + (Decimal) changeValue * (Decimal) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
    }
  }
}
