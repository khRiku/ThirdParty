﻿// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Options.UintOptions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening.Plugins.Options
{
  public struct UintOptions : IPlugOptions
  {
    public bool isNegativeChangeValue;

    public void Reset()
    {
      this.isNegativeChangeValue = false;
    }
  }
}
