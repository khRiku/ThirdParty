// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.QuaternionPlugin
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
  public class QuaternionPlugin : ABSTweenPlugin<Quaternion, Vector3, QuaternionOptions>
  {
    public override void Reset(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
    {
    }

    public override void SetFrom(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool isRelative)
    {
      Vector3 endValue = t.endValue;
      t.endValue = t.getter().eulerAngles;
      if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative)
        t.startValue = endValue;
      else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360)
      {
        t.startValue = t.endValue + endValue;
      }
      else
      {
        Quaternion rotation = t.getter();
        t.startValue = t.plugOptions.rotateMode != RotateMode.WorldAxisAdd ? (rotation * Quaternion.Euler(endValue)).eulerAngles : (rotation * Quaternion.Inverse(rotation) * Quaternion.Euler(endValue) * rotation).eulerAngles;
        t.endValue = -endValue;
      }
      t.setter(Quaternion.Euler(t.startValue));
    }

    public override Vector3 ConvertToStartValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Quaternion value)
    {
      return value.eulerAngles;
    }

    public override void SetRelativeEndValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
    {
      t.endValue += t.startValue;
    }

    public override void SetChangeValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
    {
      if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative)
      {
        Vector3 endValue = t.endValue;
        if ((double) endValue.x > 360.0)
          endValue.x %= 360f;
        if ((double) endValue.y > 360.0)
          endValue.y %= 360f;
        if ((double) endValue.z > 360.0)
          endValue.z %= 360f;
        Vector3 vector3 = endValue - t.startValue;
        float num1 = (double) vector3.x > 0.0 ? vector3.x : -vector3.x;
        if ((double) num1 > 180.0)
          vector3.x = (double) vector3.x > 0.0 ? (float) -(360.0 - (double) num1) : 360f - num1;
        float num2 = (double) vector3.y > 0.0 ? vector3.y : -vector3.y;
        if ((double) num2 > 180.0)
          vector3.y = (double) vector3.y > 0.0 ? (float) -(360.0 - (double) num2) : 360f - num2;
        float num3 = (double) vector3.z > 0.0 ? vector3.z : -vector3.z;
        if ((double) num3 > 180.0)
          vector3.z = (double) vector3.z > 0.0 ? (float) -(360.0 - (double) num3) : 360f - num3;
        t.changeValue = vector3;
      }
      else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360 || t.isRelative)
        t.changeValue = t.endValue - t.startValue;
      else
        t.changeValue = t.endValue;
    }

    public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3 changeValue)
    {
      return changeValue.magnitude / unitsXSecond;
    }

    public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      Vector3 euler = startValue;
      if (t.loopType == LoopType.Incremental)
        euler += changeValue * (t.isComplete ? (float) (t.completedLoops - 1) : (float) t.completedLoops);
      if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
        euler += changeValue * (t.loopType == LoopType.Incremental ? (float) t.loops : 1f) * (t.sequenceParent.isComplete ? (float) (t.sequenceParent.completedLoops - 1) : (float) t.sequenceParent.completedLoops);
      float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      switch (options.rotateMode)
      {
        case RotateMode.WorldAxisAdd:
        case RotateMode.LocalAxisAdd:
          Quaternion rotation = Quaternion.Euler(startValue);
          euler.x = changeValue.x * num;
          euler.y = changeValue.y * num;
          euler.z = changeValue.z * num;
          if (options.rotateMode == RotateMode.WorldAxisAdd)
          {
            setter(rotation * Quaternion.Inverse(rotation) * Quaternion.Euler(euler) * rotation);
            break;
          }
          setter(rotation * Quaternion.Euler(euler));
          break;
        default:
          euler.x += changeValue.x * num;
          euler.y += changeValue.y * num;
          euler.z += changeValue.z * num;
          setter(Quaternion.Euler(euler));
          break;
      }
    }
  }
}
