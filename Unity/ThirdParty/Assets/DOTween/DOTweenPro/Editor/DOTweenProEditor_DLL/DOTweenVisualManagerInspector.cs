// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.DOTweenVisualManagerInspector
// Assembly: DOTweenProEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1AF96003-A4AA-47A6-9D47-0CF90D290097
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\Editor\DOTweenProEditor.dll

using DG.DOTweenEditor.UI;
using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DG.DOTweenEditor
{
  [CustomEditor(typeof (DOTweenVisualManager))]
  public class DOTweenVisualManagerInspector : Editor
  {
    private DOTweenVisualManager _src;

    private void OnEnable()
    {
      this._src = this.target as DOTweenVisualManager;
      if (Application.isPlaying)
        return;
      MonoBehaviour[] components = this._src.GetComponents<MonoBehaviour>();
      int num1 = ArrayUtility.IndexOf<MonoBehaviour>(components, (MonoBehaviour) this._src);
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        if (components[index] is ABSAnimationComponent)
          ++num2;
      }
      while (num2 > 0)
      {
        --num2;
        ComponentUtility.MoveComponentUp((Component) this._src);
      }
    }

    public override void OnInspectorGUI()
    {
      EditorGUIUtils.SetGUIStyles(new Vector2?());
      EditorGUIUtility.labelWidth = 80f;
      EditorGUIUtils.InspectorLogo();
      VisualManagerPreset preset = this._src.preset;
      this._src.preset = (VisualManagerPreset) EditorGUILayout.EnumPopup("Preset", (Enum) this._src.preset, new GUILayoutOption[0]);
      if (preset != this._src.preset && this._src.preset == VisualManagerPreset.PoolingSystem)
      {
        this._src.onEnableBehaviour = OnEnableBehaviour.RestartFromSpawnPoint;
        this._src.onDisableBehaviour = OnDisableBehaviour.Rewind;
      }
      GUILayout.Space(6f);
      bool flag = (uint) this._src.preset > 0U;
      OnEnableBehaviour onEnableBehaviour = this._src.onEnableBehaviour;
      OnDisableBehaviour disableBehaviour = this._src.onDisableBehaviour;
      this._src.onEnableBehaviour = (OnEnableBehaviour) EditorGUILayout.EnumPopup(new GUIContent("On Enable", "Eventual actions to perform when this gameObject is activated"), (Enum) this._src.onEnableBehaviour, new GUILayoutOption[0]);
      this._src.onDisableBehaviour = (OnDisableBehaviour) EditorGUILayout.EnumPopup(new GUIContent("On Disable", "Eventual actions to perform when this gameObject is deactivated"), (Enum) this._src.onDisableBehaviour, new GUILayoutOption[0]);
      if (flag && onEnableBehaviour != this._src.onEnableBehaviour || disableBehaviour != this._src.onDisableBehaviour)
        this._src.preset = VisualManagerPreset.Custom;
      if (!GUI.changed)
        return;
      EditorUtility.SetDirty((UnityEngine.Object) this._src);
    }
  }
}
