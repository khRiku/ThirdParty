// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Color2
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using UnityEngine;

namespace DG.Tweening
{
  /// <summary>
  /// Struct that stores two colors (used for LineRenderer tweens)
  /// </summary>
  public struct Color2
  {
    public Color ca;
    public Color cb;

    public Color2(Color ca, Color cb)
    {
      this.ca = ca;
      this.cb = cb;
    }

    public static Color2 operator +(Color2 c1, Color2 c2)
    {
      return new Color2(c1.ca + c2.ca, c1.cb + c2.cb);
    }

    public static Color2 operator -(Color2 c1, Color2 c2)
    {
      return new Color2(c1.ca - c2.ca, c1.cb - c2.cb);
    }

    public static Color2 operator *(Color2 c1, float f)
    {
      return new Color2(c1.ca * f, c1.cb * f);
    }
  }
}
