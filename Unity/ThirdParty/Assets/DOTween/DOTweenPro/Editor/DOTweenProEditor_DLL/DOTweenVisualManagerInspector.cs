﻿using System;
using DG.DOTweenEditor.UI;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DG.DOTweenEditor
{
	// Token: 0x02000005 RID: 5
	[CustomEditor(typeof(DOTweenVisualManager))]
	public class DOTweenVisualManagerInspector : Editor
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00003D98 File Offset: 0x00001F98
		private void OnEnable()
		{
			this._src = (base.target as DOTweenVisualManager);
			if (Application.isPlaying)
			{
				return;
			}
			MonoBehaviour[] components = this._src.GetComponents<MonoBehaviour>();
			int num = ArrayUtility.IndexOf<MonoBehaviour>(components, this._src);
			int i = 0;
			for (int j = 0; j < num; j++)
			{
				if (components[j] is ABSAnimationComponent)
				{
					i++;
				}
			}
			while (i > 0)
			{
				i--;
				ComponentUtility.MoveComponentUp(this._src);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003E0C File Offset: 0x0000200C
		public override void OnInspectorGUI()
		{
			EditorGUIUtils.SetGUIStyles(null);
			EditorGUIUtility.labelWidth = 80f;
			EditorGUIUtils.InspectorLogo();
			VisualManagerPreset preset = this._src.preset;
			this._src.preset = (VisualManagerPreset)EditorGUILayout.EnumPopup("Preset", this._src.preset, new GUILayoutOption[0]);
			if (preset != this._src.preset)
			{
				VisualManagerPreset preset2 = this._src.preset;
				if (preset2 == VisualManagerPreset.PoolingSystem)
				{
					this._src.onEnableBehaviour = OnEnableBehaviour.RestartFromSpawnPoint;
					this._src.onDisableBehaviour = OnDisableBehaviour.Rewind;
				}
			}
			GUILayout.Space(6f);
			bool flag = this._src.preset > VisualManagerPreset.Custom;
			OnEnableBehaviour onEnableBehaviour = this._src.onEnableBehaviour;
			OnDisableBehaviour onDisableBehaviour = this._src.onDisableBehaviour;
			this._src.onEnableBehaviour = (OnEnableBehaviour)EditorGUILayout.EnumPopup(new GUIContent("On Enable", "Eventual actions to perform when this gameObject is activated"), this._src.onEnableBehaviour, new GUILayoutOption[0]);
			this._src.onDisableBehaviour = (OnDisableBehaviour)EditorGUILayout.EnumPopup(new GUIContent("On Disable", "Eventual actions to perform when this gameObject is deactivated"), this._src.onDisableBehaviour, new GUILayoutOption[0]);
			if ((flag && onEnableBehaviour != this._src.onEnableBehaviour) || onDisableBehaviour != this._src.onDisableBehaviour)
			{
				this._src.preset = VisualManagerPreset.Custom;
			}
			if (GUI.changed)
			{
				EditorUtility.SetDirty(this._src);
			}
		}

		// Token: 0x0400001E RID: 30
		private DOTweenVisualManager _src;
	}
}
