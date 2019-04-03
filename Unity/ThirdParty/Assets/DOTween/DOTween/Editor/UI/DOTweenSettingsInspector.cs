// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.UI.DOTweenSettingsInspector
// Assembly: DOTweenEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5921EEE1-1EAD-4AE3-AEC3-8051606D5E53
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\Editor\DOTweenEditor.dll

using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
  [CustomEditor(typeof (DOTweenSettings))]
  public class DOTweenSettingsInspector : Editor
  {
    private DOTweenSettings _src;

    private void OnEnable()
    {
      this._src = this.target as DOTweenSettings;
    }

    public override void OnInspectorGUI()
    {
      GUI.enabled = false;
      this.DrawDefaultInspector();
      GUI.enabled = true;
    }
  }
}
