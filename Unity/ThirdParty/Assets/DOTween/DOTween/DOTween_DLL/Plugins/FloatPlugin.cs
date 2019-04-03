// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.FloatPlugin
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
  public class FloatPlugin : ABSTweenPlugin<float, float, FloatOptions>
  {
    public override void Reset(TweenerCore<float, float, FloatOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<float, float, FloatOptions> t, bool isRelative)
    {
      float endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      t.setter(!t.plugOptions.snapping ? t.startValue : (float) Math.Round((double) t.startValue));
    }

    public override float ConvertToStartValue(TweenerCore<float, float, FloatOptions> t, float value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<float, float, FloatOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<float, float, FloatOptions> t)
    {
      t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(FloatOptions options, float unitsXSecond, float changeValue)
    {
      float num = changeValue / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(FloatOptions options, Tween t, bool isRelative, DOGetter<float> getter, DOSetter<float> setter, float elapsed, float startValue, float changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? (float) (t.completedLoops - 1) : (float) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += (float) ((double) changeValue * (t.loopType == LoopType.Incremental ? (double) t.loops : 1.0) * (t.sequenceParent.isComplete ? (double) (t.sequenceParent.completedLoops - 1) : (double) t.sequenceParent.completedLoops));
      setter(!options.snapping ? startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod) : (float) Math.Round((double) startValue + (double) changeValue * (double) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
    }
  }
}
