// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.PathCore.ControlPoint
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
  /// <summary>Path control point</summary>
  [Serializable]
  public struct ControlPoint
  {
    public Vector3 a;
    public Vector3 b;

    public ControlPoint(Vector3 a, Vector3 b)
    {
      this.a = a;
      this.b = b;
    }

    public static ControlPoint operator +(ControlPoint cp, Vector3 v)
    {
      return new ControlPoint(cp.a + v, cp.b + v);
    }
  }
}
