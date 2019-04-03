// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Options.QuaternionOptions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using UnityEngine;

namespace DG.Tweening.Plugins.Options
{
  public struct QuaternionOptions : IPlugOptions
  {
    public RotateMode rotateMode;
    public AxisConstraint axisConstraint;
    public Vector3 up;

    public void Reset()
    {
      this.rotateMode = RotateMode.Fast;
      this.axisConstraint = AxisConstraint.None;
      this.up = Vector3.zero;
    }
  }
}
