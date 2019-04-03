// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.DOTweenComponent
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System.Collections;
using System.Reflection;
using UnityEngine;

namespace DG.Tweening.Core
{
  /// <summary>
  /// Used to separate DOTween class from the MonoBehaviour instance (in order to use static constructors on DOTween).
  /// Contains all instance-based methods
  /// </summary>
  [AddComponentMenu("")]
  public class DOTweenComponent : MonoBehaviour, IDOTweenInit
  {
    /// <summary>Used internally inside Unity Editor, as a trick to update DOTween's inspector at every frame</summary>
    public int inspectorUpdater;
    private float _unscaledTime;
    private float _unscaledDeltaTime;
    private float _pausedTime;
    private bool _duplicateToDestroy;

    private void Awake()
    {
      if ((UnityEngine.Object) DOTween.instance == (UnityEngine.Object) null)
      {
        DOTween.instance = this;
        this.inspectorUpdater = 0;
        this._unscaledTime = Time.realtimeSinceStartup;
        System.Type looseScriptType = Utils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils");
        if (looseScriptType == null)
          Debugger.LogError((object) "Couldn't load Modules system");
        else
          looseScriptType.GetMethod("Init", BindingFlags.Static | BindingFlags.Public).Invoke((object) null, (object[]) null);
      }
      else
      {
        Debugger.LogWarning((object) "Duplicate DOTweenComponent instance found in scene: destroying it");
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }

    private void Start()
    {
      if (!((UnityEngine.Object) DOTween.instance != (UnityEngine.Object) this))
        return;
      this._duplicateToDestroy = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }

    private void Update()
    {
      this._unscaledDeltaTime = Time.realtimeSinceStartup - this._unscaledTime;
      if (DOTween.useSmoothDeltaTime && (double) this._unscaledDeltaTime > (double) DOTween.maxSmoothUnscaledTime)
        this._unscaledDeltaTime = DOTween.maxSmoothUnscaledTime;
      if (TweenManager.hasActiveDefaultTweens)
        TweenManager.Update(UpdateType.Normal, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
      this._unscaledTime = Time.realtimeSinceStartup;
      if (!TweenManager.isUnityEditor)
        return;
      ++this.inspectorUpdater;
      if (!DOTween.showUnityEditorReport || !TweenManager.hasActiveTweens)
        return;
      if (TweenManager.totActiveTweeners > DOTween.maxActiveTweenersReached)
        DOTween.maxActiveTweenersReached = TweenManager.totActiveTweeners;
      if (TweenManager.totActiveSequences <= DOTween.maxActiveSequencesReached)
        return;
      DOTween.maxActiveSequencesReached = TweenManager.totActiveSequences;
    }

    private void LateUpdate()
    {
      if (!TweenManager.hasActiveLateTweens)
        return;
      TweenManager.Update(UpdateType.Late, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
    }

    private void FixedUpdate()
    {
      if (!TweenManager.hasActiveFixedTweens || (double) Time.timeScale <= 0.0)
        return;
      TweenManager.Update(UpdateType.Fixed, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) / Time.timeScale * DOTween.timeScale);
    }

    private void OnDrawGizmos()
    {
      if (!DOTween.drawGizmos || !TweenManager.isUnityEditor)
        return;
      int count = DOTween.GizmosDelegates.Count;
      if (count == 0)
        return;
      for (int index = 0; index < count; ++index)
        DOTween.GizmosDelegates[index]();
    }

    private void OnDestroy()
    {
      if (this._duplicateToDestroy)
        return;
      if (DOTween.showUnityEditorReport)
        Debugger.LogReport((object) ("Max overall simultaneous active Tweeners/Sequences: " + (object) DOTween.maxActiveTweenersReached + "/" + (object) DOTween.maxActiveSequencesReached));
      if (!((UnityEngine.Object) DOTween.instance == (UnityEngine.Object) this))
        return;
      DOTween.instance = (DOTweenComponent) null;
    }

    public void OnApplicationPause(bool pauseStatus)
    {
      if (pauseStatus)
        this._pausedTime = Time.realtimeSinceStartup;
      else
        this._unscaledTime += Time.realtimeSinceStartup - this._pausedTime;
    }

    private void OnApplicationQuit()
    {
      DOTween.isQuitting = true;
    }

    /// <summary>
    /// Directly sets the current max capacity of Tweeners and Sequences
    /// (meaning how many Tweeners and Sequences can be running at the same time),
    /// so that DOTween doesn't need to automatically increase them in case the max is reached
    /// (which might lead to hiccups when that happens).
    /// Sequences capacity must be less or equal to Tweeners capacity
    /// (if you pass a low Tweener capacity it will be automatically increased to match the Sequence's).
    /// Beware: use this method only when there are no tweens running.
    /// </summary>
    /// <param name="tweenersCapacity">Max Tweeners capacity.
    /// Default: 200</param>
    /// <param name="sequencesCapacity">Max Sequences capacity.
    /// Default: 50</param>
    public IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity)
    {
      TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
      return (IDOTweenInit) this;
    }

    internal IEnumerator WaitForCompletion(Tween t)
    {
      while (t.active && !t.isComplete)
        yield return (object) null;
    }

    internal IEnumerator WaitForRewind(Tween t)
    {
      while (t.active && (!t.playedOnce || (double) t.position * (double) (t.completedLoops + 1) > 0.0))
        yield return (object) null;
    }

    internal IEnumerator WaitForKill(Tween t)
    {
      while (t.active)
        yield return (object) null;
    }

    internal IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
    {
      while (t.active && t.completedLoops < elapsedLoops)
        yield return (object) null;
    }

    internal IEnumerator WaitForPosition(Tween t, float position)
    {
      while (t.active && (double) t.position * (double) (t.completedLoops + 1) < (double) position)
        yield return (object) null;
    }

    internal IEnumerator WaitForStart(Tween t)
    {
      while (t.active && !t.playedOnce)
        yield return (object) null;
    }

    internal static void Create()
    {
      if ((UnityEngine.Object) DOTween.instance != (UnityEngine.Object) null)
        return;
      GameObject gameObject = new GameObject("[DOTween]");
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
      DOTween.instance = gameObject.AddComponent<DOTweenComponent>();
    }

    internal static void DestroyInstance()
    {
      if ((UnityEngine.Object) DOTween.instance != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) DOTween.instance.gameObject);
      DOTween.instance = (DOTweenComponent) null;
    }
  }
}
