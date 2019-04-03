// Decompiled with JetBrains decompiler
// Type: DG.Tweening.EaseFactory
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
  /// <summary>
  /// Allows to wrap ease method in special ways, adding extra features
  /// </summary>
  public class EaseFactory
  {
    /// <summary>
    /// Converts the given ease so that it also creates a stop-motion effect, by playing the tween at the given FPS
    /// </summary>
    /// <param name="motionFps">FPS at which the tween should be played</param>
    /// <param name="ease">Ease type</param>
    public static EaseFunction StopMotion(int motionFps, Ease? ease = null)
    {
      EaseFunction easeFunction = EaseManager.ToEaseFunction(!ease.HasValue ? DOTween.defaultEaseType : ease.Value);
      return EaseFactory.StopMotion(motionFps, easeFunction);
    }

    /// <summary>
    /// Converts the given ease so that it also creates a stop-motion effect, by playing the tween at the given FPS
    /// </summary>
    /// <param name="motionFps">FPS at which the tween should be played</param>
    /// <param name="animCurve">AnimationCurve to use for the ease</param>
    public static EaseFunction StopMotion(int motionFps, AnimationCurve animCurve)
    {
      return EaseFactory.StopMotion(motionFps, new EaseFunction(new EaseCurve(animCurve).Evaluate));
    }

    /// <summary>
    /// Converts the given ease so that it also creates a stop-motion effect, by playing the tween at the given FPS
    /// </summary>
    /// <param name="motionFps">FPS at which the tween should be played</param>
    /// <param name="customEase">Custom ease function to use</param>
    public static EaseFunction StopMotion(int motionFps, EaseFunction customEase)
    {
      float motionDelay = 1f / (float) motionFps;
      return (EaseFunction) ((time, duration, overshootOrAmplitude, period) => customEase((double) time < (double) duration ? time - time % motionDelay : time, duration, overshootOrAmplitude, period));
    }
  }
}
