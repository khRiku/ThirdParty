// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.RectOffsetPlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

namespace DG.Tweening.Plugins
{
  public class RectOffsetPlugin : ABSTweenPlugin<RectOffset, RectOffset, NoOptions>
  {
    private static RectOffset _r = new RectOffset();

    public override void Reset(TweenerCore<RectOffset, RectOffset, NoOptions> t)
    {
      t.startValue = t.endValue = t.changeValue = (RectOffset) null;
    }

    public override void SetFrom(TweenerCore<RectOffset, RectOffset, NoOptions> t, bool isRelative)
    {
      RectOffset endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = endValue;
      if (isRelative)
      {
        t.startValue.left += t.endValue.left;
        t.startValue.right += t.endValue.right;
        t.startValue.top += t.endValue.top;
        t.startValue.bottom += t.endValue.bottom;
      }
      t.setter(t.startValue);
    }

    public override RectOffset ConvertToStartValue(TweenerCore<RectOffset, RectOffset, NoOptions> t, RectOffset value)
    {
      return new RectOffset(value.left, value.right, value.top, value.bottom);
    }

    public override void SetRelativeEndValue(TweenerCore<RectOffset, RectOffset, NoOptions> t)
    {
      t.endValue.left += t.startValue.left;
      t.endValue.right += t.startValue.right;
      t.endValue.top += t.startValue.top;
      t.endValue.bottom += t.startValue.bottom;
    }

    public override void SetChangeValue(TweenerCore<RectOffset, RectOffset, NoOptions> t)
    {
      t.changeValue = new RectOffset(t.endValue.left - t.startValue.left, t.endValue.right - t.startValue.right, t.endValue.top - t.startValue.top, t.endValue.bottom - t.startValue.bottom);
    }

    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, RectOffset changeValue)
    {
      float num1 = (float) changeValue.right;
      if ((double) num1 < 0.0)
        num1 = -num1;
      float num2 = (float) changeValue.bottom;
      if ((double) num2 < 0.0)
        num2 = -num2;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2) / unitsXSecond;
    }

    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, float elapsed, RectOffset startValue, RectOffset changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      RectOffsetPlugin._r.left = startValue.left;
      RectOffsetPlugin._r.right = startValue.right;
      RectOffsetPlugin._r.top = startValue.top;
      RectOffsetPlugin._r.bottom = startValue.bottom;
      if (t.loopType == LoopType.Incremental)
      {
        int num = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
        RectOffsetPlugin._r.left += changeValue.left * num;
        RectOffsetPlugin._r.right += changeValue.right * num;
        RectOffsetPlugin._r.top += changeValue.top * num;
        RectOffsetPlugin._r.bottom += changeValue.bottom * num;
      }
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
      {
        int num = (t.loopType == LoopType.Incremental ? t.loops : 1) * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
        RectOffsetPlugin._r.left += changeValue.left * num;
        RectOffsetPlugin._r.right += changeValue.right * num;
        RectOffsetPlugin._r.top += changeValue.top * num;
        RectOffsetPlugin._r.bottom += changeValue.bottom * num;
      }
      float num1 = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      setter(new RectOffset((int) Math.Round((double) RectOffsetPlugin._r.left + (double) changeValue.left * (double) num1), (int) Math.Round((double) RectOffsetPlugin._r.right + (double) changeValue.right * (double) num1), (int) Math.Round((double) RectOffsetPlugin._r.top + (double) changeValue.top * (double) num1), (int) Math.Round((double) RectOffsetPlugin._r.bottom + (double) changeValue.bottom * (double) num1)));
    }
  }
}
