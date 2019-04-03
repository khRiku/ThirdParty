// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.DOTweenSettings
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core.Enums;
using System;
using UnityEngine;

namespace DG.Tweening.Core
{
  public class DOTweenSettings : ScriptableObject
  {
    public bool useSafeMode = true;
    public float timeScale = 1f;
    public float maxSmoothUnscaledTime = 0.15f;
    public bool drawGizmos = true;
    public AutoPlay defaultAutoPlay = AutoPlay.All;
    public Ease defaultEaseType = Ease.OutQuad;
    public float defaultEaseOvershootOrAmplitude = 1.70158f;
    public bool defaultAutoKill = true;
    public DOTweenSettings.ModulesSetup modules = new DOTweenSettings.ModulesSetup();
    public const string AssetName = "DOTweenSettings";
    public bool useSmoothDeltaTime;
    public RewindCallbackMode rewindCallbackMode;
    public bool showUnityEditorReport;
    public LogBehaviour logBehaviour;
    public bool defaultRecyclable;
    public UpdateType defaultUpdateType;
    public bool defaultTimeScaleIndependent;
    public float defaultEasePeriod;
    public LoopType defaultLoopType;
    public DOTweenSettings.SettingsLocation storeSettingsLocation;
    public bool showPlayingTweens;
    public bool showPausedTweens;

    public enum SettingsLocation
    {
      AssetsDirectory,
      DOTweenDirectory,
      DemigiantDirectory,
    }

    [Serializable]
    public class ModulesSetup
    {
      public bool audioEnabled = true;
      public bool physicsEnabled = true;
      public bool physics2DEnabled = true;
      public bool spriteEnabled = true;
      public bool uiEnabled = true;
      public bool showPanel;
      public bool textMeshProEnabled;
      public bool tk2DEnabled;
    }
  }
}
