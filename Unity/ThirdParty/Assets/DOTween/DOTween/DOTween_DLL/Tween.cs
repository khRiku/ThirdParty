// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Tween
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using System;

namespace DG.Tweening
{
  /// <summary>Indicates either a Tweener or a Sequence</summary>
  public abstract class Tween : ABSSequentiable
  {
    /// <summary>Int ID (usable for filtering with DOTween static methods). 4X faster than using an object id, 2X faster than using a string id.
    /// Default is -999 so avoid using an ID like that or it will capture all unset intIds</summary>
    public int intId = -999;
    public int activeId = -1;
    public bool delayComplete = true;
    public int miscInt = -1;
    /// <summary>TimeScale for the tween</summary>
    public float timeScale;
    /// <summary>If TRUE the tween wil go backwards</summary>
    public bool isBackwards;
    /// <summary>Object ID (usable for filtering with DOTween static methods). Can be anything except a string or an int
    /// (use <see cref="F:DG.Tweening.Tween.stringId" /> or <see cref="F:DG.Tweening.Tween.intId" /> for those)</summary>
    public object id;
    /// <summary>String ID (usable for filtering with DOTween static methods). 2X faster than using an object id</summary>
    public string stringId;
    /// <summary>Tween target (usable for filtering with DOTween static methods). Automatically set by tween creation shorcuts</summary>
    public object target;
    public UpdateType updateType;
    public bool isIndependentUpdate;
    /// <summary>Called when the tween is set in a playing state, after any eventual delay.
    /// Also called each time the tween resumes playing from a paused state</summary>
    public TweenCallback onPlay;
    /// <summary>Called when the tween state changes from playing to paused.
    /// If the tween has autoKill set to FALSE, this is called also when the tween reaches completion.</summary>
    public TweenCallback onPause;
    /// <summary>Called when the tween is rewinded,
    /// either by calling <code>Rewind</code> or by reaching the start position while playing backwards.
    /// Rewinding a tween that is already rewinded will not fire this callback</summary>
    public TweenCallback onRewind;
    /// <summary>Called each time the tween updates</summary>
    public TweenCallback onUpdate;
    /// <summary>Called the moment the tween completes one loop cycle</summary>
    public TweenCallback onStepComplete;
    /// <summary>Called the moment the tween reaches completion (loops included)</summary>
    public TweenCallback onComplete;
    /// <summary>Called the moment the tween is killed</summary>
    public TweenCallback onKill;
    /// <summary>Called when a path tween's current waypoint changes</summary>
    public TweenCallback<int> onWaypointChange;
    public bool isFrom;
    public bool isBlendable;
    public bool isRecyclable;
    public bool isSpeedBased;
    public bool autoKill;
    public float duration;
    public int loops;
    public LoopType loopType;
    public float delay;
    public Ease easeType;
    public EaseFunction customEase;
    public float easeOvershootOrAmplitude;
    public float easePeriod;
    public Type typeofT1;
    public Type typeofT2;
    public Type typeofTPlugOptions;
    public bool isSequenced;
    public Sequence sequenceParent;
    public SpecialStartupMode specialStartupMode;
    public bool creationLocked;
    public bool startupDone;
    public float fullDuration;
    public int completedLoops;
    public bool isPlaying;
    public bool isComplete;
    public float elapsedDelay;

    /// <summary>Tweeners-only (ignored by Sequences), returns TRUE if the tween was set as relative</summary>
    public bool isRelative { get;  set; }

    /// <summary>FALSE when tween is (or should be) despawned - set only by TweenManager</summary>
    public bool active { get;  set; }

    /// <summary>Gets and sets the time position (loops included, delays excluded) of the tween</summary>
    public float fullPosition
    {
      get
      {
        return this.Elapsed(true);
      }
      set
      {
        this.Goto(value, this.isPlaying);
      }
    }

    /// <summary>TRUE after the tween was set in a play state at least once, AFTER any delay is elapsed</summary>
    public bool playedOnce { get; private set; }

    /// <summary>Time position within a single loop cycle</summary>
    public float position { get;  set; }

    public virtual void Reset()
    {
      this.timeScale = 1f;
      this.isBackwards = false;
      this.id = (object) null;
      this.stringId = (string) null;
      this.intId = -999;
      this.isIndependentUpdate = false;
      this.onStart = this.onPlay = this.onRewind = this.onUpdate = this.onComplete = this.onStepComplete = this.onKill = (TweenCallback) null;
      this.onWaypointChange = (TweenCallback<int>) null;
      this.target = (object) null;
      this.isFrom = false;
      this.isBlendable = false;
      this.isSpeedBased = false;
      this.duration = 0.0f;
      this.loops = 1;
      this.delay = 0.0f;
      this.isRelative = false;
      this.customEase = (EaseFunction) null;
      this.isSequenced = false;
      this.sequenceParent = (Sequence) null;
      this.specialStartupMode = SpecialStartupMode.None;
      this.creationLocked = this.startupDone = this.playedOnce = false;
      this.position = this.fullDuration = (float) (this.completedLoops = 0);
      this.isPlaying = this.isComplete = false;
      this.elapsedDelay = 0.0f;
      this.delayComplete = true;
      this.miscInt = -1;
    }

