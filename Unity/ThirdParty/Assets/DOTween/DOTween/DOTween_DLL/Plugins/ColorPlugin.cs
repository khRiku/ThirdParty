// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.ColorPlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
  public class ColorPlugin : ABSTweenPlugin<Color, Color, ColorOptions>
  {
    public override void Reset(TweenerCore<Color, Color, ColorOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<Color, Color, ColorOptions> t, bool isRelative)
    {
      Color endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      Color pNewValue = t.endValue;
      if (!t.plugOptions.alphaOnly)
        pNewValue = t.startValue;
      else
        pNewValue.a = t.startValue.a;
      t.setter(pNewValue);
    }

    public override Color ConvertToStartValue(TweenerCore<Color, Color, ColorOptions> t, Color value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<Color, Color, ColorOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<Color, Color, ColorOptions> t)
    {
      t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color changeValue)
    {
      return 1f / unitsXSecond;
    }

    public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color> getter, DOSetter<Color> setter, float elapsed, Color startValue, Color changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? (float) (t.completedLoops - 1) : (float) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += changeValue * (t.loopType == LoopType.Incremental ? (float) t.loops : 1f) * (t.sequenceParent.isComplete ? (float) (t.sequenceParent.completedLoops - 1) : (float) t.sequenceParent.completedLoops);
      float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      if (!options.alphaOnly)
      {
        startValue.r += changeValue.r * num;
        startValue.g += changeValue.g * num;
        startValue.b += changeValue.b * num;
        startValue.a += changeValue.a * num;
        setter(startValue);
      }
      else
      {
        Color pNewValue = getter();
        pNewValue.a = startValue.a + changeValue.a * num;
        setter(pNewValue);
      }
    }
  }
}
