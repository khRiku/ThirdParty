using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenUpgradeManager
{
	// Token: 0x02000003 RID: 3
	internal class UpgradeWindow : EditorWindow
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020C5 File Offset: 0x000002C5
		public static void Open()
		{
			UpgradeWindow window = EditorWindow.GetWindow<UpgradeWindow>(true, "New Version of DOTween Imported", true);
			window.minSize = UpgradeWindow._WinSize;
			window.maxSize = UpgradeWindow._WinSize;
			window.ShowUtility();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F0 File Offset: 0x000002F0
		private void OnGUI()
		{
			UpgradeWindow.Styles.Init();
			Rect rect = new Rect(0f, 0f, base.position.width, base.position.height);
			GUI.color = new Color(0.18f, 0.18f, 0.18f);
			GUI.DrawTexture(rect, Texture2D.whiteTexture);
			GUI.color = Color.white;
			GUILayout.Space(4f);
			GUILayout.Label("DOTWEEN SETUP REQUIRED", UpgradeWindow.Styles.descrTitle, new GUILayoutOption[0]);
			GUILayout.Label("Select <color=#ffc47a><b>\"Setup DOTween...\"</b></color> in <b>DOTween's Utility Panel</b> to set it up and add/remove Modules.", UpgradeWindow.Styles.descrLabel, new GUILayoutOption[0]);
			GUILayout.Space(12f);
			GUILayout.Label("IMPORTANT IN CASE OF UPGRADE", UpgradeWindow.Styles.descrTitle, new GUILayoutOption[0]);
			GUILayout.Label("If you're upgrading from a DOTween version older than <b>1.2.000</b> or <b>Pro older than 1.0.000</b> (<color=#ffc47a><i>before the introduction of DOTween Modules</i></color>) you will see lots of errors. <b>Follow these instructions</b> to fix them:", UpgradeWindow.Styles.descrLabel, new GUILayoutOption[0]);
			GUILayout.Space(-15f);
			GUILayout.Label("\n<color=#94de59><b>1)</b></color> <color=#ffc47a><b>Close and reopen the project</b></color> (if you haven't already done so)\n<color=#94de59><b>2)</b></color> Open DOTween's Utility Panel and <color=#ffc47a><b>run the Setup</b></color> to activate required Modules", UpgradeWindow.Styles.descrLabel, new GUILayoutOption[0]);
			GUILayout.Space(12f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Open DOTween Utility Panel", new GUILayoutOption[]
			{
				GUILayout.Height(30f)
			}))
			{
				Type type = Type.GetType("DG.DOTweenEditor.UI.DOTweenUtilityWindow, DOTweenEditor");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("Open", BindingFlags.Static | BindingFlags.Public);
					if (method != null)
					{
						method.Invoke(null, null);
					}
				}
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(Autorun.OnUpdate));
				base.Close();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x04000001 RID: 1
		private const string _Title = "New Version of DOTween Imported";

		// Token: 0x04000002 RID: 2
		private static readonly Vector2 _WinSize = new Vector2(400f, 280f);

		// Token: 0x04000003 RID: 3
		private const string _DescrTitle0 = "DOTWEEN SETUP REQUIRED";

		// Token: 0x04000004 RID: 4
		private const string _DescrContent0 = "Select <color=#ffc47a><b>\"Setup DOTween...\"</b></color> in <b>DOTween's Utility Panel</b> to set it up and add/remove Modules.";

		// Token: 0x04000005 RID: 5
		private const string _DescrTitle1 = "IMPORTANT IN CASE OF UPGRADE";

		// Token: 0x04000006 RID: 6
		private const string _DescrContent1 = "If you're upgrading from a DOTween version older than <b>1.2.000</b> or <b>Pro older than 1.0.000</b> (<color=#ffc47a><i>before the introduction of DOTween Modules</i></color>) you will see lots of errors. <b>Follow these instructions</b> to fix them:";

		// Token: 0x04000007 RID: 7
		private const string _DescrContent2 = "\n<color=#94de59><b>1)</b></color> <color=#ffc47a><b>Close and reopen the project</b></color> (if you haven't already done so)\n<color=#94de59><b>2)</b></color> Open DOTween's Utility Panel and <color=#ffc47a><b>run the Setup</b></color> to activate required Modules";

		// Token: 0x02000004 RID: 4
		private static class Styles
		{
			// Token: 0x06000009 RID: 9 RVA: 0x00002290 File Offset: 0x00000490
			public static void Init()
			{
				if (UpgradeWindow.Styles._initialized)
				{
					return;
				}
				UpgradeWindow.Styles._initialized = true;
				UpgradeWindow.Styles.descrTitle = new GUIStyle(GUI.skin.label);
				UpgradeWindow.Styles.descrTitle.richText = true;
				UpgradeWindow.Styles.descrTitle.fontSize = 18;
				UpgradeWindow.Styles.SetTextColor(UpgradeWindow.Styles.descrTitle, new Color(0.58f, 0.87f, 0.35f));
				UpgradeWindow.Styles.descrLabel = new GUIStyle(GUI.skin.label);
				UpgradeWindow.Styles.descrLabel.fontSize = 12;
				UpgradeWindow.Styles.descrLabel.wordWrap = (UpgradeWindow.Styles.descrLabel.richText = true);
				UpgradeWindow.Styles.SetTextColor(UpgradeWindow.Styles.descrLabel, new Color(0.93f, 0.93f, 0.93f));
			}

			// Token: 0x0600000A RID: 10 RVA: 0x0000234C File Offset: 0x0000054C
			private static void SetTextColor(GUIStyle style, Color color)
			{
				GUIStyleState normal = style.normal;
				GUIStyleState active = style.active;
				GUIStyleState focused = style.focused;
				GUIStyleState hover = style.hover;
				GUIStyleState onNormal = style.onNormal;
				GUIStyleState onActive = style.onActive;
				GUIStyleState onFocused = style.onFocused;
				style.onHover.textColor = color;
				onFocused.textColor = color;
				onActive.textColor = color;
				onNormal.textColor = color;
				hover.textColor = color;
				focused.textColor = color;
				active.textColor = color;
				normal.textColor = color;
			}

			// Token: 0x04000008 RID: 8
			private static bool _initialized;

			// Token: 0x04000009 RID: 9
			public static GUIStyle descrTitle;

			// Token: 0x0400000A RID: 10
			public static GUIStyle descrLabel;
		}
	}
}
