// Decompiled with JetBrains decompiler
// Type: DG.Tweening.TweenExtensions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
  /// <summary>
  /// Methods that extend Tween objects and allow to control or get data from them
  /// </summary>
  public static class TweenExtensions
  {
    /// <summary>Completes the tween</summary>
    public static void Complete(this Tween t)
    {
      t.Complete(false);
    }

    /// <summary>Completes the tween</summary>
    /// <param name="withCallbacks">For Sequences only: if TRUE also internal Sequence callbacks will be fired,
    /// otherwise they will be ignored</param>
    public static void Complete(this Tween t, bool withCallbacks)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
      {
        UpdateMode updateMode = TweenManager.isUpdateLoop ? UpdateMode.IgnoreOnComplete : (withCallbacks ? UpdateMode.Update : UpdateMode.Goto);
        TweenManager.Complete(t, true, updateMode);
      }
    }

    /// <summary>Flips the direction of this tween (backwards if it was going forward or viceversa)</summary>
    public static void Flip(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.Flip(t);
    }

    /// <summary>Forces the tween to initialize its settings immediately</summary>
    public static void ForceInit(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.ForceInit(t, false);
    }

    /// <summary>Send the tween to the given position in time</summary>
    /// <param name="to">Time position to reach
    /// (if higher than the whole tween duration the tween will simply reach its end)</param>
    /// <param name="andPlay">If TRUE will play the tween after reaching the given position, otherwise it will pause it</param>
    public static void Goto(this Tween t, float to, bool andPlay = false)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
      {
        if ((double) to < 0.0)
          to = 0.0f;
        TweenManager.Goto(t, to, andPlay, UpdateMode.Goto);
      }
    }

    /// <summary>Kills the tween</summary>
    /// <param name="complete">If TRUE completes the tween before killing it</param>
    public static void Kill(this Tween t, bool complete = false)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
      {
        if (complete)
        {
          TweenManager.Complete(t, true, UpdateMode.Goto);
          if (t.autoKill && t.loops >= 0)
            return;
        }
        if (TweenManager.isUpdateLoop)
          t.active = false;
        else
          TweenManager.Despawn(t, true);
      }
    }

    /// <summary>Pauses the tween</summary>
    public static T Pause<T>(this T t) where T : Tween
    {
      if ((object) t == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNullTween((Tween) t);
        return t;
      }
      if (!t.active)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogInvalidTween((Tween) t);
        return t;
      }
      if (t.isSequenced)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNestedTween((Tween) t);
        return t;
      }
      TweenManager.Pause((Tween) t);
      return t;
    }

    /// <summary>Plays the tween</summary>
    public static T Play<T>(this T t) where T : Tween
    {
      if ((object) t == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNullTween((Tween) t);
        return t;
      }
      if (!t.active)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogInvalidTween((Tween) t);
        return t;
      }
      if (t.isSequenced)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNestedTween((Tween) t);
        return t;
      }
      TweenManager.Play((Tween) t);
      return t;
    }

    /// <summary>Sets the tween in a backwards direction and plays it</summary>
    public static void PlayBackwards(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.PlayBackwards(t);
    }

    /// <summary>Sets the tween in a forward direction and plays it</summary>
    public static void PlayForward(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.PlayForward(t);
    }

    /// <summary>Restarts the tween from the beginning</summary>
    /// <param name="includeDelay">If TRUE includes the eventual tween delay, otherwise skips it</param>
    /// <param name="changeDelayTo">If &gt;= 0 changes the startup delay to this value, otherwise doesn't touch it</param>
    public static void Restart(this Tween t, bool includeDelay = true, float changeDelayTo = -1f)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.Restart(t, includeDelay, changeDelayTo);
    }

    /// <summary>Rewinds and pauses the tween</summary>
    /// <param name="includeDelay">If TRUE includes the eventual tween delay, otherwise skips it</param>
    public static void Rewind(this Tween t, bool includeDelay = true)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.Rewind(t, includeDelay);
    }

    /// <summary>Smoothly rewinds the tween (delays excluded).
    /// A "smooth rewind" animates the tween to its start position,
    /// skipping all elapsed loops (except in case of LoopType.Incremental) while keeping the animation fluent.
    /// If called on a tween who is still waiting for its delay to happen, it will simply set the delay to 0 and pause the tween.
    /// <para>Note that a tween that was smoothly rewinded will have its play direction flipped</para></summary>
    public static void SmoothRewind(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.SmoothRewind(t);
    }

    /// <summary>Plays the tween if it was paused, pauses it if it was playing</summary>
    public static void TogglePause(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
        TweenManager.TogglePause(t);
    }

    /// <summary>Send a path tween to the given waypoint.
    /// Has no effect if this is not a path tween.
    /// <para>BEWARE, this is a special utility method:
    /// it works only with Linear eases. Also, the lookAt direction might be wrong after calling this and might need to be set manually
    /// (because it relies on a smooth path movement and doesn't work well with jumps that encompass dramatic direction changes)</para></summary>
    /// <param name="waypointIndex">Waypoint index to reach
    /// (if higher than the max waypoint index the tween will simply go to the last one)</param>
    /// <param name="andPlay">If TRUE will play the tween after reaching the given waypoint, otherwise it will pause it</param>
    public static void GotoWaypoint(this Tween t, int waypointIndex, bool andPlay = false)
    {
      if (t == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(t);
      }
      else if (!t.active)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogInvalidTween(t);
      }
      else if (t.isSequenced)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNestedTween(t);
      }
      else
      {
        TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
        if (tweenerCore == null)
        {
          if (Debugger.logPriority <= 1)
            return;
          Debugger.LogNonPathTween(t);
        }
        else
        {
          if (!t.startupDone)
            TweenManager.ForceInit(t, false);
          if (waypointIndex < 0)
            waypointIndex = 0;
          else if (waypointIndex > tweenerCore.changeValue.wps.Length - 1)
            waypointIndex = tweenerCore.changeValue.wps.Length - 1;
          float num1 = 0.0f;
          for (int index = 0; index < waypointIndex + 1; ++index)
            num1 += tweenerCore.changeValue.wpLengths[index];
          float num2 = num1 / tweenerCore.changeValue.length;
          if ((t.loopType != LoopType.Yoyo ? 0 : ((double) t.position < (double) t.duration ? ((uint) (t.completedLoops % 2) > 0U ? 1 : 0) : (t.completedLoops % 2 == 0 ? 1 : 0))) != 0)
            num2 = 1f - num2;
          float to = (float) ((t.isComplete ? (double) (t.completedLoops - 1) : (double) t.completedLoops) * (double) t.duration + (double) num2 * (double) t.duration);
          TweenManager.Goto(t, to, andPlay, UpdateMode.Goto);
        }
      }
    }

    /// <summary>
    /// Creates a yield instruction that waits until the tween is killed or complete.
    /// It can be used inside a coroutine as a yield.
    /// <para>Example usage:</para><code>yield return myTween.WaitForCompletion();</code>
    /// </summary>
    public static YieldInstruction WaitForCompletion(this Tween t)
    {
      if (t.active)
        return (YieldInstruction) DOTween.instance.StartCoroutine(DOTween.instance.WaitForCompletion(t));
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return (YieldInstruction) null;
    }

    /// <summary>
    /// Creates a yield instruction that waits until the tween is killed or rewinded.
    /// It can be used inside a coroutine as a yield.
    /// <para>Example usage:</para><code>yield return myTween.WaitForRewind();</code>
    /// </summary>
    public static YieldInstruction WaitForRewind(this Tween t)
    {
      if (t.active)
        return (YieldInstruction) DOTween.instance.StartCoroutine(DOTween.instance.WaitForRewind(t));
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return (YieldInstruction) null;
    }

    /// <summary>
    /// Creates a yield instruction that waits until the tween is killed.
    /// It can be used inside a coroutine as a yield.
    /// <para>Example usage:</para><code>yield return myTween.WaitForKill();</code>
    /// </summary>
    public static YieldInstruction WaitForKill(this Tween t)
    {
      if (t.active)
        return (YieldInstruction) DOTween.instance.StartCoroutine(DOTween.instance.WaitForKill(t));
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return (YieldInstruction) null;
    }

    /// <summary>
    /// Creates a yield instruction that waits until the tween is killed or has gone through the given amount of loops.
    /// It can be used inside a coroutine as a yield.
    /// <para>Example usage:</para><code>yield return myTween.WaitForElapsedLoops(2);</code>
    /// </summary>
    /// <param name="elapsedLoops">Elapsed loops to wait for</param>
    public static YieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops)
    {
      if (t.active)
        return (YieldInstruction) DOTween.instance.StartCoroutine(DOTween.instance.WaitForElapsedLoops(t, elapsedLoops));
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return (YieldInstruction) null;
    }

    /// <summary>
    /// Creates a yield instruction that waits until the tween is killed or has reached the given position (loops included, delays excluded).
    /// It can be used inside a coroutine as a yield.
    /// <para>Example usage:</para><code>yield return myTween.WaitForPosition(2.5f);</code>
    /// </summary>
    /// <param name="position">Position (loops included, delays excluded) to wait for</param>
    public static YieldInstruction WaitForPosition(this Tween t, float position)
    {
      if (t.active)
        return (YieldInstruction) DOTween.instance.StartCoroutine(DOTween.instance.WaitForPosition(t, position));
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return (YieldInstruction) null;
    }

    /// <summary>
    /// Creates a yield instruction that waits until the tween is killed or started
    /// (meaning when the tween is set in a playing state the first time, after any eventual delay).
    /// It can be used inside a coroutine as a yield.
    /// <para>Example usage:</para><code>yield return myTween.WaitForStart();</code>
    /// </summary>
    public static Coroutine WaitForStart(this Tween t)
    {
      if (t.active)
        return DOTween.instance.StartCoroutine(DOTween.instance.WaitForStart(t));
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return (Coroutine) null;
    }

    /// <summary>Returns the total number of loops completed by this tween</summary>
    public static int CompletedLoops(this Tween t)
    {
      if (t.active)
        return t.completedLoops;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return 0;
    }

    /// <summary>Returns the eventual delay set for this tween</summary>
    public static float Delay(this Tween t)
    {
      if (t.active)
        return t.delay;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return 0.0f;
    }

    /// <summary>Returns the duration of this tween (delays excluded).
    /// <para>NOTE: when using settings like SpeedBased, the duration will be recalculated when the tween starts</para></summary>
    /// <param name="includeLoops">If TRUE returns the full duration loops included,
    /// otherwise the duration of a single loop cycle</param>
    public static float Duration(this Tween t, bool includeLoops = true)
    {
      if (!t.active)
      {
        if (Debugger.logPriority > 0)
          Debugger.LogInvalidTween(t);
        return 0.0f;
      }
      if (!includeLoops)
        return t.duration;
      if (t.loops != -1)
        return t.duration * (float) t.loops;
      return float.PositiveInfinity;
    }

    /// <summary>Returns the elapsed time for this tween (delays exluded)</summary>
    /// <param name="includeLoops">If TRUE returns the elapsed time since startup loops included,
    /// otherwise the elapsed time within the current loop cycle</param>
    public static float Elapsed(this Tween t, bool includeLoops = true)
    {
      if (!t.active)
      {
        if (Debugger.logPriority > 0)
          Debugger.LogInvalidTween(t);
        return 0.0f;
      }
      if (includeLoops)
        return ((double) t.position >= (double) t.duration ? (float) (t.completedLoops - 1) : (float) t.completedLoops) * t.duration + t.position;
      return t.position;
    }

    /// <summary>Returns the elapsed percentage (0 to 1) of this tween (delays exluded)</summary>
    /// <param name="includeLoops">If TRUE returns the elapsed percentage since startup loops included,
    /// otherwise the elapsed percentage within the current loop cycle</param>
    public static float ElapsedPercentage(this Tween t, bool includeLoops = true)
    {
      if (!t.active)
      {
        if (Debugger.logPriority > 0)
          Debugger.LogInvalidTween(t);
        return 0.0f;
      }
      if (!includeLoops)
        return t.position / t.duration;
      if ((double) t.fullDuration <= 0.0)
        return 0.0f;
      return (((double) t.position >= (double) t.duration ? (float) (t.completedLoops - 1) : (float) t.completedLoops) * t.duration + t.position) / t.fullDuration;
    }

    /// <summary>Returns the elapsed percentage (0 to 1) of this tween (delays exluded),
    /// based on a single loop, and calculating eventual backwards Yoyo loops as 1 to 0 instead of 0 to 1</summary>
    public static float ElapsedDirectionalPercentage(this Tween t)
    {
      if (!t.active)
      {
        if (Debugger.logPriority > 0)
          Debugger.LogInvalidTween(t);
        return 0.0f;
      }
      float num = t.position / t.duration;
      if ((t.completedLoops <= 0 || t.loopType != LoopType.Yoyo ? 0 : (t.isComplete || t.completedLoops % 2 == 0 ? (!t.isComplete ? 0 : (t.completedLoops % 2 == 0 ? 1 : 0)) : 1)) == 0)
        return num;
      return 1f - num;
    }

    /// <summary>Returns FALSE if this tween has been killed.
    /// <para>BEWARE: if this tween is recyclable it might have been spawned again for another use and thus return TRUE anyway.</para>
    /// When working with recyclable tweens you should take care to know when a tween has been killed and manually set your references to NULL.
    /// If you want to be sure your references are set to NULL when a tween is killed you can use the <code>OnKill</code> callback like this:
    /// <para><code>.OnKill(()=&gt; myTweenReference = null)</code></para></summary>
    public static bool IsActive(this Tween t)
    {
      return t.active;
    }

    /// <summary>Returns TRUE if this tween was reversed and is set to go backwards</summary>
    public static bool IsBackwards(this Tween t)
    {
      if (t.active)
        return t.isBackwards;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return false;
    }

    /// <summary>Returns TRUE if the tween is complete
    /// (silently fails and returns FALSE if the tween has been killed)</summary>
    public static bool IsComplete(this Tween t)
    {
      if (t.active)
        return t.isComplete;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return false;
    }

    /// <summary>Returns TRUE if this tween has been initialized</summary>
    public static bool IsInitialized(this Tween t)
    {
      if (t.active)
        return t.startupDone;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return false;
    }

    /// <summary>Returns TRUE if this tween is playing</summary>
    public static bool IsPlaying(this Tween t)
    {
      if (t.active)
        return t.isPlaying;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return false;
    }

    /// <summary>Returns the total number of loops set for this tween
    /// (returns -1 if the loops are infinite)</summary>
    public static int Loops(this Tween t)
    {
      if (t.active)
        return t.loops;
      if (Debugger.logPriority > 0)
        Debugger.LogInvalidTween(t);
      return 0;
    }

    /// <summary>
    /// Returns a point on a path based on the given path percentage.
    /// Returns <code>Vector3.zero</code> if this is not a path tween, if the tween is invalid, or if the path is not yet initialized.
    /// A path is initialized after its tween starts, or immediately if the tween was created with the Path Editor (DOTween Pro feature).
    /// You can force a path to be initialized by calling <code>myTween.ForceInit()</code>.
    /// </summary>
    /// <param name="pathPercentage">Percentage of the path (0 to 1) on which to get the point</param>
    public static Vector3 PathGetPoint(this Tween t, float pathPercentage)
    {
      if ((double) pathPercentage > 1.0)
        pathPercentage = 1f;
      else if ((double) pathPercentage < 0.0)
        pathPercentage = 0.0f;
      if (t == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNullTween(t);
        return Vector3.zero;
      }
      if (!t.active)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogInvalidTween(t);
        return Vector3.zero;
      }
      if (t.isSequenced)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNestedTween(t);
        return Vector3.zero;
      }
      TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
      if (tweenerCore == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNonPathTween(t);
        return Vector3.zero;
      }
      if (tweenerCore.endValue.isFinalized)
        return tweenerCore.endValue.GetPoint(pathPercentage, true);
      if (Debugger.logPriority > 1)
        Debugger.LogWarning((object) "The path is not finalized yet");
      return Vector3.zero;
    }

    /// <summary>
    /// Returns an array of points that can be used to draw the path.
    /// Note that this method generates allocations, because it creates a new array.
    /// Returns <code>NULL</code> if this is not a path tween, if the tween is invalid, or if the path is not yet initialized.
    /// A path is initialized after its tween starts, or immediately if the tween was created with the Path Editor (DOTween Pro feature).
    /// You can force a path to be initialized by calling <code>myTween.ForceInit()</code>.
    /// </summary>
    /// <param name="subdivisionsXSegment">How many points to create for each path segment (waypoint to waypoint).
    /// Only used in case of non-Linear paths</param>
    public static Vector3[] PathGetDrawPoints(this Tween t, int subdivisionsXSegment = 10)
    {
      if (t == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNullTween(t);
        return (Vector3[]) null;
      }
      if (!t.active)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogInvalidTween(t);
        return (Vector3[]) null;
      }
      if (t.isSequenced)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNestedTween(t);
        return (Vector3[]) null;
      }
      TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
      if (tweenerCore == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNonPathTween(t);
        return (Vector3[]) null;
      }
      if (tweenerCore.endValue.isFinalized)
        return Path.GetDrawPoints(tweenerCore.endValue, subdivisionsXSegment);
      if (Debugger.logPriority > 1)
        Debugger.LogWarning((object) "The path is not finalized yet");
      return (Vector3[]) null;
    }

    /// <summary>
    /// Returns the length of a path.
    /// Returns -1 if this is not a path tween, if the tween is invalid, or if the path is not yet initialized.
    /// A path is initialized after its tween starts, or immediately if the tween was created with the Path Editor (DOTween Pro feature).
    /// You can force a path to be initialized by calling <code>myTween.ForceInit()</code>.
    /// </summary>
    public static float PathLength(this Tween t)
    {
      if (t == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNullTween(t);
        return -1f;
      }
      if (!t.active)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogInvalidTween(t);
        return -1f;
      }
      if (t.isSequenced)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNestedTween(t);
        return -1f;
      }
      TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
      if (tweenerCore == null)
      {
        if (Debugger.logPriority > 1)
          Debugger.LogNonPathTween(t);
        return -1f;
      }
      if (tweenerCore.endValue.isFinalized)
        return tweenerCore.endValue.length;
      if (Debugger.logPriority > 1)
        Debugger.LogWarning((object) "The path is not finalized yet");
      return -1f;
    }
  }
}