    public abstract bool Validate();

    public virtual float UpdateDelay(float elapsed)
    {
      return 0.0f;
    }

    public abstract bool Startup();

    public abstract bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice);

    public static bool DoGoto(Tween t, float toPosition, int toCompletedLoops, UpdateMode updateMode)
    {
      if (!t.startupDone && !t.Startup())
        return true;
      if (!t.playedOnce && updateMode == UpdateMode.Update)
      {
        t.playedOnce = true;
        if (t.onStart != null)
        {
          Tween.OnTweenCallback(t.onStart);
          if (!t.active)
            return true;
        }
        if (t.onPlay != null)
        {
          Tween.OnTweenCallback(t.onPlay);
          if (!t.active)
            return true;
        }
      }
      float position = t.position;
      int completedLoops = t.completedLoops;
      t.completedLoops = toCompletedLoops;
      bool flag = (double) t.position <= 0.0 && completedLoops <= 0;
      bool isComplete = t.isComplete;
      if (t.loops != -1)
        t.isComplete = t.completedLoops == t.loops;
      int newCompletedSteps = 0;
      if (updateMode == UpdateMode.Update)
      {
        if (t.isBackwards)
        {
          newCompletedSteps = t.completedLoops < completedLoops ? completedLoops - t.completedLoops : ((double) toPosition > 0.0 || flag ? 0 : 1);
          if (isComplete)
            --newCompletedSteps;
        }
        else
          newCompletedSteps = t.completedLoops > completedLoops ? t.completedLoops - completedLoops : 0;
      }
      else if (t.tweenType == TweenType.Sequence)
      {
        newCompletedSteps = completedLoops - toCompletedLoops;
        if (newCompletedSteps < 0)
          newCompletedSteps = -newCompletedSteps;
      }
      t.position = toPosition;
      if ((double) t.position > (double) t.duration)
        t.position = t.duration;
      else if ((double) t.position <= 0.0)
        t.position = t.completedLoops > 0 || t.isComplete ? t.duration : 0.0f;
      bool isPlaying = t.isPlaying;
      if (t.isPlaying)
        t.isPlaying = t.isBackwards ? t.completedLoops != 0 || (double) t.position > 0.0 : !t.isComplete;
      bool useInversePosition = t.loopType == LoopType.Yoyo && ((double) t.position < (double) t.duration ? (uint) (t.completedLoops % 2) > 0U : t.completedLoops % 2 == 0);
      UpdateNotice updateNotice = (flag ? 0 : (t.loopType != LoopType.Restart || t.completedLoops == completedLoops || t.loops != -1 && t.completedLoops >= t.loops ? ((double) t.position > 0.0 ? 0 : (t.completedLoops <= 0 ? 1 : 0)) : 1)) != 0 ? UpdateNotice.RewindStep : UpdateNotice.None;
      if (t.ApplyTween(position, completedLoops, newCompletedSteps, useInversePosition, updateMode, updateNotice))
        return true;
      if (t.onUpdate != null && updateMode != UpdateMode.IgnoreOnUpdate)
        Tween.OnTweenCallback(t.onUpdate);
      if ((double) t.position <= 0.0 && t.completedLoops <= 0 && (!flag && t.onRewind != null))
        Tween.OnTweenCallback(t.onRewind);
      if (newCompletedSteps > 0 && updateMode == UpdateMode.Update && t.onStepComplete != null)
      {
        for (int index = 0; index < newCompletedSteps; ++index)
          Tween.OnTweenCallback(t.onStepComplete);
      }
      if (t.isComplete && !isComplete && (updateMode != UpdateMode.IgnoreOnComplete && t.onComplete != null))
        Tween.OnTweenCallback(t.onComplete);
      if (!t.isPlaying & isPlaying && (!t.isComplete || !t.autoKill) && t.onPause != null)
        Tween.OnTweenCallback(t.onPause);
      if (t.autoKill)
        return t.isComplete;
      return false;
    }

    public static bool OnTweenCallback(TweenCallback callback)
    {
      if (DOTween.useSafeMode)
      {
        try
        {
          callback();
        }
        catch (Exception ex)
        {
          Debugger.LogWarning((object) string.Format("An error inside a tween callback was silently taken care of ({0}) ► {1}\n\n{2}\n\n", (object) ex.TargetSite, (object) ex.Message, (object) ex.StackTrace));
          return false;
        }
      }
      else
        callback();
      return true;
    }

    public static bool OnTweenCallback<T>(TweenCallback<T> callback, T param)
    {
      if (DOTween.useSafeMode)
      {
        try
        {
          callback(param);
        }
        catch (Exception ex)
        {
          Debugger.LogWarning((object) string.Format("An error inside a tween callback was silently taken care of ({0}) ► {1}", (object) ex.TargetSite, (object) ex.Message));
          return false;
        }
      }
      else
        callback(param);
      return true;
    }
  }
}
