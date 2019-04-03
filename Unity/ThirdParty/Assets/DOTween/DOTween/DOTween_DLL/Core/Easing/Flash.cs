// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.Easing.Flash
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System;
using UnityEngine;

namespace DG.Tweening.Core.Easing
{
  public static class Flash
  {
    public static float Ease(float time, float duration, float overshootOrAmplitude, float period)
    {
      int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
      float stepDuration = duration / overshootOrAmplitude;
      time -= stepDuration * (float) (stepIndex - 1);
      float dir = stepIndex % 2 != 0 ? 1f : -1f;
      if ((double) dir < 0.0)
        time -= stepDuration;
      float res = time * dir / stepDuration;
      return Flash.WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
    }

    public static float EaseIn(float time, float duration, float overshootOrAmplitude, float period)
    {
      int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
      float stepDuration = duration / overshootOrAmplitude;
      time -= stepDuration * (float) (stepIndex - 1);
      float dir = stepIndex % 2 != 0 ? 1f : -1f;
      if ((double) dir < 0.0)
        time -= stepDuration;
      time *= dir;
      float res = (time /= stepDuration) * time;
      return Flash.WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
    }

    public static float EaseOut(float time, float duration, float overshootOrAmplitude, float period)
    {
      int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
      float stepDuration = duration / overshootOrAmplitude;
      time -= stepDuration * (float) (stepIndex - 1);
      float dir = stepIndex % 2 != 0 ? 1f : -1f;
      if ((double) dir < 0.0)
        time -= stepDuration;
      time *= dir;
      float res = (float) (-(double) (time /= stepDuration) * ((double) time - 2.0));
      return Flash.WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
    }

    public static float EaseInOut(float time, float duration, float overshootOrAmplitude, float period)
    {
      int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
      float stepDuration = duration / overshootOrAmplitude;
      time -= stepDuration * (float) (stepIndex - 1);
      float dir = stepIndex % 2 != 0 ? 1f : -1f;
      if ((double) dir < 0.0)
        time -= stepDuration;
      time *= dir;
      float res = (double) (time /= stepDuration * 0.5f) < 1.0 ? 0.5f * time * time : (float) (-0.5 * ((double) --time * ((double) time - 2.0) - 1.0));
      return Flash.WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
    }

    private static float WeightedEase(float overshootOrAmplitude, float period, int stepIndex, float stepDuration, float dir, float res)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      if ((double) dir > 0.0 && (int) overshootOrAmplitude % 2 == 0)
        ++stepIndex;
      else if ((double) dir < 0.0 && (int) overshootOrAmplitude % 2 != 0)
        ++stepIndex;
      if ((double) period > 0.0)
      {
        float num3 = (float) Math.Truncate((double) overshootOrAmplitude);
        float num4 = overshootOrAmplitude - num3;
        if ((double) num3 % 2.0 > 0.0)
          num4 = 1f - num4;
        num2 = num4 * (float) stepIndex / overshootOrAmplitude;
        num1 = res * (overshootOrAmplitude - (float) stepIndex) / overshootOrAmplitude;
      }
      else if ((double) period < 0.0)
      {
        period = -period;
        num1 = res * (float) stepIndex / overshootOrAmplitude;
      }
      float num5 = num1 - res;
      res += num5 * period + num2;
      if ((double) res > 1.0)
        res = 1f;
      return res;
    }
  }
}
