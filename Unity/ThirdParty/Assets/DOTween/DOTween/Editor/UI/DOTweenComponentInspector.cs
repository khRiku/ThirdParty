// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.UI.DOTweenComponentInspector
// Assembly: DOTweenEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5921EEE1-1EAD-4AE3-AEC3-8051606D5E53
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\Editor\DOTweenEditor.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
  [CustomEditor(typeof (DOTweenComponent))]
  public class DOTweenComponentInspector : Editor
  {
    private readonly StringBuilder _strBuilder = new StringBuilder();
    private DOTweenSettings _settings;
    private string _title;
    private bool _isRuntime;
    private Texture2D _headerImg;

    private void OnEnable()
    {
      this._isRuntime = EditorApplication.isPlaying;
      this.ConnectToSource(true);
      this._strBuilder.Remove(0, this._strBuilder.Length);
      this._strBuilder.Append("DOTween v").Append(DOTween.Version);
      if (TweenManager.isDebugBuild)
        this._strBuilder.Append(" [Debug build]");
      else
        this._strBuilder.Append(" [Release build]");
      if (EditorUtils.hasPro)
        this._strBuilder.Append("\nDOTweenPro v").Append(EditorUtils.proVersion);
      else
        this._strBuilder.Append("\nDOTweenPro not installed");
      this._title = this._strBuilder.ToString();
    }

    public override void OnInspectorGUI()
    {
      this._isRuntime = EditorApplication.isPlaying;
      this.ConnectToSource(false);
      EditorGUIUtils.SetGUIStyles(new Vector2?());
      GUILayout.Space(4f);
      GUILayout.BeginHorizontal();
      GUI.DrawTexture(GUILayoutUtility.GetRect(0.0f, 93f, 18f, 18f), (Texture) this._headerImg, ScaleMode.ScaleToFit, true);
      GUILayout.Label(this._isRuntime ? "RUNTIME MODE" : "EDITOR MODE");
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      int totActiveTweens = TweenManager.totActiveTweens;
      int num1 = TweenManager.TotalPlayingTweens();
      int num2 = totActiveTweens - num1;
      int activeDefaultTweens = TweenManager.totActiveDefaultTweens;
      int activeLateTweens = TweenManager.totActiveLateTweens;
      GUILayout.Label(this._title, TweenManager.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle, new GUILayoutOption[0]);
      if (!this._isRuntime)
      {
        GUI.backgroundColor = new Color(0.0f, 0.31f, 0.48f);
        GUI.contentColor = Color.white;
        GUILayout.Label("This component is <b>added automatically</b> by DOTween at runtime.\nAdding it yourself is <b>not recommended</b> unless you really know what you're doing: you'll have to be sure it's <b>never destroyed</b> and that it's present <b>in every scene</b>.", EditorGUIUtils.infoboxStyle, new GUILayoutOption[0]);
        Color white;
        GUI.contentColor = white = Color.white;
        GUI.contentColor = white;
        GUI.backgroundColor = white;
      }
      GUILayout.Space(6f);
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Documentation"))
        Application.OpenURL("http://dotween.demigiant.com/documentation.php");
      if (GUILayout.Button("Check Updates"))
        Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
      GUILayout.EndHorizontal();
      if (this._isRuntime)
      {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(this._settings.showPlayingTweens ? "Hide Playing Tweens" : "Show Playing Tweens"))
        {
          this._settings.showPlayingTweens = !this._settings.showPlayingTweens;
          EditorUtility.SetDirty((Object) this._settings);
        }
        if (GUILayout.Button(this._settings.showPausedTweens ? "Hide Paused Tweens" : "Show Paused Tweens"))
        {
          this._settings.showPausedTweens = !this._settings.showPausedTweens;
          EditorUtility.SetDirty((Object) this._settings);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Play all"))
          DOTween.PlayAll();
        if (GUILayout.Button("Pause all"))
          DOTween.PauseAll();
        if (GUILayout.Button("Kill all"))
          DOTween.KillAll(false);
        GUILayout.EndHorizontal();
        GUILayout.Space(8f);
        this._strBuilder.Length = 0;
        this._strBuilder.Append("Active tweens: ").Append(totActiveTweens).Append(" (").Append(TweenManager.totActiveTweeners).Append("/").Append(TweenManager.totActiveSequences).Append(")").Append("\nDefault/Late tweens: ").Append(activeDefaultTweens).Append("/").Append(activeLateTweens).Append("\nPlaying tweens: ").Append(num1);
        if (this._settings.showPlayingTweens)
        {
          foreach (Tween activeTween in TweenManager._activeTweens)
          {
            if (activeTween != null && activeTween.isPlaying)
              this._strBuilder.Append("\n   - [").Append((object) activeTween.tweenType).Append("] ").Append(activeTween.target);
          }
        }
        this._strBuilder.Append("\nPaused tweens: ").Append(num2);
        if (this._settings.showPausedTweens)
        {
          foreach (Tween activeTween in TweenManager._activeTweens)
          {
            if (activeTween != null && !activeTween.isPlaying)
              this._strBuilder.Append("\n   - [").Append((object) activeTween.tweenType).Append("] ").Append(activeTween.target);
          }
        }
        this._strBuilder.Append("\nPooled tweens: ").Append(TweenManager.TotalPooledTweens()).Append(" (").Append(TweenManager.totPooledTweeners).Append("/").Append(TweenManager.totPooledSequences).Append(")");
        GUILayout.Label(this._strBuilder.ToString());
        GUILayout.Space(8f);
        this._strBuilder.Remove(0, this._strBuilder.Length);
        this._strBuilder.Append("Tweens Capacity: ").Append(TweenManager.maxTweeners).Append("/").Append(TweenManager.maxSequences).Append("\nMax Simultaneous Active Tweens: ").Append(DOTween.maxActiveTweenersReached).Append("/").Append(DOTween.maxActiveSequencesReached);
        GUILayout.Label(this._strBuilder.ToString());
      }
      GUILayout.Space(8f);
      this._strBuilder.Remove(0, this._strBuilder.Length);
      this._strBuilder.Append("SETTINGS ▼");
      this._strBuilder.Append("\nSafe Mode: ").Append((this._isRuntime ? (DOTween.useSafeMode ? 1 : 0) : (this._settings.useSafeMode ? 1 : 0)) != 0 ? "ON" : "OFF");
      this._strBuilder.Append("\nLog Behaviour: ").Append((object) (LogBehaviour) (this._isRuntime ? (int) DOTween.logBehaviour : (int) this._settings.logBehaviour));
      this._strBuilder.Append("\nShow Unity Editor Report: ").Append(this._isRuntime ? DOTween.showUnityEditorReport : this._settings.showUnityEditorReport);
      this._strBuilder.Append("\nTimeScale (Unity/DOTween): ").Append(Time.timeScale).Append("/").Append(this._isRuntime ? DOTween.timeScale : this._settings.timeScale);
      GUILayout.Label(this._strBuilder.ToString());
      GUILayout.Label("NOTE: DOTween's TimeScale is not the same as Unity's Time.timeScale: it is actually multiplied by it except for tweens that are set to update independently", EditorGUIUtils.wordWrapItalicLabelStyle, new GUILayoutOption[0]);
      GUILayout.Space(8f);
      this._strBuilder.Remove(0, this._strBuilder.Length);
      this._strBuilder.Append("DEFAULTS ▼");
      this._strBuilder.Append("\ndefaultRecyclable: ").Append(this._isRuntime ? DOTween.defaultRecyclable : this._settings.defaultRecyclable);
      this._strBuilder.Append("\ndefaultUpdateType: ").Append((object) (UpdateType) (this._isRuntime ? (int) DOTween.defaultUpdateType : (int) this._settings.defaultUpdateType));
      this._strBuilder.Append("\ndefaultTSIndependent: ").Append(this._isRuntime ? DOTween.defaultTimeScaleIndependent : this._settings.defaultTimeScaleIndependent);
      this._strBuilder.Append("\ndefaultAutoKill: ").Append(this._isRuntime ? DOTween.defaultAutoKill : this._settings.defaultAutoKill);
      this._strBuilder.Append("\ndefaultAutoPlay: ").Append((object) (AutoPlay) (this._isRuntime ? (int) DOTween.defaultAutoPlay : (int) this._settings.defaultAutoPlay));
      this._strBuilder.Append("\ndefaultEaseType: ").Append((object) (Ease) (this._isRuntime ? (int) DOTween.defaultEaseType : (int) this._settings.defaultEaseType));
      this._strBuilder.Append("\ndefaultLoopType: ").Append((object) (LoopType) (this._isRuntime ? (int) DOTween.defaultLoopType : (int) this._settings.defaultLoopType));
      GUILayout.Label(this._strBuilder.ToString());
      GUILayout.Space(10f);
    }

    private void ConnectToSource(bool forceReconnection = false)
    {
      this._headerImg = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/DOTweenIcon.png", typeof (Texture2D)) as Texture2D;
      if (!((Object) this._settings == (Object) null | forceReconnection))
        return;
      this._settings = this._isRuntime ? Resources.Load("DOTweenSettings") as DOTweenSettings : DOTweenUtilityWindow.GetDOTweenSettings();
    }
  }
}
