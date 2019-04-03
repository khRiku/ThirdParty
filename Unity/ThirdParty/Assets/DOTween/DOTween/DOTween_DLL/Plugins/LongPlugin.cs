// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.LongPlugin
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
  public class LongPlugin : ABSTweenPlugin<long, long, NoOptions>
  {
    public override void Reset(TweenerCore<long, long, NoOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<long, long, NoOptions> t, bool isRelative)
    {
      long endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      t.setter(t.startValue);
    }

    public override long ConvertToStartValue(TweenerCore<long, long, NoOptions> t, long value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<long, long, NoOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<long, long, NoOptions> t)
    {
      t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, long changeValue)
    {
      float num = (float) changeValue / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<long> getter, DOSetter<long> setter, float elapsed, long startValue, long changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? (long) (t.completedLoops - 1) : (long) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += changeValue * (t.loopType == LoopType.Incremental ? (long) t.loops : 1L) * (t.sequenceParent.isComplete ? (long) (t.sequenceParent.completedLoops - 1) : (long) t.sequenceParent.completedLoops);
      setter((long) Math.Round((double) startValue + (double) changeValue * (double) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
    }
  }
}
