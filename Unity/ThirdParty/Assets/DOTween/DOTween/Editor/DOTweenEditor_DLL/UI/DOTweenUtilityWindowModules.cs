using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
	// Token: 0x0200000B RID: 11
	public static class DOTweenUtilityWindowModules
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00003EF0 File Offset: 0x000020F0
		static DOTweenUtilityWindowModules()
		{
			for (int i = 0; i < DOTweenUtilityWindowModules._ModuleDependentFiles.Length; i++)
			{
				DOTweenUtilityWindowModules._ModuleDependentFiles[i] = DOTweenUtilityWindowModules._ModuleDependentFiles[i].Replace("DOTWEENDIR/", EditorUtils.dotweenDir);
				DOTweenUtilityWindowModules._ModuleDependentFiles[i] = DOTweenUtilityWindowModules._ModuleDependentFiles[i].Replace("DOTWEENPRODIR/", EditorUtils.dotweenProDir);
			}
			DOTweenUtilityWindowModules._audioModule.filePath = EditorUtils.dotweenDir + DOTweenUtilityWindowModules._audioModule.filePath;
			DOTweenUtilityWindowModules._physicsModule.filePath = EditorUtils.dotweenDir + DOTweenUtilityWindowModules._physicsModule.filePath;
			DOTweenUtilityWindowModules._physics2DModule.filePath = EditorUtils.dotweenDir + DOTweenUtilityWindowModules._physics2DModule.filePath;
			DOTweenUtilityWindowModules._spriteModule.filePath = EditorUtils.dotweenDir + DOTweenUtilityWindowModules._spriteModule.filePath;
			DOTweenUtilityWindowModules._uiModule.filePath = EditorUtils.dotweenDir + DOTweenUtilityWindowModules._uiModule.filePath;
			DOTweenUtilityWindowModules._textMeshProModule.filePath = EditorUtils.dotweenProDir + DOTweenUtilityWindowModules._textMeshProModule.filePath;
			DOTweenUtilityWindowModules._tk2DModule.filePath = EditorUtils.dotweenProDir + DOTweenUtilityWindowModules._tk2DModule.filePath;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000040DC File Offset: 0x000022DC
		public static bool Draw(EditorWindow editor, DOTweenSettings src)
		{
			DOTweenUtilityWindowModules._editor = editor;
			DOTweenUtilityWindowModules._src = src;
			if (!DOTweenUtilityWindowModules._refreshed)
			{
				DOTweenUtilityWindowModules.Refresh(DOTweenUtilityWindowModules._src, false);
			}
			GUILayout.Label("Add/Remove Modules", EditorGUIUtils.titleStyle, new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
			GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
			GUILayout.Label("Unity", EditorGUIUtils.boldLabelStyle, new GUILayoutOption[0]);
			DOTweenUtilityWindowModules._audioModule.enabled = EditorGUILayout.Toggle("Audio", DOTweenUtilityWindowModules._audioModule.enabled, new GUILayoutOption[0]);
			DOTweenUtilityWindowModules._physicsModule.enabled = EditorGUILayout.Toggle("Physics", DOTweenUtilityWindowModules._physicsModule.enabled, new GUILayoutOption[0]);
			DOTweenUtilityWindowModules._physics2DModule.enabled = EditorGUILayout.Toggle("Physics2D", DOTweenUtilityWindowModules._physics2DModule.enabled, new GUILayoutOption[0]);
			DOTweenUtilityWindowModules._spriteModule.enabled = EditorGUILayout.Toggle("Sprites", DOTweenUtilityWindowModules._spriteModule.enabled, new GUILayoutOption[0]);
			DOTweenUtilityWindowModules._uiModule.enabled = EditorGUILayout.Toggle("UI", DOTweenUtilityWindowModules._uiModule.enabled, new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			if (EditorUtils.hasPro)
			{
				GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
				GUILayout.Label("External Assets (Pro)", EditorGUIUtils.boldLabelStyle, new GUILayoutOption[0]);
				DOTweenUtilityWindowModules._textMeshProModule.enabled = EditorGUILayout.Toggle("TextMesh Pro", DOTweenUtilityWindowModules._textMeshProModule.enabled, new GUILayoutOption[0]);
				DOTweenUtilityWindowModules._tk2DModule.enabled = EditorGUILayout.Toggle("2D Toolkit", DOTweenUtilityWindowModules._tk2DModule.enabled, new GUILayoutOption[0]);
				EditorGUILayout.EndVertical();
			}
			GUILayout.Space(2f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Apply", new GUILayoutOption[0]))
			{
				DOTweenUtilityWindowModules.Apply();
				DOTweenUtilityWindowModules.Refresh(DOTweenUtilityWindowModules._src, false);
				return true;
			}
			if (GUILayout.Button("Cancel", new GUILayoutOption[0]))
			{
				return true;
			}
			GUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			GUILayout.EndVertical();
			if (EditorApplication.isCompiling)
			{
				DOTweenUtilityWindowModules.WaitForCompilation();
			}
			return false;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000042FB File Offset: 0x000024FB
		private static void WaitForCompilation()
		{
			if (!DOTweenUtilityWindowModules._isWaitingForCompilation)
			{
				DOTweenUtilityWindowModules._isWaitingForCompilation = true;
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(DOTweenUtilityWindowModules.WaitForCompilation_Update));
				DOTweenUtilityWindowModules.WaitForCompilation_Update();
			}
			EditorGUILayout.HelpBox("Waiting for Unity to finish the compilation process...", 1);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000433C File Offset: 0x0000253C
		private static void WaitForCompilation_Update()
		{
			if (!EditorApplication.isCompiling)
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(DOTweenUtilityWindowModules.WaitForCompilation_Update));
				DOTweenUtilityWindowModules._isWaitingForCompilation = false;
				DOTweenUtilityWindowModules.Refresh(DOTweenUtilityWindowModules._src, false);
			}
			DOTweenUtilityWindowModules._editor.Repaint();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000438C File Offset: 0x0000258C
		public static void ApplyModulesSettings()
		{
			DOTweenSettings dotweenSettings = DOTweenUtilityWindow.GetDOTweenSettings();
			if (dotweenSettings != null)
			{
				DOTweenUtilityWindowModules.Refresh(dotweenSettings, true);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000043B0 File Offset: 0x000025B0
		public static void Refresh(DOTweenSettings src, bool applySrcSettings = false)
		{
			DOTweenUtilityWindowModules._src = src;
			DOTweenUtilityWindowModules._refreshed = true;
			AssetDatabase.StartAssetEditing();
			DOTweenUtilityWindowModules._audioModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._audioModule);
			DOTweenUtilityWindowModules._physicsModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._physicsModule);
			DOTweenUtilityWindowModules._physics2DModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._physics2DModule);
			DOTweenUtilityWindowModules._spriteModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._spriteModule);
			DOTweenUtilityWindowModules._uiModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._uiModule);
			DOTweenUtilityWindowModules._textMeshProModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._textMeshProModule);
			DOTweenUtilityWindowModules._tk2DModule.enabled = DOTweenUtilityWindowModules.ModuleIsEnabled(DOTweenUtilityWindowModules._tk2DModule);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._audioModule, ref src.modules.audioEnabled);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._physicsModule, ref src.modules.physicsEnabled);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._physics2DModule, ref src.modules.physics2DEnabled);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._spriteModule, ref src.modules.spriteEnabled);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._uiModule, ref src.modules.uiEnabled);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._textMeshProModule, ref src.modules.textMeshProEnabled);
			DOTweenUtilityWindowModules.CheckAutoModuleSettings(applySrcSettings, DOTweenUtilityWindowModules._tk2DModule, ref src.modules.tk2DEnabled);
			AssetDatabase.StopAssetEditing();
			EditorUtility.SetDirty(DOTweenUtilityWindowModules._src);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004504 File Offset: 0x00002704
		private static void Apply()
		{
			AssetDatabase.StartAssetEditing();
			bool flag = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._audioModule, ref DOTweenUtilityWindowModules._src.modules.audioEnabled);
			bool flag2 = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._physicsModule, ref DOTweenUtilityWindowModules._src.modules.physicsEnabled);
			bool flag3 = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._physics2DModule, ref DOTweenUtilityWindowModules._src.modules.physics2DEnabled);
			bool flag4 = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._spriteModule, ref DOTweenUtilityWindowModules._src.modules.spriteEnabled);
			bool flag5 = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._uiModule, ref DOTweenUtilityWindowModules._src.modules.uiEnabled);
			bool flag6 = false;
			bool flag7 = false;
			if (EditorUtils.hasPro)
			{
				flag6 = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._textMeshProModule, ref DOTweenUtilityWindowModules._src.modules.textMeshProEnabled);
				flag7 = DOTweenUtilityWindowModules.ToggleModule(DOTweenUtilityWindowModules._tk2DModule, ref DOTweenUtilityWindowModules._src.modules.tk2DEnabled);
			}
			AssetDatabase.StopAssetEditing();
			EditorUtility.SetDirty(DOTweenUtilityWindowModules._src);
			if (flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<b>DOTween module files modified ► </b>");
				if (flag)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.audioEnabled, "Audio");
				}
				if (flag2)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.physicsEnabled, "Physics");
				}
				if (flag3)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.physics2DEnabled, "Physics2D");
				}
				if (flag4)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.spriteEnabled, "Sprites");
				}
				if (flag5)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.uiEnabled, "UI");
				}
				if (flag6)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.textMeshProEnabled, "TextMesh Pro");
				}
				if (flag7)
				{
					DOTweenUtilityWindowModules.Apply_AppendLog(stringBuilder, DOTweenUtilityWindowModules._src.modules.tk2DEnabled, "2D Toolkit");
				}
				stringBuilder.Remove(stringBuilder.Length - 3, 3);
				Debug.Log(stringBuilder.ToString());
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004708 File Offset: 0x00002908
		private static void Apply_AppendLog(StringBuilder strb, bool enabled, string id)
		{
			strb.Append("<color=#").Append(enabled ? "00ff00" : "ff0000").Append('>').Append(id).Append("</color>").Append(" - ");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004758 File Offset: 0x00002958
		private static bool ModuleIsEnabled(DOTweenUtilityWindowModules.ModuleInfo m)
		{
			if (!File.Exists(m.filePath))
			{
				return false;
			}
			using (StreamReader streamReader = new StreamReader(m.filePath))
			{
				for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
				{
					if (text.EndsWith("MODULE_MARKER") && text.StartsWith("#if"))
					{
						return text.Contains("true");
					}
				}
			}
			return true;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000047DC File Offset: 0x000029DC
		private static void CheckAutoModuleSettings(bool applySettings, DOTweenUtilityWindowModules.ModuleInfo m, ref bool srcModuleEnabled)
		{
			if (m.enabled != srcModuleEnabled)
			{
				if (applySettings)
				{
					m.enabled = srcModuleEnabled;
					DOTweenUtilityWindowModules.ToggleModule(m, ref srcModuleEnabled);
					return;
				}
				srcModuleEnabled = m.enabled;
				EditorUtility.SetDirty(DOTweenUtilityWindowModules._src);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004810 File Offset: 0x00002A10
		private static bool ToggleModule(DOTweenUtilityWindowModules.ModuleInfo m, ref bool srcSetting)
		{
			if (!File.Exists(m.filePath))
			{
				return false;
			}
			srcSetting = m.enabled;
			bool result = false;
			DOTweenUtilityWindowModules._LinesToChange.Clear();
			string[] array = File.ReadAllLines(m.filePath);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.EndsWith("MODULE_MARKER") && text.StartsWith("#if") && ((m.enabled && text.Contains("false")) || (!m.enabled && text.Contains("true"))))
				{
					DOTweenUtilityWindowModules._LinesToChange.Add(i);
				}
			}
			if (DOTweenUtilityWindowModules._LinesToChange.Count > 0)
			{
				result = true;
				using (StreamWriter streamWriter = new StreamWriter(m.filePath))
				{
					for (int j = 0; j < array.Length; j++)
					{
						string text2 = array[j];
						if (DOTweenUtilityWindowModules._LinesToChange.Contains(j))
						{
							text2 = (m.enabled ? text2.Replace("false", "true") : text2.Replace("true", "false"));
						}
						streamWriter.WriteLine(text2);
					}
				}
				AssetDatabase.ImportAsset(EditorUtils.FullPathToADBPath(m.filePath), 0);
			}
			string marker = m.id + "_MARKER";
			for (int k = 0; k < DOTweenUtilityWindowModules._ModuleDependentFiles.Length; k++)
			{
				if (DOTweenUtilityWindowModules.ToggleModuleInDependentFile(DOTweenUtilityWindowModules._ModuleDependentFiles[k], m.enabled, marker))
				{
					result = true;
				}
			}
			DOTweenUtilityWindowModules._LinesToChange.Clear();
			return result;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000049A8 File Offset: 0x00002BA8
		private static bool ToggleModuleInDependentFile(string filePath, bool enable, string marker)
		{
			if (!File.Exists(filePath))
			{
				return false;
			}
			bool result = false;
			DOTweenUtilityWindowModules._LinesToChange.Clear();
			string[] array = File.ReadAllLines(filePath);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.EndsWith(marker) && text.StartsWith("#if") && ((enable && text.Contains("false")) || (!enable && text.Contains("true"))))
				{
					DOTweenUtilityWindowModules._LinesToChange.Add(i);
				}
			}
			if (DOTweenUtilityWindowModules._LinesToChange.Count > 0)
			{
				result = true;
				using (StreamWriter streamWriter = new StreamWriter(filePath))
				{
					for (int j = 0; j < array.Length; j++)
					{
						string text2 = array[j];
						if (DOTweenUtilityWindowModules._LinesToChange.Contains(j))
						{
							text2 = (enable ? text2.Replace("false", "true") : text2.Replace("true", "false"));
						}
						streamWriter.WriteLine(text2);
					}
				}
				AssetDatabase.ImportAsset(EditorUtils.FullPathToADBPath(filePath), 0);
			}
			return result;
		}

		// Token: 0x0400003A RID: 58
		private const string ModuleMarkerId = "MODULE_MARKER";

		// Token: 0x0400003B RID: 59
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _audioModule = new DOTweenUtilityWindowModules.ModuleInfo("Modules/DOTweenModuleAudio.cs", "AUDIO");

		// Token: 0x0400003C RID: 60
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _physicsModule = new DOTweenUtilityWindowModules.ModuleInfo("Modules/DOTweenModulePhysics.cs", "PHYSICS");

		// Token: 0x0400003D RID: 61
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _physics2DModule = new DOTweenUtilityWindowModules.ModuleInfo("Modules/DOTweenModulePhysics2D.cs", "PHYSICS2D");

		// Token: 0x0400003E RID: 62
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _spriteModule = new DOTweenUtilityWindowModules.ModuleInfo("Modules/DOTweenModuleSprite.cs", "SPRITE");

		// Token: 0x0400003F RID: 63
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _uiModule = new DOTweenUtilityWindowModules.ModuleInfo("Modules/DOTweenModuleUI.cs", "UI");

		// Token: 0x04000040 RID: 64
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _textMeshProModule = new DOTweenUtilityWindowModules.ModuleInfo("DOTweenTextMeshPro.cs", "TEXTMESHPRO");

		// Token: 0x04000041 RID: 65
		private static readonly DOTweenUtilityWindowModules.ModuleInfo _tk2DModule = new DOTweenUtilityWindowModules.ModuleInfo("DOTweenTk2D.cs", "TK2D");

		// Token: 0x04000042 RID: 66
		private static readonly string[] _ModuleDependentFiles = new string[]
		{
			"DOTWEENDIR/Modules/DOTweenModuleUtils.cs",
			"DOTWEENPRODIR/DOTweenAnimation.cs",
			"DOTWEENPRODIR/DOTweenProShortcuts.cs",
			"DOTWEENPRODIR/Editor/DOTweenAnimationInspector.cs"
		};

		// Token: 0x04000043 RID: 67
		private static EditorWindow _editor;

		// Token: 0x04000044 RID: 68
		private static DOTweenSettings _src;

		// Token: 0x04000045 RID: 69
		private static bool _refreshed;

		// Token: 0x04000046 RID: 70
		private static bool _isWaitingForCompilation;

		// Token: 0x04000047 RID: 71
		private static readonly List<int> _LinesToChange = new List<int>();

		// Token: 0x02000010 RID: 16
		private class ModuleInfo
		{
			// Token: 0x06000061 RID: 97 RVA: 0x0000599D File Offset: 0x00003B9D
			public ModuleInfo(string filePath, string id)
			{
				this.filePath = filePath;
				this.id = id;
			}

			// Token: 0x0400005B RID: 91
			public bool enabled;

			// Token: 0x0400005C RID: 92
			public string filePath;

			// Token: 0x0400005D RID: 93
			public readonly string id;
		}
	}
}
