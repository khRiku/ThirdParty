// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.Core.StylePalette
// Assembly: DOTweenProEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1AF96003-A4AA-47A6-9D47-0CF90D290097
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\Editor\DOTweenProEditor.dll

using DG.DemiEditor;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
  public class StylePalette : DeStylePalette
  {
    public readonly StylePalette.Custom custom = new StylePalette.Custom();

    public class Custom : DeStyleSubPalette
    {
      public GUIStyle stickyToolbar;
      public GUIStyle stickyTitle;
      public GUIStyle warningLabel;

      /// <summary>
      /// Needs to be overridden in order to initialize new styles added from inherited classes
      /// </summary>
      public override void Init()
      {
        this.stickyToolbar = new GUIStyle(DeGUI.styles.toolbar.flat);
        this.stickyTitle = new GUIStyle(GUI.skin.label).Clone((object) FontStyle.Bold, (object) 11).MarginBottom(0).ContentOffsetX(-2f);
        this.warningLabel = new GUIStyle(GUI.skin.label).Add((object) Color.black, (object) Format.RichText).Background(DeStylePalette.orangeSquare, (Texture2D) null);
      }
    }
  }
}
