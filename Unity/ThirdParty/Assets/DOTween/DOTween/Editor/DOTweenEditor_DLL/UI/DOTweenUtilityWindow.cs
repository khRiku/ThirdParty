using System;
using System.IO;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
	// Token: 0x0200000D RID: 13
	internal class DOTweenUtilityWindow : EditorWindow
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00004AF0 File Offset: 0x00002CF0
		[MenuItem("Tools/Demigiant/DOTween Utility Panel")]
		private static void ShowWindow()
		{
			DOTweenUtilityWindow.Open();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004AF8 File Offset: 0x00002CF8
		public static void Open()
		{
			DOTweenUtilityWindow window = EditorWindow.GetWindow<DOTweenUtilityWindow>(true, "DOTween Utility Panel", true);
			window.minSize = DOTweenUtilityWindow._WinSize;
			window.maxSize = DOTweenUtilityWindow._WinSize;
			window.ShowUtility();
			EditorPrefs.SetString("DOTweenVersion", DOTween.Version);
			EditorPrefs.SetString("DOTweenProVersion", EditorUtils.proVersion);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004B4C File Offset: 0x00002D4C
		private bool Init()
		{
			if (this._initialized)
			{
				return true;
			}
			if (this._headerImg == null)
			{
				this._headerImg = (AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/Header.jpg", typeof(Texture2D)) as Texture2D);
				if (this._headerImg == null)
				{
					return false;
				}

				EditorUtils.SetEditorTexture(this._headerImg, FilterMode.Bilinear, 512);
				this._headerSize.x = DOTweenUtilityWindow._WinSize.x;
				this._headerSize.y = (float)((int)(DOTweenUtilityWindow._WinSize.x * (float)this._headerImg.height / (float)this._headerImg.width));
				this._footerImg = (AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + (EditorGUIUtility.isProSkin ? "Imgs/Footer.png" : "Imgs/Footer_dark.png"), typeof(Texture2D)) as Texture2D);
				EditorUtils.SetEditorTexture(this._footerImg, FilterMode.Bilinear, 256);
				this._footerSize.x = DOTweenUtilityWindow._WinSize.x;
				this._footerSize.y = (float)((int)(DOTweenUtilityWindow._WinSize.x * (float)this._footerImg.height / (float)this._footerImg.width));
			}
			this._initialized = true;
			return true;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004CA4 File Offset: 0x00002EA4
		private void OnHierarchyChange()
		{
			base.Repaint();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004CAC File Offset: 0x00002EAC
		private void OnEnable()
		{
			this._innerTitle = "DOTween v" + DOTween.Version + (TweenManager.isDebugBuild ? " [Debug build]" : " [Release build]");
			if (EditorUtils.hasPro)
			{
				this._innerTitle = this._innerTitle + "\nDOTweenPro v" + EditorUtils.proVersion;
			}
			else
			{
				this._innerTitle += "\nDOTweenPro not installed";
			}
			this.Init();
			this._setupRequired = EditorUtils.DOTweenSetupRequired();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004D2D File Offset: 0x00002F2D
		private void OnDestroy()
		{
			if (this._src != null)
			{
				this._src.modules.showPanel = false;
				EditorUtility.SetDirty(this._src);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004D5C File Offset: 0x00002F5C
		private void OnGUI()
		{
			if (!this.Init())
			{
				GUILayout.Space(8f);
				GUILayout.Label("Completing import process...", new GUILayoutOption[0]);
				return;
			}
			this.Connect(false);
			EditorGUIUtils.SetGUIStyles(new Vector2?(this._footerSize));
			if (Application.isPlaying)
			{
				GUILayout.Space(40f);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(40f);
				GUILayout.Label("DOTween Utility Panel\nis disabled while in Play Mode", EditorGUIUtils.wrapCenterLabelStyle, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true)
				});
				GUILayout.Space(40f);
				GUILayout.EndHorizontal();
			}
			else if (this._src.modules.showPanel)
			{
				if (DOTweenUtilityWindowModules.Draw(this, this._src))
				{
					this._setupRequired = EditorUtils.DOTweenSetupRequired();
					this._src.modules.showPanel = false;
					EditorUtility.SetDirty(this._src);
				}
			}
			else
			{
				Rect rect = new Rect(0f, 0f, this._headerSize.x, 30f);
				this._selectedTab = GUI.Toolbar(rect, this._selectedTab, this._tabLabels);
				int selectedTab = this._selectedTab;
				if (selectedTab == 1)
				{
					this.DrawPreferencesGUI();
				}
				else
				{
					this.DrawSetupGUI();
				}
			}
			if (GUI.changed)
			{
				EditorUtility.SetDirty(this._src);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004EA8 File Offset: 0x000030A8
		private void DrawSetupGUI()
		{
			Rect rect = new Rect(0f, 30f, this._headerSize.x, this._headerSize.y);
			GUI.DrawTexture(rect, this._headerImg, 0, false);
			GUILayout.Space(rect.y + this._headerSize.y + 2f);
			GUILayout.Label(this._innerTitle, TweenManager.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle, new GUILayoutOption[0]);
			if (this._setupRequired)
			{
				GUI.backgroundColor = Color.red;
				GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
				GUILayout.Label("DOTWEEN SETUP REQUIRED", EditorGUIUtils.setupLabelStyle, new GUILayoutOption[0]);
				GUILayout.EndVertical();
				GUI.backgroundColor = Color.white;
			}
			else
			{
				GUILayout.Space(8f);
			}
			GUI.color = Color.green;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("<b>Setup DOTween...</b>\n(add/remove Modules)", EditorGUIUtils.btSetup, new GUILayoutOption[0]))
			{
				DOTweenUtilityWindowModules.ApplyModulesSettings();
				this._src.modules.showPanel = true;
				EditorUtility.SetDirty(this._src);
				EditorUtils.DeleteLegacyNoModulesDOTweenFiles();
				DOTweenDefines.RemoveAllLegacyDefines();
				EditorUtils.DeleteDOTweenUpgradeManagerFiles();
				return;
			}
			GUILayout.FlexibleSpace();
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
			GUILayout.Space(8f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Website", EditorGUIUtils.btBigStyle, new GUILayoutOption[]
			{
				GUILayout.Width(DOTweenUtilityWindow._HalfBtSize)
			}))
			{
				Application.OpenURL("http://dotween.demigiant.com/index.php");
			}
			if (GUILayout.Button("Get Started", EditorGUIUtils.btBigStyle, new GUILayoutOption[]
			{
				GUILayout.Width(DOTweenUtilityWindow._HalfBtSize)
			}))
			{
				Application.OpenURL("http://dotween.demigiant.com/getstarted.php");
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Documentation", EditorGUIUtils.btBigStyle, new GUILayoutOption[]
			{
				GUILayout.Width(DOTweenUtilityWindow._HalfBtSize)
			}))
			{
				Application.OpenURL("http://dotween.demigiant.com/documentation.php");
			}
			if (GUILayout.Button("Support", EditorGUIUtils.btBigStyle, new GUILayoutOption[]
			{
				GUILayout.Width(DOTweenUtilityWindow._HalfBtSize)
			}))
			{
				Application.OpenURL("http://dotween.demigiant.com/support.php");
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Changelog", EditorGUIUtils.btBigStyle, new GUILayoutOption[]
			{
				GUILayout.Width(DOTweenUtilityWindow._HalfBtSize)
			}))
			{
				Application.OpenURL("http://dotween.demigiant.com/download.php");
			}
			if (GUILayout.Button("Check Updates", EditorGUIUtils.btBigStyle, new GUILayoutOption[]
			{
				GUILayout.Width(DOTweenUtilityWindow._HalfBtSize)
			}))
			{
				Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(14f);
			if (GUILayout.Button(this._footerImg, EditorGUIUtils.btImgStyle, new GUILayoutOption[0]))
			{
				Application.OpenURL("http://www.demigiant.com/");
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005180 File Offset: 0x00003380
		private void DrawPreferencesGUI()
		{
			GUILayout.Space(40f);
			if (GUILayout.Button("Reset", EditorGUIUtils.btBigStyle, new GUILayoutOption[0]))
			{
				this._src.useSafeMode = true;
				this._src.showUnityEditorReport = false;
				this._src.timeScale = 1f;
				this._src.useSmoothDeltaTime = false;
				this._src.maxSmoothUnscaledTime = 0.15f;
				this._src.rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;
				this._src.logBehaviour = LogBehaviour.ErrorsOnly;
				this._src.drawGizmos = true;
				this._src.defaultRecyclable = false;
				this._src.defaultAutoPlay = AutoPlay.All;
				this._src.defaultUpdateType = UpdateType.Normal;
				this._src.defaultTimeScaleIndependent = false;
				this._src.defaultEaseType = Ease.OutQuad;
				this._src.defaultEaseOvershootOrAmplitude = 1.70158f;
				this._src.defaultEasePeriod = 0f;
				this._src.defaultAutoKill = true;
				this._src.defaultLoopType = LoopType.Restart;
				EditorUtility.SetDirty(this._src);
			}
			GUILayout.Space(8f);
			this._src.useSafeMode = EditorGUILayout.Toggle("Safe Mode", this._src.useSafeMode, new GUILayoutOption[0]);
			this._src.timeScale = EditorGUILayout.FloatField("DOTween's TimeScale", this._src.timeScale, new GUILayoutOption[0]);
			this._src.useSmoothDeltaTime = EditorGUILayout.Toggle("Smooth DeltaTime", this._src.useSmoothDeltaTime, new GUILayoutOption[0]);
			this._src.maxSmoothUnscaledTime = EditorGUILayout.Slider("Max SmoothUnscaledTime", this._src.maxSmoothUnscaledTime, 0.01f, 1f, new GUILayoutOption[0]);
			this._src.rewindCallbackMode = (RewindCallbackMode)EditorGUILayout.EnumPopup("OnRewind Callback Mode", this._src.rewindCallbackMode, new GUILayoutOption[0]);
			GUILayout.Space(-5f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(154f);
			EditorGUILayout.HelpBox((this._src.rewindCallbackMode == RewindCallbackMode.FireIfPositionChanged) ? "When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will be fired only if the tween isn't already rewinded" : ((this._src.rewindCallbackMode == RewindCallbackMode.FireAlwaysWithRewind) ? "When calling Rewind, OnRewind callbacks will always be fired, even if the tween is already rewinded." : "When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will always be fired, even if the tween is already rewinded"), 0);
			GUILayout.EndHorizontal();
			this._src.showUnityEditorReport = EditorGUILayout.Toggle("Editor Report", this._src.showUnityEditorReport, new GUILayoutOption[0]);
			this._src.logBehaviour = (LogBehaviour)EditorGUILayout.EnumPopup("Log Behaviour", this._src.logBehaviour, new GUILayoutOption[0]);
			this._src.drawGizmos = EditorGUILayout.Toggle("Draw Path Gizmos", this._src.drawGizmos, new GUILayoutOption[0]);
			DOTweenSettings.SettingsLocation storeSettingsLocation = this._src.storeSettingsLocation;
			this._src.storeSettingsLocation = (DOTweenSettings.SettingsLocation)EditorGUILayout.Popup("Settings Location", (int)this._src.storeSettingsLocation, this._settingsLocation, new GUILayoutOption[0]);
			if (this._src.storeSettingsLocation != storeSettingsLocation)
			{
				if (this._src.storeSettingsLocation == DOTweenSettings.SettingsLocation.DemigiantDirectory && EditorUtils.demigiantDir == null)
				{
					EditorUtility.DisplayDialog("Change DOTween Settings Location", "Demigiant directory not present (must be the parent of DOTween's directory)", "Ok");
					if (storeSettingsLocation == DOTweenSettings.SettingsLocation.DemigiantDirectory)
					{
						this._src.storeSettingsLocation = DOTweenSettings.SettingsLocation.AssetsDirectory;
						this.Connect(true);
					}
					else
					{
						this._src.storeSettingsLocation = storeSettingsLocation;
					}
				}
				else
				{
					this.Connect(true);
				}
			}
			GUILayout.Space(8f);
			GUILayout.Label("DEFAULTS ▼", new GUILayoutOption[0]);
			this._src.defaultRecyclable = EditorGUILayout.Toggle("Recycle Tweens", this._src.defaultRecyclable, new GUILayoutOption[0]);
			this._src.defaultAutoPlay = (AutoPlay)EditorGUILayout.EnumPopup("AutoPlay", this._src.defaultAutoPlay, new GUILayoutOption[0]);
			this._src.defaultUpdateType = (UpdateType)EditorGUILayout.EnumPopup("Update Type", this._src.defaultUpdateType, new GUILayoutOption[0]);
			this._src.defaultTimeScaleIndependent = EditorGUILayout.Toggle("TimeScale Independent", this._src.defaultTimeScaleIndependent, new GUILayoutOption[0]);
			this._src.defaultEaseType = (Ease)EditorGUILayout.EnumPopup("Ease", this._src.defaultEaseType, new GUILayoutOption[0]);
			this._src.defaultEaseOvershootOrAmplitude = EditorGUILayout.FloatField("Ease Overshoot", this._src.defaultEaseOvershootOrAmplitude, new GUILayoutOption[0]);
			this._src.defaultEasePeriod = EditorGUILayout.FloatField("Ease Period", this._src.defaultEasePeriod, new GUILayoutOption[0]);
			this._src.defaultAutoKill = EditorGUILayout.Toggle("AutoKill", this._src.defaultAutoKill, new GUILayoutOption[0]);
			this._src.defaultLoopType = (LoopType)EditorGUILayout.EnumPopup("Loop Type", this._src.defaultLoopType, new GUILayoutOption[0]);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000567E File Offset: 0x0000387E
		public static DOTweenSettings GetDOTweenSettings()
		{
			return DOTweenUtilityWindow.ConnectToSource(null, false, false);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005688 File Offset: 0x00003888
		private static DOTweenSettings ConnectToSource(DOTweenSettings src, bool createIfMissing, bool fullSetup)
		{
			DOTweenUtilityWindow.LocationData locationData = new DOTweenUtilityWindow.LocationData(EditorUtils.assetsPath + EditorUtils.pathSlash + "Resources");
			DOTweenUtilityWindow.LocationData locationData2 = new DOTweenUtilityWindow.LocationData(EditorUtils.dotweenDir + "Resources");
			bool flag = EditorUtils.demigiantDir != null;
			DOTweenUtilityWindow.LocationData locationData3 = flag ? new DOTweenUtilityWindow.LocationData(EditorUtils.demigiantDir + "Resources") : default(DOTweenUtilityWindow.LocationData);
			if (src == null)
			{
				src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(locationData.adbFilePath, false);
				if (src == null)
				{
					src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(locationData2.adbFilePath, false);
				}
				if (src == null && flag)
				{
					src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(locationData3.adbFilePath, false);
				}
			}
			if (src == null)
			{
				if (!createIfMissing)
				{
					return null;
				}
				if (!Directory.Exists(locationData.dir))
				{
					AssetDatabase.CreateFolder(locationData.adbParentDir, "Resources");
				}
				src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(locationData.adbFilePath, true);
			}
			if (fullSetup)
			{
				switch (src.storeSettingsLocation)
				{
				case DOTweenSettings.SettingsLocation.AssetsDirectory:
					src = DOTweenUtilityWindow.MoveSrc(new DOTweenUtilityWindow.LocationData[]
					{
						locationData2,
						locationData3
					}, locationData);
					break;
				case DOTweenSettings.SettingsLocation.DOTweenDirectory:
					src = DOTweenUtilityWindow.MoveSrc(new DOTweenUtilityWindow.LocationData[]
					{
						locationData,
						locationData3
					}, locationData2);
					break;
				case DOTweenSettings.SettingsLocation.DemigiantDirectory:
					src = DOTweenUtilityWindow.MoveSrc(new DOTweenUtilityWindow.LocationData[]
					{
						locationData,
						locationData2
					}, locationData3);
					break;
				}
			}
			return src;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000057F3 File Offset: 0x000039F3
		private void Connect(bool forceReconnect = false)
		{
			if (this._src != null && !forceReconnect)
			{
				return;
			}
			this._src = DOTweenUtilityWindow.ConnectToSource(this._src, true, true);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000581C File Offset: 0x00003A1C
		private static DOTweenSettings MoveSrc(DOTweenUtilityWindow.LocationData[] from, DOTweenUtilityWindow.LocationData to)
		{
			if (!Directory.Exists(to.dir))
			{
				AssetDatabase.CreateFolder(to.adbParentDir, "Resources");
			}
			foreach (DOTweenUtilityWindow.LocationData locationData in from)
			{
				if (File.Exists(locationData.filePath))
				{
					AssetDatabase.MoveAsset(locationData.adbFilePath, to.adbFilePath);
					AssetDatabase.DeleteAsset(locationData.adbFilePath);
					if (Directory.GetDirectories(locationData.dir).Length == 0 && Directory.GetFiles(locationData.dir).Length == 0)
					{
						AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(locationData.dir));
					}
				}
			}
			return EditorUtils.ConnectToSourceAsset<DOTweenSettings>(to.adbFilePath, true);
		}

		// Token: 0x04000049 RID: 73
		private const string _Title = "DOTween Utility Panel";

		// Token: 0x0400004A RID: 74
		private static readonly Vector2 _WinSize = new Vector2(370f, 490f);

		// Token: 0x0400004B RID: 75
		public const string Id = "DOTweenVersion";

		// Token: 0x0400004C RID: 76
		public const string IdPro = "DOTweenProVersion";

		// Token: 0x0400004D RID: 77
		private static readonly float _HalfBtSize = DOTweenUtilityWindow._WinSize.x * 0.5f - 6f;

		// Token: 0x0400004E RID: 78
		private bool _initialized;

		// Token: 0x0400004F RID: 79
		private DOTweenSettings _src;

		// Token: 0x04000050 RID: 80
		private Texture2D _headerImg;

		// Token: 0x04000051 RID: 81
		private Texture2D _footerImg;

		// Token: 0x04000052 RID: 82
		private Vector2 _headerSize;

		// Token: 0x04000053 RID: 83
		private Vector2 _footerSize;

		// Token: 0x04000054 RID: 84
		private string _innerTitle;

		// Token: 0x04000055 RID: 85
		private bool _setupRequired;

		// Token: 0x04000056 RID: 86
		private int _selectedTab;

		// Token: 0x04000057 RID: 87
		private string[] _tabLabels = new string[]
		{
			"Setup",
			"Preferences"
		};

		// Token: 0x04000058 RID: 88
		private string[] _settingsLocation = new string[]
		{
			"Assets > Resources",
			"DOTween > Resources",
			"Demigiant > Resources"
		};

		// Token: 0x02000011 RID: 17
		private struct LocationData
		{
			// Token: 0x06000062 RID: 98 RVA: 0x000059B4 File Offset: 0x00003BB4
			public LocationData(string srcDir)
			{
				this = default(DOTweenUtilityWindow.LocationData);
				this.dir = srcDir;
				this.filePath = this.dir + EditorUtils.pathSlash + "DOTweenSettings.asset";
				this.adbFilePath = EditorUtils.FullPathToADBPath(this.filePath);
				this.adbParentDir = EditorUtils.FullPathToADBPath(this.dir.Substring(0, this.dir.LastIndexOf(EditorUtils.pathSlash)));
			}

			// Token: 0x0400005E RID: 94
			public string dir;

			// Token: 0x0400005F RID: 95
			public string filePath;

			// Token: 0x04000060 RID: 96
			public string adbFilePath;

			// Token: 0x04000061 RID: 97
			public string adbParentDir;
		}
	}
}
