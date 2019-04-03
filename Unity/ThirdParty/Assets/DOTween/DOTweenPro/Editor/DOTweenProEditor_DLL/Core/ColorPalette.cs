// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.Core.ColorPalette
// Assembly: DOTweenProEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1AF96003-A4AA-47A6-9D47-0CF90D290097
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\Editor\DOTweenProEditor.dll

using DG.DemiLib;
using System;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
  [Serializable]
  public class ColorPalette : DeColorPalette
  {
    public ColorPalette.Custom custom = new ColorPalette.Custom();

    /// <summary>Custom colors</summary>
    [Serializable]
    public class Custom
    {
      public DeSkinColor stickyDivider = new DeSkinColor(Color.black, new Color(0.5f, 0.5f, 0.5f));
    }
  }
}
