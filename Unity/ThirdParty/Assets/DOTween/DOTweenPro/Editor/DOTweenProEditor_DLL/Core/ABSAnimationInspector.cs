using System;
using DG.DemiEditor;
using UnityEditor;

namespace DG.DOTweenEditor.Core
{
	// Token: 0x02000006 RID: 6
	public class ABSAnimationInspector : Editor
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00003F95 File Offset: 0x00002195
		public override void OnInspectorGUI()
		{
			DeGUI.BeginGUI(ABSAnimationInspector.colors, ABSAnimationInspector.styles);
		}

		// Token: 0x0400001F RID: 31
		public static ColorPalette colors = new ColorPalette();

		// Token: 0x04000020 RID: 32
		public static StylePalette styles = new StylePalette();

		// Token: 0x04000021 RID: 33
		public SerializedProperty onStartProperty;

		// Token: 0x04000022 RID: 34
		public SerializedProperty onPlayProperty;

		// Token: 0x04000023 RID: 35
		public SerializedProperty onUpdateProperty;

		// Token: 0x04000024 RID: 36
		public SerializedProperty onStepCompleteProperty;

		// Token: 0x04000025 RID: 37
		public SerializedProperty onCompleteProperty;

		// Token: 0x04000026 RID: 38
		public SerializedProperty onRewindProperty;

		// Token: 0x04000027 RID: 39
		public SerializedProperty onTweenCreatedProperty;
	}
}
