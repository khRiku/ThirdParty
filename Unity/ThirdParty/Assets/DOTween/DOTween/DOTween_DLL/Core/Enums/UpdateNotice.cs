// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.Enums.UpdateNotice
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening.Core.Enums
{
  /// <summary>
  /// Additional notices passed to plugins when updating.
  /// Public so it can be used by custom plugins. Internally, only PathPlugin uses it
  /// </summary>
  public enum UpdateNotice
  {
    None,
    RewindStep,
  }
}
