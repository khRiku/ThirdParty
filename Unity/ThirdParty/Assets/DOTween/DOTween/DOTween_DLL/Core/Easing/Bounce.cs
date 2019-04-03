// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.Easing.Bounce
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening.Core.Easing
{
  /// <summary>
  /// This class contains a C# port of the easing equations created by Robert Penner (http://robertpenner.com/easing).
  /// </summary>
  public static class Bounce
  {
    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: accelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time (in frames or seconds).</param>
    /// <param name="duration">
    /// Expected easing duration (in frames or seconds).
    /// </param>
    /// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
    /// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
    /// <returns>The eased value.</returns>
    public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
    {
      return 1f - Bounce.EaseOut(duration - time, duration, -1f, -1f);
    }

    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: decelerating from zero velocity.
    /// </summary>
    /// <param name="time">Current time (in frames or seconds).</param>
    /// <param name="duration">
    /// Expected easing duration (in frames or seconds).
    /// </param>
    /// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
    /// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
    /// <returns>The eased value.</returns>
    public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
    {
      if ((double) (time /= duration) < 0.363636374473572)
        return 121f / 16f * time * time;
      if ((double) time < 0.727272748947144)
        return (float) (121.0 / 16.0 * (double) (time -= 0.5454546f) * (double) time + 0.75);
      if ((double) time < 0.909090936183929)
        return (float) (121.0 / 16.0 * (double) (time -= 0.8181818f) * (double) time + 15.0 / 16.0);
      return (float) (121.0 / 16.0 * (double) (time -= 0.9545454f) * (double) time + 63.0 / 64.0);
    }

    /// <summary>
    /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: acceleration until halfway, then deceleration.
    /// </summary>
    /// <param name="time">Current time (in frames or seconds).</param>
    /// <param name="duration">
    /// Expected easing duration (in frames or seconds).
    /// </param>
    /// <param name="unusedOvershootOrAmplitude">Unused: here to keep same delegate for all ease types.</param>
    /// <param name="unusedPeriod">Unused: here to keep same delegate for all ease types.</param>
    /// <returns>The eased value.</returns>
    public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
    {
      if ((double) time < (double) duration * 0.5)
        return Bounce.EaseIn(time * 2f, duration, -1f, -1f) * 0.5f;
      return (float) ((double) Bounce.EaseOut(time * 2f - duration, duration, -1f, -1f) * 0.5 + 0.5);
    }
  }
}
