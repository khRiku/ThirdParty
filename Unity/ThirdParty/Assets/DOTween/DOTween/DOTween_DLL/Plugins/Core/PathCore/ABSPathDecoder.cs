// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.PathCore.ABSPathDecoder
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
  internal abstract class ABSPathDecoder
  {
    internal abstract void FinalizePath(Path p, Vector3[] wps, bool isClosedPath);

    internal abstract Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints);
  }
}
