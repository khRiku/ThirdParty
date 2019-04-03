// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.Easing.EaseCurve
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using UnityEngine;

namespace DG.Tweening.Core.Easing
{
  /// <summary>
  /// Used to interpret AnimationCurves as eases.
  /// Public so it can be used by external ease factories
  /// </summary>
  public class EaseCurve
  {
    private readonly AnimationCurve _animCurve;

    public EaseCurve(AnimationCurve animCurve)
    {
      this._animCurve = animCurve;
    }

    public float Evaluate(float time, float duration, float unusedOvershoot, float unusedPeriod)
    {
      float time1 = this._animCurve[this._animCurve.length - 1].time;
      return this._animCurve.Evaluate(time / duration * time1);
    }
  }
}
