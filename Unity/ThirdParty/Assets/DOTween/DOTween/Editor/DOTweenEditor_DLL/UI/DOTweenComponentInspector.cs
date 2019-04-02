using System;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
	// Token: 0x0200000A RID: 10
	[CustomEditor(typeof(DOTweenComponent))]
	public class DOTweenComponentInspector : Editor
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000035A0 File Offset: 0x000017A0
		private void OnEnable()
		{
			this._isRuntime = EditorApplication.isPlaying;
			this.ConnectToSource(true);
			this._strBuilder.Remove(0, this._strBuilder.Length);
			this._strBuilder.Append("DOTween v").Append(DOTween.Version);
			if (TweenManager.isDebugBuild)
			{
				this._strBuilder.Append(" [Debug build]");
			}
			else
			{
				this._strBuilder.Append(" [Release build]");
			}
			if (EditorUtils.hasPro)
			{
				this._strBuilder.Append("\nDOTweenPro v").Append(EditorUtils.proVersion);
			}
			else
			{
				this._strBuilder.Append("\nDOTweenPro not installed");
			}
			this._title = this._strBuilder.ToString();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003664 File Offset: 0x00001864
		public override void OnInspectorGUI()
		{
			this._isRuntime = EditorApplication.isPlaying;
			this.ConnectToSource(false);
			EditorGUIUtils.SetGUIStyles(null);
			GUILayout.Space(4f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUI.DrawTexture(GUILayoutUtility.GetRect(0f, 93f, 18f, 18f), this._headerImg, 2, true);
			GUILayout.Label(this._isRuntime ? "RUNTIME MODE" : "EDITOR MODE", new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			int totActiveTweens = TweenManager.totActiveTweens;
			int num = TweenManager.TotalPlayingTweens();
			int value = totActiveTweens - num;
			int totActiveDefaultTweens = TweenManager.totActiveDefaultTweens;
			int totActiveLateTweens = TweenManager.totActiveLateTweens;
			GUILayout.Label(this._title, TweenManager.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle, new GUILayoutOption[0]);
			if (!this._isRuntime)
			{
				GUI.backgroundColor = new Color(0f, 0.31f, 0.48f);
				GUI.contentColor = Color.white;
				GUILayout.Label("This component is <b>added automatically</b> by DOTween at runtime.\nAdding it yourself is <b>not recommended</b> unless you really know what you're doing: you'll have to be sure it's <b>never destroyed</b> and that it's present <b>in every scene</b>.", EditorGUIUtils.infoboxStyle, new GUILayoutOption[0]);
				GUI.backgroundColor = (GUI.contentColor = (GUI.contentColor = Color.white));
			}
			GUILayout.Space(6f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("Documentation", new GUILayoutOption[0]))
			{
				Application.OpenURL("http://dotween.demigiant.com/documentation.php");
			}
			if (GUILayout.Button("Check Updates", new GUILayoutOption[0]))
			{
				Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
			}
			GUILayout.EndHorizontal();
			if (this._isRuntime)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(this._settings.showPlayingTweens ? "Hide Playing Tweens" : "Show Playing Tweens", new GUILayoutOption[0]))
				{
					this._settings.showPlayingTweens = !this._settings.showPlayingTweens;
					EditorUtility.SetDirty(this._settings);
				}
				if (GUILayout.Button(this._settings.showPausedTweens ? "Hide Paused Tweens" : "Show Paused Tweens", new GUILayoutOption[0]))
				{
					this._settings.showPausedTweens = !this._settings.showPausedTweens;
					EditorUtility.SetDirty(this._settings);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Play all", new GUILayoutOption[0]))
				{
					DOTween.PlayAll();
				}
				if (GUILayout.Button("Pause all", new GUILayoutOption[0]))
				{
					DOTween.PauseAll();
				}
				if (GUILayout.Button("Kill all", new GUILayoutOption[0]))
				{
					DOTween.KillAll(false);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(8f);
				this._strBuilder.Length = 0;
				this._strBuilder.Append("Active tweens: ").Append(totActiveTweens).Append(" (").Append(TweenManager.totActiveTweeners).Append("/").Append(TweenManager.totActiveSequences).Append(")").Append("\nDefault/Late tweens: ").Append(totActiveDefaultTweens).Append("/").Append(totActiveLateTweens).Append("\nPlaying tweens: ").Append(num);
				if (this._settings.showPlayingTweens)
				{
					foreach (Tween tween in TweenManager._activeTweens)
					{
						if (tween != null && tween.isPlaying)
						{
							this._strBuilder.Append("\n   - [").Append(tween.tweenType).Append("] ").Append(tween.target);
						}
					}
				}
				this._strBuilder.Append("\nPaused tweens: ").Append(value);
				if (this._settings.showPausedTweens)
				{
					foreach (Tween tween2 in TweenManager._activeTweens)
					{
						if (tween2 != null && !tween2.isPlaying)
						{
							this._strBuilder.Append("\n   - [").Append(tween2.tweenType).Append("] ").Append(tween2.target);
						}
					}
				}
				this._strBuilder.Append("\nPooled tweens: ").Append(TweenManager.TotalPooledTweens()).Append(" (").Append(TweenManager.totPooledTweeners).Append("/").Append(TweenManager.totPooledSequences).Append(")");
				GUILayout.Label(this._strBuilder.ToString(), new GUILayoutOption[0]);
				GUILayout.Space(8f);
				this._strBuilder.Remove(0, this._strBuilder.Length);
				this._strBuilder.Append("Tweens Capacity: ").Append(TweenManager.maxTweeners).Append("/").Append(TweenManager.maxSequences).Append("\nMax Simultaneous Active Tweens: ").Append(DOTween.maxActiveTweenersReached).Append("/").Append(DOTween.maxActiveSequencesReached);
				GUILayout.Label(this._strBuilder.ToString(), new GUILayoutOption[0]);
			}
			GUILayout.Space(8f);
			this._strBuilder.Remove(0, this._strBuilder.Length);
			this._strBuilder.Append("SETTINGS ▼");
			this._strBuilder.Append("\nSafe Mode: ").Append((this._isRuntime ? DOTween.useSafeMode : this._settings.useSafeMode) ? "ON" : "OFF");
			this._strBuilder.Append("\nLog Behaviour: ").Append(this._isRuntime ? DOTween.logBehaviour : this._settings.logBehaviour);
			this._strBuilder.Append("\nShow Unity Editor Report: ").Append(this._isRuntime ? DOTween.showUnityEditorReport : this._settings.showUnityEditorReport);
			this._strBuilder.Append("\nTimeScale (Unity/DOTween): ").Append(Time.timeScale).Append("/").Append(this._isRuntime ? DOTween.timeScale : this._settings.timeScale);
			GUILayout.Label(this._strBuilder.ToString(), new GUILayoutOption[0]);
			GUILayout.Label("NOTE: DOTween's TimeScale is not the same as Unity's Time.timeScale: it is actually multiplied by it except for tweens that are set to update independently", EditorGUIUtils.wordWrapItalicLabelStyle, new GUILayoutOption[0]);
			GUILayout.Space(8f);
			this._strBuilder.Remove(0, this._strBuilder.Length);
			this._strBuilder.Append("DEFAULTS ▼");
			this._strBuilder.Append("\ndefaultRecyclable: ").Append(this._isRuntime ? DOTween.defaultRecyclable : this._settings.defaultRecyclable);
			this._strBuilder.Append("\ndefaultUpdateType: ").Append(this._isRuntime ? DOTween.defaultUpdateType : this._settings.defaultUpdateType);
			this._strBuilder.Append("\ndefaultTSIndependent: ").Append(this._isRuntime ? DOTween.defaultTimeScaleIndependent : this._settings.defaultTimeScaleIndependent);
			this._strBuilder.Append("\ndefaultAutoKill: ").Append(this._isRuntime ? DOTween.defaultAutoKill : this._settings.defaultAutoKill);
			this._strBuilder.Append("\ndefaultAutoPlay: ").Append(this._isRuntime ? DOTween.defaultAutoPlay : this._settings.defaultAutoPlay);
			this._strBuilder.Append("\ndefaultEaseType: ").Append(this._isRuntime ? DOTween.defaultEaseType : this._settings.defaultEaseType);
			this._strBuilder.Append("\ndefaultLoopType: ").Append(this._isRuntime ? DOTween.defaultLoopType : this._settings.defaultLoopType);
			GUILayout.Label(this._strBuilder.ToString(), new GUILayoutOption[0]);
			GUILayout.Space(10f);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003E6C File Offset: 0x0000206C
		private void ConnectToSource(bool forceReconnection = false)
		{
			this._headerImg = (AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/DOTweenIcon.png", typeof(Texture2D)) as Texture2D);
			if (this._settings == null || forceReconnection)
			{
				this._settings = (this._isRuntime ? (Resources.Load("DOTweenSettings") as DOTweenSettings) : DOTweenUtilityWindow.GetDOTweenSettings());
			}
		}

		// Token: 0x04000035 RID: 53
		private DOTweenSettings _settings;

		// Token: 0x04000036 RID: 54
		private string _title;

		// Token: 0x04000037 RID: 55
		private readonly StringBuilder _strBuilder = new StringBuilder();

		// Token: 0x04000038 RID: 56
		private bool _isRuntime;

		// Token: 0x04000039 RID: 57
		private Texture2D _headerImg;
	}
}
