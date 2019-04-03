// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Vector3ArrayPlugin
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
  /// <summary>This plugin generates some GC allocations at startup</summary>
  public class Vector3ArrayPlugin : ABSTweenPlugin<Vector3, Vector3[], Vector3ArrayOptions>
  {
    public override void Reset(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
    {
      t.startValue = t.endValue = t.changeValue = (Vector3[]) null;
    }

    public override void SetFrom(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool isRelative)
    {
    }

    public override Vector3[] ConvertToStartValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, Vector3 value)
    {
      int length = t.endValue.Length;
      Vector3[] vector3Array = new Vector3[length];
      for (int index = 0; index < length; ++index)
        vector3Array[index] = index != 0 ? t.endValue[index - 1] : value;
      return vector3Array;
    }

    public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
    {
      int length = t.endValue.Length;
      for (int index = 0; index < length; ++index)
      {
        if (index > 0)
          t.startValue[index] = t.endValue[index - 1];
        t.endValue[index] = t.startValue[index] + t.endValue[index];
      }
    }

    public override void SetChangeValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
    {
      int length = t.endValue.Length;
      t.changeValue = new Vector3[length];
      for (int index = 0; index < length; ++index)
        t.changeValue[index] = t.endValue[index] - t.startValue[index];
    }

    public override float GetSpeedBasedDuration(Vector3ArrayOptions options, float unitsXSecond, Vector3[] changeValue)
    {
      float num1 = 0.0f;
      int length = changeValue.Length;
      for (int index = 0; index < length; ++index)
      {
        float num2 = changeValue[index].magnitude / options.durations[index];
        options.durations[index] = num2;
        num1 += num2;
      }
      return num1;
    }

    public override void EvaluateAndApply(Vector3ArrayOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3[] startValue, Vector3[] changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      Vector3 vector3 = Vector3.zero;
      if (t.loopType == LoopType.Incremental)
      {
        int num = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
        if (num > 0)
        {
          int index = startValue.Length - 1;
          vector3 = (startValue[index] + changeValue[index] - startValue[0]) * (float) num;
        }
      }
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
      {
        int num = (t.loopType == LoopType.Incremental ? t.loops : 1) * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
        if (num > 0)
        {
          int index = startValue.Length - 1;
          vector3 += (startValue[index] + changeValue[index] - startValue[0]) * (float) num;
        }
      }
      int index1 = 0;
      float time = 0.0f;
      float duration1 = 0.0f;
      int length = options.durations.Length;
      float num1 = 0.0f;
      for (int index2 = 0; index2 < length; ++index2)
      {
        duration1 = options.durations[index2];
        num1 += duration1;
        if ((double) elapsed > (double) num1)
        {
          time += duration1;
        }
        else
        {
          index1 = index2;
          time = elapsed - time;
          break;
        }
      }
      float num2 = EaseManager.Evaluate(t.easeType, t.customEase, time, duration1, t.easeOvershootOrAmplitude, t.easePeriod);
      switch (options.axisConstraint)
      {
        case AxisConstraint.X:
          Vector3 pNewValue1 = getter();
          pNewValue1.x = (float) ((double) startValue[index1].x + (double) vector3.x + (double) changeValue[index1].x * (double) num2);
          if (options.snapping)
            pNewValue1.x = (float) Math.Round((double) pNewValue1.x);
          setter(pNewValue1);
          break;
        case AxisConstraint.Y:
          Vector3 pNewValue2 = getter();
          pNewValue2.y = (float) ((double) startValue[index1].y + (double) vector3.y + (double) changeValue[index1].y * (double) num2);
          if (options.snapping)
            pNewValue2.y = (float) Math.Round((double) pNewValue2.y);
          setter(pNewValue2);
          break;
        case AxisConstraint.Z:
          Vector3 pNewValue3 = getter();
          pNewValue3.z = (float) ((double) startValue[index1].z + (double) vector3.z + (double) changeValue[index1].z * (double) num2);
          if (options.snapping)
            pNewValue3.z = (float) Math.Round((double) pNewValue3.z);
          setter(pNewValue3);
          break;
        default:
          Vector3 pNewValue4;
          pNewValue4.x = (float) ((double) startValue[index1].x + (double) vector3.x + (double) changeValue[index1].x * (double) num2);
          pNewValue4.y = (float) ((double) startValue[index1].y + (double) vector3.y + (double) changeValue[index1].y * (double) num2);
          pNewValue4.z = (float) ((double) startValue[index1].z + (double) vector3.z + (double) changeValue[index1].z * (double) num2);
          if (options.snapping)
          {
            pNewValue4.x = (float) Math.Round((double) pNewValue4.x);
            pNewValue4.y = (float) Math.Round((double) pNewValue4.y);
            pNewValue4.z = (float) Math.Round((double) pNewValue4.z);
          }
          setter(pNewValue4);
          break;
      }
    }
  }
}
