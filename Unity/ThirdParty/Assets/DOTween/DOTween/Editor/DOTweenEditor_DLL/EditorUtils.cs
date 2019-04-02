using System;
using System.IO;
using System.Reflection;
using System.Text;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
	// Token: 0x02000005 RID: 5
	public static class EditorUtils
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000022E4 File Offset: 0x000004E4
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000022EB File Offset: 0x000004EB
		public static string projectPath { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000022F3 File Offset: 0x000004F3
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000022FA File Offset: 0x000004FA
		public static string assetsPath { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002302 File Offset: 0x00000502
		public static bool hasPro
		{
			get
			{
				if (!EditorUtils._hasCheckedForPro)
				{
					EditorUtils.CheckForPro();
				}
				return EditorUtils._hasPro;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002315 File Offset: 0x00000515
		public static string proVersion
		{
			get
			{
				if (!EditorUtils._hasCheckedForPro)
				{
					EditorUtils.CheckForPro();
				}
				return EditorUtils._proVersion;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002328 File Offset: 0x00000528
		public static string editorADBDir
		{
			get
			{
				if (string.IsNullOrEmpty(EditorUtils._editorADBDir))
				{
					EditorUtils.StoreEditorADBDir();
				}
				return EditorUtils._editorADBDir;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002340 File Offset: 0x00000540
		public static string demigiantDir
		{
			get
			{
				if (string.IsNullOrEmpty(EditorUtils._demigiantDir))
				{
					EditorUtils.StoreDOTweenDirs();
				}
				return EditorUtils._demigiantDir;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002358 File Offset: 0x00000558
		public static string dotweenDir
		{
			get
			{
				if (string.IsNullOrEmpty(EditorUtils._dotweenDir))
				{
					EditorUtils.StoreDOTweenDirs();
				}
				return EditorUtils._dotweenDir;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002370 File Offset: 0x00000570
		public static string dotweenProDir
		{
			get
			{
				if (string.IsNullOrEmpty(EditorUtils._dotweenProDir))
				{
					EditorUtils.StoreDOTweenDirs();
				}
				return EditorUtils._dotweenProDir;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002388 File Offset: 0x00000588
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000238F File Offset: 0x0000058F
		public static bool isOSXEditor { get; private set; } = Application.platform == 0;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002397 File Offset: 0x00000597
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000239E File Offset: 0x0000059E
		public static string pathSlash { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000023A6 File Offset: 0x000005A6
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000023AD File Offset: 0x000005AD
		public static string pathSlashToReplace { get; private set; }

		// Token: 0x0600001A RID: 26 RVA: 0x000023B8 File Offset: 0x000005B8
		static EditorUtils()
		{
			bool flag = Application.platform == 7;
			EditorUtils.pathSlash = (flag ? "\\" : "/");
			EditorUtils.pathSlashToReplace = (flag ? "/" : "\\");
			EditorUtils.projectPath = Application.dataPath;
			EditorUtils.projectPath = EditorUtils.projectPath.Substring(0, EditorUtils.projectPath.LastIndexOf("/"));
			EditorUtils.projectPath = EditorUtils.projectPath.Replace(EditorUtils.pathSlashToReplace, EditorUtils.pathSlash);
			EditorUtils.assetsPath = EditorUtils.projectPath + EditorUtils.pathSlash + "Assets";
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002466 File Offset: 0x00000666
		public static void DelayedCall(float delay, Action callback)
		{
			new DelayedCall(delay, callback);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002470 File Offset: 0x00000670
		public static void SetEditorTexture(Texture2D texture, FilterMode filterMode = 0, int maxTextureSize = 32)
		{
			if (texture.wrapMode == 1)
			{
				return;
			}
			string assetPath = AssetDatabase.GetAssetPath(texture);
			TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			textureImporter.textureType = 2;
			textureImporter.npotScale = 0;
			textureImporter.filterMode = filterMode;
			textureImporter.wrapMode = 1;
			textureImporter.maxTextureSize = maxTextureSize;
			textureImporter.textureFormat = -3;
			AssetDatabase.ImportAsset(assetPath);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024C7 File Offset: 0x000006C7
		public static bool DOTweenSetupRequired()
		{
			return Directory.Exists(EditorUtils.dotweenDir) && Directory.GetFiles(EditorUtils.dotweenDir + "Editor", "DOTweenUpgradeManager.*").Length != 0;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024F4 File Offset: 0x000006F4
		public static void DeleteDOTweenUpgradeManagerFiles()
		{
			string str = EditorUtils.FullPathToADBPath(EditorUtils.dotweenDir);
			AssetDatabase.StartAssetEditing();
			EditorUtils.DeleteAssetsIfExist(new string[]
			{
				str + "Editor/DOTweenUpgradeManager.dll",
				str + "Editor/DOTweenUpgradeManager.xml",
				str + "Editor/DOTweenUpgradeManager.dll.mdb"
			});
			AssetDatabase.StopAssetEditing();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000254C File Offset: 0x0000074C
		public static void DeleteLegacyNoModulesDOTweenFiles()
		{
			string str = EditorUtils.FullPathToADBPath(EditorUtils.dotweenDir);
			AssetDatabase.StartAssetEditing();
			EditorUtils.DeleteAssetsIfExist(new string[]
			{
				str + "DOTween43.dll",
				str + "DOTween43.xml",
				str + "DOTween43.dll.mdb",
				str + "DOTween43.dll.addon",
				str + "DOTween43.xml.addon",
				str + "DOTween43.dll.mdb.addon",
				str + "DOTween46.dll",
				str + "DOTween46.xml",
				str + "DOTween46.dll.mdb",
				str + "DOTween46.dll.addon",
				str + "DOTween46.xml.addon",
				str + "DOTween46.dll.mdb.addon",
				str + "DOTween50.dll",
				str + "DOTween50.xml",
				str + "DOTween50.dll.mdb",
				str + "DOTween50.dll.addon",
				str + "DOTween50.xml.addon",
				str + "DOTween50.dll.mdb.addon",
				str + "DOTweenTextMeshPro.cs.addon",
				str + "DOTweenTextMeshPro_mod.cs",
				str + "DOTweenTk2d.cs.addon"
			});
			AssetDatabase.StopAssetEditing();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026AC File Offset: 0x000008AC
		public static void DeleteOldDemiLibCore()
		{
			string text = EditorUtils.GetAssemblyFilePath(typeof(DOTween).Assembly);
			string text2 = (text.IndexOf("/") != -1) ? "/" : "\\";
			text = text.Substring(0, text.LastIndexOf(text2));
			text = text.Substring(0, text.LastIndexOf(text2)) + text2 + "DemiLib";
			string text3 = EditorUtils.FullPathToADBPath(text);
			if (!EditorUtils.AssetExists(text3))
			{
				return;
			}
			string text4 = text3 + "/Core";
			if (!EditorUtils.AssetExists(text4))
			{
				return;
			}
			EditorUtils.DeleteAssetsIfExist(new string[]
			{
				text3 + "/DemiLib.dll",
				text3 + "/DemiLib.xml",
				text3 + "/DemiLib.dll.mdb",
				text3 + "/Editor/DemiEditor.dll",
				text3 + "/Editor/DemiEditor.xml",
				text3 + "/Editor/DemiEditor.dll.mdb",
				text3 + "/Editor/Imgs"
			});
			if (EditorUtils.AssetExists(text3 + "/Editor") && Directory.GetFiles(text + text2 + "Editor").Length == 0)
			{
				AssetDatabase.DeleteAsset(text3 + "/Editor");
				AssetDatabase.ImportAsset(text4, 256);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027E8 File Offset: 0x000009E8
		private static void DeleteAssetsIfExist(string[] adbFilePaths)
		{
			foreach (string text in adbFilePaths)
			{
				if (EditorUtils.AssetExists(text))
				{
					AssetDatabase.DeleteAsset(text);
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002818 File Offset: 0x00000A18
		public static bool AssetExists(string adbPath)
		{
			string path = EditorUtils.ADBPathToFullPath(adbPath);
			return File.Exists(path) || Directory.Exists(path);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000283C File Offset: 0x00000A3C
		public static string ADBPathToFullPath(string adbPath)
		{
			adbPath = adbPath.Replace(EditorUtils.pathSlashToReplace, EditorUtils.pathSlash);
			return EditorUtils.projectPath + EditorUtils.pathSlash + adbPath;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002860 File Offset: 0x00000A60
		public static string FullPathToADBPath(string fullPath)
		{
			return fullPath.Substring(EditorUtils.projectPath.Length + 1).Replace("\\", "/");
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002884 File Offset: 0x00000A84
		public static T ConnectToSourceAsset<T>(string adbFilePath, bool createIfMissing = false) where T : ScriptableObject
		{
			if (!EditorUtils.AssetExists(adbFilePath))
			{
				if (!createIfMissing)
				{
					return default(T);
				}
				EditorUtils.CreateScriptableAsset<T>(adbFilePath);
			}
			T t = (T)((object)AssetDatabase.LoadAssetAtPath(adbFilePath, typeof(T)));
			if (t == null)
			{
				EditorUtils.CreateScriptableAsset<T>(adbFilePath);
				t = (T)((object)AssetDatabase.LoadAssetAtPath(adbFilePath, typeof(T)));
			}
			return t;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028F0 File Offset: 0x00000AF0
		public static string GetAssemblyFilePath(Assembly assembly)
		{
			string text = Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
			if (text.Substring(text.Length - 3) == "dll")
			{
				return text;
			}
			return Path.GetFullPath(assembly.Location);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000293C File Offset: 0x00000B3C
		public static void AddGlobalDefine(string id)
		{
			bool flag = false;
			int num = 0;
			foreach (BuildTargetGroup buildTargetGroup in (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup)))
			{
				if (EditorUtils.IsValidBuildTargetGroup(buildTargetGroup))
				{
					string text = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
					if (Array.IndexOf<string>(text.Split(new char[]
					{
						';'
					}), id) == -1)
					{
						flag = true;
						num++;
						text += ((text.Length > 0) ? (";" + id) : id);
						PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, text);
					}
				}
			}
			if (flag)
			{
				Debug.Log(string.Format("DOTween : added global define \"{0}\" to {1} BuildTargetGroups", id, num));
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029EC File Offset: 0x00000BEC
		public static void RemoveGlobalDefine(string id)
		{
			bool flag = false;
			int num = 0;
			foreach (BuildTargetGroup buildTargetGroup in (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup)))
			{
				if (EditorUtils.IsValidBuildTargetGroup(buildTargetGroup))
				{
					string[] array2 = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(new char[]
					{
						';'
					});
					if (Array.IndexOf<string>(array2, id) != -1)
					{
						flag = true;
						num++;
						EditorUtils._Strb.Length = 0;
						for (int j = 0; j < array2.Length; j++)
						{
							if (!(array2[j] == id))
							{
								if (EditorUtils._Strb.Length > 0)
								{
									EditorUtils._Strb.Append(';');
								}
								EditorUtils._Strb.Append(array2[j]);
							}
						}
						PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, EditorUtils._Strb.ToString());
					}
				}
			}
			EditorUtils._Strb.Length = 0;
			if (flag)
			{
				Debug.Log(string.Format("DOTween : removed global define \"{0}\" from {1} BuildTargetGroups", id, num));
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002AEC File Offset: 0x00000CEC
		public static bool HasGlobalDefine(string id, BuildTargetGroup? buildTargetGroup = null)
		{
			BuildTargetGroup[] array;
			if (buildTargetGroup != null)
			{
				(array = new BuildTargetGroup[1])[0] = buildTargetGroup.Value;
			}
			else
			{
				array = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));
			}
			foreach (BuildTargetGroup buildTargetGroup2 in array)
			{
				if (EditorUtils.IsValidBuildTargetGroup(buildTargetGroup2) && Array.IndexOf<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup2).Split(new char[]
				{
					';'
				}), id) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B68 File Offset: 0x00000D68
		private static void CheckForPro()
		{
			EditorUtils._hasCheckedForPro = true;
			try
			{
				EditorUtils._proVersion = (Assembly.Load("DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null").GetType("DG.Tweening.DOTweenPro").GetField("Version", BindingFlags.Static | BindingFlags.Public).GetValue(null) as string);
				EditorUtils._hasPro = true;
			}
			catch
			{
				EditorUtils._hasPro = false;
				EditorUtils._proVersion = "-";
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002BD8 File Offset: 0x00000DD8
		private static void StoreEditorADBDir()
		{
			EditorUtils._editorADBDir = Path.GetDirectoryName(EditorUtils.GetAssemblyFilePath(Assembly.GetExecutingAssembly())).Substring(Application.dataPath.Length + 1).Replace("\\", "/") + "/";
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C18 File Offset: 0x00000E18
		private static void StoreDOTweenDirs()
		{
			EditorUtils._dotweenDir = Path.GetDirectoryName(EditorUtils.GetAssemblyFilePath(Assembly.GetExecutingAssembly()));
			string text = (EditorUtils._dotweenDir.IndexOf("/") != -1) ? "/" : "\\";
			EditorUtils._dotweenDir = EditorUtils._dotweenDir.Substring(0, EditorUtils._dotweenDir.LastIndexOf(text) + 1);
			EditorUtils._dotweenProDir = EditorUtils._dotweenDir.Substring(0, EditorUtils._dotweenDir.LastIndexOf(text));
			EditorUtils._dotweenProDir = EditorUtils._dotweenProDir.Substring(0, EditorUtils._dotweenProDir.LastIndexOf(text) + 1) + "DOTweenPro" + text;
			EditorUtils._demigiantDir = EditorUtils._dotweenDir.Substring(0, EditorUtils._dotweenDir.LastIndexOf(text));
			EditorUtils._demigiantDir = EditorUtils._demigiantDir.Substring(0, EditorUtils._demigiantDir.LastIndexOf(text) + 1);
			if (EditorUtils._demigiantDir.Substring(EditorUtils._demigiantDir.Length - 10, 9) != "Demigiant")
			{
				EditorUtils._demigiantDir = null;
			}
			EditorUtils._dotweenDir = EditorUtils._dotweenDir.Replace(EditorUtils.pathSlashToReplace, EditorUtils.pathSlash);
			EditorUtils._dotweenProDir = EditorUtils._dotweenProDir.Replace(EditorUtils.pathSlashToReplace, EditorUtils.pathSlash);
			if (EditorUtils._demigiantDir != null)
			{
				EditorUtils._demigiantDir = EditorUtils._demigiantDir.Replace(EditorUtils.pathSlashToReplace, EditorUtils.pathSlash);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D6D File Offset: 0x00000F6D
		private static void CreateScriptableAsset<T>(string adbFilePath) where T : ScriptableObject
		{
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<T>(), adbFilePath);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002D80 File Offset: 0x00000F80
		private static bool IsValidBuildTargetGroup(BuildTargetGroup group)
		{
			if (group == null)
			{
				return false;
			}
			MethodBase method = Type.GetType("UnityEditor.Modules.ModuleManager, UnityEditor.dll").GetMethod("GetTargetStringFromBuildTargetGroup", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo method2 = typeof(PlayerSettings).GetMethod("GetPlatformName", BindingFlags.Static | BindingFlags.NonPublic);
			string value = (string)method.Invoke(null, new object[]
			{
				group
			});
			string value2 = (string)method2.Invoke(null, new object[]
			{
				group
			});
			return !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(value2);
		}

		// Token: 0x0400000D RID: 13
		private static readonly StringBuilder _Strb = new StringBuilder();

		// Token: 0x0400000E RID: 14
		private static bool _hasPro;

		// Token: 0x0400000F RID: 15
		private static string _proVersion;

		// Token: 0x04000010 RID: 16
		private static bool _hasCheckedForPro;

		// Token: 0x04000011 RID: 17
		private static string _editorADBDir;

		// Token: 0x04000012 RID: 18
		private static string _demigiantDir;

		// Token: 0x04000013 RID: 19
		private static string _dotweenDir;

		// Token: 0x04000014 RID: 20
		private static string _dotweenProDir;
	}
}
