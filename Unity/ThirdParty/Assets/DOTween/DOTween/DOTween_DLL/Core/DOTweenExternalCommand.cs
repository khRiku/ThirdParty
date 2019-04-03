// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.DOTweenExternalCommand
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

namespace DG.Tweening.Core
{
  /// <summary>
  /// Used to dispatch commands that need to be captured externally, usually by Modules
  /// </summary>
  public static class DOTweenExternalCommand
  {
    public static event Action<PathOptions, Tween, Quaternion, Transform> SetOrientationOnPath;

    internal static void Dispatch_SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
    {
      // ISSUE: reference to a compiler-generated field
      if (DOTweenExternalCommand.SetOrientationOnPath == null)
        return;
      // ISSUE: reference to a compiler-generated field
      DOTweenExternalCommand.SetOrientationOnPath(options, t, newRot, trans);
    }
  }
}
