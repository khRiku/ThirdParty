// Decompiled with JetBrains decompiler
// Type: DG.Tweening.AxisConstraint
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System;

namespace DG.Tweening
{
  /// <summary>What axis to constrain in case of Vector tweens</summary>
  [Flags]
  public enum AxisConstraint
  {
    None = 0,
    X = 2,
    Y = 4,
    Z = 8,
    W = 16, // 0x00000010
  }
}
