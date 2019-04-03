// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Vector3Plugin
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
  public class Vector3Plugin : ABSTweenPlugin<Vector3, Vector3, VectorOptions>
  {
    public override void Reset(TweenerCore<Vector3, Vector3, VectorOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<Vector3, Vector3, VectorOptions> t, bool isRelative)
    {
      Vector3 endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue + endValue : endValue;
      Vector3 pNewValue = t.endValue;
      switch (t.plugOptions.axisConstraint)
      {
        case AxisConstraint.X:
          pNewValue.x = t.startValue.x;
          break;
        case AxisConstraint.Y:
          pNewValue.y = t.startValue.y;
          break;
        case AxisConstraint.Z:
          pNewValue.z = t.startValue.z;
          break;
        default:
          pNewValue = t.startValue;
          break;
      }
      if (t.plugOptions.snapping)
      {
        pNewValue.x = (float) Math.Round((double) pNewValue.x);
        pNewValue.y = (float) Math.Round((double) pNewValue.y);
        pNewValue.z = (float) Math.Round((double) pNewValue.z);
      }
      t.setter(pNewValue);
    }

    public override Vector3 ConvertToStartValue(TweenerCore<Vector3, Vector3, VectorOptions> t, Vector3 value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3, VectorOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<Vector3, Vector3, VectorOptions> t)
    {
      switch (t.plugOptions.axisConstraint)
      {
        case AxisConstraint.X:
          t.changeValue = new Vector3(t.endValue.x - t.startValue.x, 0.0f, 0.0f);
          break;
        case AxisConstraint.Y:
          t.changeValue = new Vector3(0.0f, t.endValue.y - t.startValue.y, 0.0f);
          break;
        case AxisConstraint.Z:
          t.changeValue = new Vector3(0.0f, 0.0f, t.endValue.z - t.startValue.z);
          break;
        default:
          t.changeValue = t.endValue - t.startValue;
          break;
      }
    }

    public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector3 changeValue)
    {
      return changeValue.magnitude / unitsXSecond;
    }

    public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental)
        startValue += changeValue * (t.isComplete ? (float) (t.completedLoops - 1) : (float) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        startValue += changeValue * (t.loopType == LoopType.Incremental ? (float) t.loops : 1f) * (t.sequenceParent.isComplete ? (float) (t.sequenceParent.completedLoops - 1) : (float) t.sequenceParent.completedLoops);
      float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      switch (options.axisConstraint)
      {
        case AxisConstraint.X:
          Vector3 pNewValue1 = getter();
          pNewValue1.x = startValue.x + changeValue.x * num;
          if (options.snapping)
            pNewValue1.x = (float) Math.Round((double) pNewValue1.x);
          setter(pNewValue1);
          break;
        case AxisConstraint.Y:
          Vector3 pNewValue2 = getter();
          pNewValue2.y = startValue.y + changeValue.y * num;
          if (options.snapping)
            pNewValue2.y = (float) Math.Round((double) pNewValue2.y);
          setter(pNewValue2);
          break;
        case AxisConstraint.Z:
          Vector3 pNewValue3 = getter();
          pNewValue3.z = startValue.z + changeValue.z * num;
          if (options.snapping)
            pNewValue3.z = (float) Math.Round((double) pNewValue3.z);
          setter(pNewValue3);
          break;
        default:
          startValue.x += changeValue.x * num;
          startValue.y += changeValue.y * num;
          startValue.z += changeValue.z * num;
          if (options.snapping)
          {
            startValue.x = (float) Math.Round((double) startValue.x);
            startValue.y = (float) Math.Round((double) startValue.y);
            startValue.z = (float) Math.Round((double) startValue.z);
          }
          setter(startValue);
          break;
      }
    }
  }
}
