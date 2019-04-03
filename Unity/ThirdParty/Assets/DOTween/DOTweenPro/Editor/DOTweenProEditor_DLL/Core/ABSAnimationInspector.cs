// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.Core.ABSAnimationInspector
// Assembly: DOTweenProEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1AF96003-A4AA-47A6-9D47-0CF90D290097
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\Editor\DOTweenProEditor.dll

using DG.DemiEditor;
using DG.DemiLib;
using UnityEditor;

namespace DG.DOTweenEditor.Core
{
  public class ABSAnimationInspector : Editor
  {
    public static ColorPalette colors = new ColorPalette();
    public static StylePalette styles = new StylePalette();
    public SerializedProperty onStartProperty;
    public SerializedProperty onPlayProperty;
    public SerializedProperty onUpdateProperty;
    public SerializedProperty onStepCompleteProperty;
    public SerializedProperty onCompleteProperty;
    public SerializedProperty onRewindProperty;
    public SerializedProperty onTweenCreatedProperty;

    public override void OnInspectorGUI()
    {
      DeGUI.BeginGUI((DeColorPalette) ABSAnimationInspector.colors, (DeStylePalette) ABSAnimationInspector.styles);
    }
  }
}
