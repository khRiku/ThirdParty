// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.IntPlugin
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
  public class IntPlugin : ABSTweenPlugin<int, int, NoOptions>
  {
    public override void Reset(TweenerCore<int, int, NoOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<int, int, NoOptions> t, bool isRelative)
    {
      int endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      t.setter(t.startValue);
    }

    public override int ConvertToStartValue(TweenerCore<int, int, NoOptions> t, int value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<int, int, NoOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<int, int, NoOptions> t)
    {
      t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, int changeValue)
    {
      float num = (float) changeValue / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<int> getter, DOSetter<int> setter, float elapsed, int startValue, int changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1) * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
      setter((int) Math.Round((double) startValue + (double) changeValue * (double) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
    }
  }
}
