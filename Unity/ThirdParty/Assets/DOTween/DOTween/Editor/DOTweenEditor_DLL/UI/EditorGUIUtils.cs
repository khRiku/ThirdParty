using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
	// Token: 0x02000009 RID: 9
	public static class EditorGUIUtils
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002F48 File Offset: 0x00001148
		public static Texture2D logo
		{
			get
			{
				if (EditorGUIUtils._logo == null)
				{
					EditorGUIUtils._logo = (AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/DOTweenIcon.png", typeof(Texture2D)) as Texture2D);
					EditorUtils.SetEditorTexture(EditorGUIUtils._logo, 1, 128);
				}
				return EditorGUIUtils._logo;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002FA4 File Offset: 0x000011A4
		public static Ease FilteredEasePopup(Ease currEase)
		{
			int num = (currEase == Ease.INTERNAL_Custom) ? (EditorGUIUtils.FilteredEaseTypes.Length - 1) : Array.IndexOf<string>(EditorGUIUtils.FilteredEaseTypes, currEase.ToString());
			if (num == -1)
			{
				num = 0;
			}
			num = EditorGUILayout.Popup("Ease", num, EditorGUIUtils.FilteredEaseTypes, new GUILayoutOption[0]);
			if (num != EditorGUIUtils.FilteredEaseTypes.Length - 1)
			{
				return (Ease)Enum.Parse(typeof(Ease), EditorGUIUtils.FilteredEaseTypes[num]);
			}
			return Ease.INTERNAL_Custom;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003020 File Offset: 0x00001220
		public static void InspectorLogo()
		{
			GUILayout.Box(EditorGUIUtils.logo, EditorGUIUtils.logoIconStyle, new GUILayoutOption[0]);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003038 File Offset: 0x00001238
		public static bool ToggleButton(bool toggled, GUIContent content, GUIStyle guiStyle = null, params GUILayoutOption[] options)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = (toggled ? Color.green : Color.white);
			if ((guiStyle == null) ? GUILayout.Button(content, options) : GUILayout.Button(content, guiStyle, options))
			{
				toggled = !toggled;
				GUI.changed = true;
			}
			GUI.backgroundColor = backgroundColor;
			return toggled;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003088 File Offset: 0x00001288
		public static void SetGUIStyles(Vector2? footerSize = null)
		{
			if (!EditorGUIUtils._additionalStylesSet && footerSize != null)
			{
				EditorGUIUtils._additionalStylesSet = true;
				Vector2 value = footerSize.Value;
				EditorGUIUtils.btImgStyle = new GUIStyle(GUI.skin.button);
				EditorGUIUtils.btImgStyle.normal.background = null;
				EditorGUIUtils.btImgStyle.imagePosition = 2;
				EditorGUIUtils.btImgStyle.padding = new RectOffset(0, 0, 0, 0);
				EditorGUIUtils.btImgStyle.fixedWidth = value.x;
				EditorGUIUtils.btImgStyle.fixedHeight = value.y;
			}
			if (!EditorGUIUtils._stylesSet)
			{
				EditorGUIUtils._stylesSet = true;
				EditorGUIUtils.boldLabelStyle = new GUIStyle(GUI.skin.label);
				EditorGUIUtils.boldLabelStyle.fontStyle = 1;
				EditorGUIUtils.redLabelStyle = new GUIStyle(GUI.skin.label);
				EditorGUIUtils.redLabelStyle.normal.textColor = Color.red;
				EditorGUIUtils.setupLabelStyle = new GUIStyle(EditorGUIUtils.boldLabelStyle);
				EditorGUIUtils.setupLabelStyle.alignment = 4;
				EditorGUIUtils.wrapCenterLabelStyle = new GUIStyle(GUI.skin.label);
				EditorGUIUtils.wrapCenterLabelStyle.wordWrap = true;
				EditorGUIUtils.wrapCenterLabelStyle.alignment = 4;
				EditorGUIUtils.btBigStyle = new GUIStyle(GUI.skin.button);
				EditorGUIUtils.btBigStyle.padding = new RectOffset(0, 0, 10, 10);
				EditorGUIUtils.btSetup = new GUIStyle(EditorGUIUtils.btBigStyle);
				EditorGUIUtils.btSetup.padding = new RectOffset(36, 36, 6, 6);
				EditorGUIUtils.btSetup.wordWrap = true;
				EditorGUIUtils.btSetup.richText = true;
				EditorGUIUtils.titleStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					fontStyle = 1
				};
				EditorGUIUtils.handlelabelStyle = new GUIStyle(GUI.skin.label)
				{
					normal = 
					{
						textColor = Color.white
					},
					alignment = 3
				};
				EditorGUIUtils.handleSelectedLabelStyle = new GUIStyle(EditorGUIUtils.handlelabelStyle)
				{
					normal = 
					{
						textColor = Color.yellow
					},
					fontStyle = 1
				};
				EditorGUIUtils.wordWrapLabelStyle = new GUIStyle(GUI.skin.label);
				EditorGUIUtils.wordWrapLabelStyle.wordWrap = true;
				EditorGUIUtils.wordWrapItalicLabelStyle = new GUIStyle(EditorGUIUtils.wordWrapLabelStyle);
				EditorGUIUtils.wordWrapItalicLabelStyle.fontStyle = 2;
				EditorGUIUtils.logoIconStyle = new GUIStyle(GUI.skin.box);
				EditorGUIUtils.logoIconStyle.active.background = (EditorGUIUtils.logoIconStyle.normal.background = null);
				EditorGUIUtils.logoIconStyle.margin = new RectOffset(0, 0, 0, 0);
				EditorGUIUtils.logoIconStyle.padding = new RectOffset(0, 0, 0, 0);
				EditorGUIUtils.sideBtStyle = new GUIStyle(GUI.skin.button);
				EditorGUIUtils.sideBtStyle.margin.top = 1;
				EditorGUIUtils.sideBtStyle.padding = new RectOffset(0, 0, 2, 2);
				EditorGUIUtils.sideLogoIconBoldLabelStyle = new GUIStyle(EditorGUIUtils.boldLabelStyle);
				EditorGUIUtils.sideLogoIconBoldLabelStyle.alignment = 3;
				EditorGUIUtils.sideLogoIconBoldLabelStyle.padding.top = 2;
				EditorGUIUtils.wordWrapTextArea = new GUIStyle(GUI.skin.textArea);
				EditorGUIUtils.wordWrapTextArea.wordWrap = true;
				EditorGUIUtils.popupButton = new GUIStyle(EditorStyles.popup);
				EditorGUIUtils.popupButton.fixedHeight = 18f;
				EditorGUIUtils.popupButton.margin.top++;
				EditorGUIUtils.btIconStyle = new GUIStyle(GUI.skin.button);
				EditorGUIUtils.btIconStyle.padding.left -= 2;
				EditorGUIUtils.btIconStyle.fixedWidth = 24f;
				EditorGUIUtils.btIconStyle.stretchWidth = false;
				EditorGUIUtils.infoboxStyle = new GUIStyle(GUI.skin.box)
				{
					alignment = 0,
					richText = true,
					wordWrap = true,
					padding = new RectOffset(5, 5, 5, 6),
					normal = 
					{
						textColor = Color.white,
						background = Texture2D.whiteTexture
					}
				};
			}
		}

		// Token: 0x0400001E RID: 30
		private static bool _stylesSet;

		// Token: 0x0400001F RID: 31
		private static bool _additionalStylesSet;

		// Token: 0x04000020 RID: 32
		public static GUIStyle boldLabelStyle;

		// Token: 0x04000021 RID: 33
		public static GUIStyle setupLabelStyle;

		// Token: 0x04000022 RID: 34
		public static GUIStyle redLabelStyle;

		// Token: 0x04000023 RID: 35
		public static GUIStyle btBigStyle;

		// Token: 0x04000024 RID: 36
		public static GUIStyle btSetup;

		// Token: 0x04000025 RID: 37
		public static GUIStyle btImgStyle;

		// Token: 0x04000026 RID: 38
		public static GUIStyle wrapCenterLabelStyle;

		// Token: 0x04000027 RID: 39
		public static GUIStyle handlelabelStyle;

		// Token: 0x04000028 RID: 40
		public static GUIStyle handleSelectedLabelStyle;

		// Token: 0x04000029 RID: 41
		public static GUIStyle wordWrapLabelStyle;

		// Token: 0x0400002A RID: 42
		public static GUIStyle wordWrapItalicLabelStyle;

		// Token: 0x0400002B RID: 43
		public static GUIStyle titleStyle;

		// Token: 0x0400002C RID: 44
		public static GUIStyle logoIconStyle;

		// Token: 0x0400002D RID: 45
		public static GUIStyle sideBtStyle;

		// Token: 0x0400002E RID: 46
		public static GUIStyle sideLogoIconBoldLabelStyle;

		// Token: 0x0400002F RID: 47
		public static GUIStyle wordWrapTextArea;

		// Token: 0x04000030 RID: 48
		public static GUIStyle popupButton;

		// Token: 0x04000031 RID: 49
		public static GUIStyle btIconStyle;

		// Token: 0x04000032 RID: 50
		public static GUIStyle infoboxStyle;

		// Token: 0x04000033 RID: 51
		private static Texture2D _logo;

		// Token: 0x04000034 RID: 52
		internal static readonly string[] FilteredEaseTypes = new string[]
		{
			"Linear",
			"InSine",
			"OutSine",
			"InOutSine",
			"InQuad",
			"OutQuad",
			"InOutQuad",
			"InCubic",
			"OutCubic",
			"InOutCubic",
			"InQuart",
			"OutQuart",
			"InOutQuart",
			"InQuint",
			"OutQuint",
			"InOutQuint",
			"InExpo",
			"OutExpo",
			"InOutExpo",
			"InCirc",
			"OutCirc",
			"InOutCirc",
			"InElastic",
			"OutElastic",
			"InOutElastic",
			"InBack",
			"OutBack",
			"InOutBack",
			"InBounce",
			"OutBounce",
			"InOutBounce",
			":: AnimationCurve"
		};
	}
}
