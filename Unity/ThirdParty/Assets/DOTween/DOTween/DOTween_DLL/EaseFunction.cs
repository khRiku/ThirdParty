// Decompiled with JetBrains decompiler
// Type: DG.Tweening.EaseFunction
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening
{
  /// <summary>
  /// Used for custom and animationCurve-based ease functions. Must return a value between 0 and 1.
  /// </summary>
  public delegate float EaseFunction(float time, float duration, float overshootOrAmplitude, float period);
}
