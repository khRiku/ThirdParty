// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.DoublePlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
  public class DoublePlugin : ABSTweenPlugin<double, double, NoOptions>
  {
    public override void Reset(TweenerCore<double, double, NoOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<double, double, NoOptions> t, bool isRelative)
    {
      double endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      t.setter(t.startValue);
    }

    public override double ConvertToStartValue(TweenerCore<double, double, NoOptions> t, double value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<double, double, NoOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<double, double, NoOptions> t)
    {
      t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, double changeValue)
    {
      float num = (float) changeValue / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<double> getter, DOSetter<double> setter, float elapsed, double startValue, double changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? (double) (t.completedLoops - 1) : (double) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += changeValue * (t.loopType == LoopType.Incremental ? (double) t.loops : 1.0) * (t.sequenceParent.isComplete ? (double) (t.sequenceParent.completedLoops - 1) : (double) t.sequenceParent.completedLoops);
      setter(startValue + changeValue * (double) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
    }
  }
}
