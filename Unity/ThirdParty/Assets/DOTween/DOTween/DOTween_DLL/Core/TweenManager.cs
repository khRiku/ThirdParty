// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.TweenManager
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening.Core
{
  public static class TweenManager
  {
    public static int maxActive = 250;
    public static int maxTweeners = 200;
    public static int maxSequences = 50;
    public static Tween[] _activeTweens = new Tween[250];
    private static Tween[] _pooledTweeners = new Tween[200];
    private static readonly Stack<Tween> _PooledSequences = new Stack<Tween>();
    private static readonly List<Tween> _KillList = new List<Tween>(250);
    private static int _maxActiveLookupId = -1;
    private static int _reorganizeFromId = -1;
    private static int _minPooledTweenerId = -1;
    private static int _maxPooledTweenerId = -1;
    public static bool isUnityEditor = Application.isEditor;
    private const int _DefaultMaxTweeners = 200;
    private const int _DefaultMaxSequences = 50;
    private const string _MaxTweensReached = "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup";
    public static bool isDebugBuild;
    public static bool hasActiveTweens;
    public static bool hasActiveDefaultTweens;
    public static bool hasActiveLateTweens;
    public static bool hasActiveFixedTweens;
    public static bool hasActiveManualTweens;
    public static int totActiveTweens;
    public static int totActiveDefaultTweens;
    public static int totActiveLateTweens;
    public static int totActiveFixedTweens;
    public static int totActiveManualTweens;
    public static int totActiveTweeners;
    public static int totActiveSequences;
    public static int totPooledTweeners;
    public static int totPooledSequences;
    public static int totTweeners;
    public static int totSequences;
    public static bool isUpdateLoop;
    private static bool _requiresActiveReorganization;
    private static bool _despawnAllCalledFromUpdateLoopCallback;

    public static TweenerCore<T1, T2, TPlugOptions> GetTweener<T1, T2, TPlugOptions>() where TPlugOptions : struct, IPlugOptions
    {
      if (TweenManager.totPooledTweeners > 0)
      {
        System.Type type1 = typeof (T1);
        System.Type type2 = typeof (T2);
        System.Type type3 = typeof (TPlugOptions);
        for (int maxPooledTweenerId = TweenManager._maxPooledTweenerId; maxPooledTweenerId > TweenManager._minPooledTweenerId - 1; --maxPooledTweenerId)
        {
          Tween pooledTweener = TweenManager._pooledTweeners[maxPooledTweenerId];
          if (pooledTweener != null && pooledTweener.typeofT1 == type1 && (pooledTweener.typeofT2 == type2 && pooledTweener.typeofTPlugOptions == type3))
          {
            TweenerCore<T1, T2, TPlugOptions> tweenerCore = (TweenerCore<T1, T2, TPlugOptions>) pooledTweener;
            TweenManager.AddActiveTween((Tween) tweenerCore);
            TweenManager._pooledTweeners[maxPooledTweenerId] = (Tween) null;
            if (TweenManager._maxPooledTweenerId != TweenManager._minPooledTweenerId)
            {
              if (maxPooledTweenerId == TweenManager._maxPooledTweenerId)
                --TweenManager._maxPooledTweenerId;
              else if (maxPooledTweenerId == TweenManager._minPooledTweenerId)
                ++TweenManager._minPooledTweenerId;
            }
            --TweenManager.totPooledTweeners;
            return tweenerCore;
          }
        }
        if (TweenManager.totTweeners >= TweenManager.maxTweeners)
        {
          TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId] = (Tween) null;
          --TweenManager._maxPooledTweenerId;
          --TweenManager.totPooledTweeners;
          --TweenManager.totTweeners;
        }
      }
      else if (TweenManager.totTweeners >= TweenManager.maxTweeners - 1)
      {
        int maxTweeners = TweenManager.maxTweeners;
        int maxSequences = TweenManager.maxSequences;
        TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.TweenersOnly);
        if (Debugger.logPriority >= 1)
          Debugger.LogWarning((object) "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", maxTweeners.ToString() + "/" + (object) maxSequences).Replace("#1", TweenManager.maxTweeners.ToString() + "/" + (object) TweenManager.maxSequences));
      }
      TweenerCore<T1, T2, TPlugOptions> tweenerCore1 = new TweenerCore<T1, T2, TPlugOptions>();
      ++TweenManager.totTweeners;
      TweenManager.AddActiveTween((Tween) tweenerCore1);
      return tweenerCore1;
    }

    public static Sequence GetSequence()
    {
      if (TweenManager.totPooledSequences > 0)
      {
        Sequence sequence = (Sequence) TweenManager._PooledSequences.Pop();
        TweenManager.AddActiveTween((Tween) sequence);
        --TweenManager.totPooledSequences;
        return sequence;
      }
      if (TweenManager.totSequences >= TweenManager.maxSequences - 1)
      {
        int maxTweeners = TweenManager.maxTweeners;
        int maxSequences = TweenManager.maxSequences;
        TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.SequencesOnly);
        if (Debugger.logPriority >= 1)
          Debugger.LogWarning((object) "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", maxTweeners.ToString() + "/" + (object) maxSequences).Replace("#1", TweenManager.maxTweeners.ToString() + "/" + (object) TweenManager.maxSequences));
      }
      Sequence sequence1 = new Sequence();
      ++TweenManager.totSequences;
      TweenManager.AddActiveTween((Tween) sequence1);
      return sequence1;
    }

    public static void SetUpdateType(Tween t, UpdateType updateType, bool isIndependentUpdate)
    {
      if (!t.active || t.updateType == updateType)
      {
        t.updateType = updateType;
        t.isIndependentUpdate = isIndependentUpdate;
      }
      else
      {
        if (t.updateType == UpdateType.Normal)
        {
          --TweenManager.totActiveDefaultTweens;
          TweenManager.hasActiveDefaultTweens = TweenManager.totActiveDefaultTweens > 0;
        }
        else
        {
          switch (t.updateType)
          {
            case UpdateType.Late:
              --TweenManager.totActiveLateTweens;
              TweenManager.hasActiveLateTweens = TweenManager.totActiveLateTweens > 0;
              break;
            case UpdateType.Fixed:
              --TweenManager.totActiveFixedTweens;
              TweenManager.hasActiveFixedTweens = TweenManager.totActiveFixedTweens > 0;
              break;
            default:
              --TweenManager.totActiveManualTweens;
              TweenManager.hasActiveManualTweens = TweenManager.totActiveManualTweens > 0;
              break;
          }
        }
        t.updateType = updateType;
        t.isIndependentUpdate = isIndependentUpdate;
        switch (updateType)
        {
          case UpdateType.Normal:
            ++TweenManager.totActiveDefaultTweens;
            TweenManager.hasActiveDefaultTweens = true;
            break;
          case UpdateType.Late:
            ++TweenManager.totActiveLateTweens;
            TweenManager.hasActiveLateTweens = true;
            break;
          case UpdateType.Fixed:
            ++TweenManager.totActiveFixedTweens;
            TweenManager.hasActiveFixedTweens = true;
            break;
          default:
            ++TweenManager.totActiveManualTweens;
            TweenManager.hasActiveManualTweens = true;
            break;
        }
      }
    }

    public static void AddActiveTweenToSequence(Tween t)
    {
      TweenManager.RemoveActiveTween(t);
    }

    public static int DespawnAll()
    {
      int totActiveTweens = TweenManager.totActiveTweens;
      for (int index = 0; index < TweenManager._maxActiveLookupId + 1; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween != null)
          TweenManager.Despawn(activeTween, false);
      }
      TweenManager.ClearTweenArray(TweenManager._activeTweens);
      int num1;
      TweenManager.hasActiveManualTweens = (num1 = 0) != 0;
      TweenManager.hasActiveFixedTweens = num1 != 0;
      TweenManager.hasActiveLateTweens = num1 != 0;
      TweenManager.hasActiveDefaultTweens = num1 != 0;
      TweenManager.hasActiveTweens = num1 != 0;
      int num2;
      TweenManager.totActiveManualTweens = num2 = 0;
      TweenManager.totActiveFixedTweens = num2;
      TweenManager.totActiveLateTweens = num2;
      TweenManager.totActiveDefaultTweens = num2;
      TweenManager.totActiveTweens = num2;
      TweenManager.totActiveTweeners = TweenManager.totActiveSequences = 0;
      TweenManager._maxActiveLookupId = TweenManager._reorganizeFromId = -1;
      TweenManager._requiresActiveReorganization = false;
      if (TweenManager.isUpdateLoop)
        TweenManager._despawnAllCalledFromUpdateLoopCallback = true;
      return totActiveTweens;
    }

    public static void Despawn(Tween t, bool modifyActiveLists = true)
    {
      if (t.onKill != null)
        Tween.OnTweenCallback(t.onKill);
      if (modifyActiveLists)
        TweenManager.RemoveActiveTween(t);
      if (t.isRecyclable)
      {
        switch (t.tweenType)
        {
          case TweenType.Tweener:
            if (TweenManager._maxPooledTweenerId == -1)
            {
              TweenManager._maxPooledTweenerId = TweenManager.maxTweeners - 1;
              TweenManager._minPooledTweenerId = TweenManager.maxTweeners - 1;
            }
            if (TweenManager._maxPooledTweenerId < TweenManager.maxTweeners - 1)
            {
              TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId + 1] = t;
              ++TweenManager._maxPooledTweenerId;
              if (TweenManager._minPooledTweenerId > TweenManager._maxPooledTweenerId)
                TweenManager._minPooledTweenerId = TweenManager._maxPooledTweenerId;
            }
            else
            {
              for (int maxPooledTweenerId = TweenManager._maxPooledTweenerId; maxPooledTweenerId > -1; --maxPooledTweenerId)
              {
                if (TweenManager._pooledTweeners[maxPooledTweenerId] == null)
                {
                  TweenManager._pooledTweeners[maxPooledTweenerId] = t;
                  if (maxPooledTweenerId < TweenManager._minPooledTweenerId)
                    TweenManager._minPooledTweenerId = maxPooledTweenerId;
                  if (TweenManager._maxPooledTweenerId < TweenManager._minPooledTweenerId)
                  {
                    TweenManager._maxPooledTweenerId = TweenManager._minPooledTweenerId;
                    break;
                  }
                  break;
                }
              }
            }
            ++TweenManager.totPooledTweeners;
            break;
          case TweenType.Sequence:
            TweenManager._PooledSequences.Push(t);
            ++TweenManager.totPooledSequences;
            Sequence sequence1 = (Sequence) t;
            int count1 = sequence1.sequencedTweens.Count;
            for (int index = 0; index < count1; ++index)
              TweenManager.Despawn(sequence1.sequencedTweens[index], false);
            break;
        }
      }
      else
      {
        switch (t.tweenType)
        {
          case TweenType.Tweener:
            --TweenManager.totTweeners;
            break;
          case TweenType.Sequence:
            --TweenManager.totSequences;
            Sequence sequence2 = (Sequence) t;
            int count2 = sequence2.sequencedTweens.Count;
            for (int index = 0; index < count2; ++index)
              TweenManager.Despawn(sequence2.sequencedTweens[index], false);
            break;
        }
      }
      t.active = false;
      t.Reset();
    }

    public static void PurgeAll()
    {
      for (int index = 0; index < TweenManager.totActiveTweens; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween != null)
        {
          activeTween.active = false;
          if (activeTween.onKill != null)
            Tween.OnTweenCallback(activeTween.onKill);
        }
      }
      TweenManager.ClearTweenArray(TweenManager._activeTweens);
      int num1;
      TweenManager.hasActiveManualTweens = (num1 = 0) != 0;
      TweenManager.hasActiveFixedTweens = num1 != 0;
      TweenManager.hasActiveLateTweens = num1 != 0;
      TweenManager.hasActiveDefaultTweens = num1 != 0;
      TweenManager.hasActiveTweens = num1 != 0;
      int num2;
      TweenManager.totActiveManualTweens = num2 = 0;
      TweenManager.totActiveFixedTweens = num2;
      TweenManager.totActiveLateTweens = num2;
      TweenManager.totActiveDefaultTweens = num2;
      TweenManager.totActiveTweens = num2;
      TweenManager.totActiveTweeners = TweenManager.totActiveSequences = 0;
      TweenManager._maxActiveLookupId = TweenManager._reorganizeFromId = -1;
      TweenManager._requiresActiveReorganization = false;
      TweenManager.PurgePools();
      TweenManager.ResetCapacities();
      TweenManager.totTweeners = TweenManager.totSequences = 0;
    }

    public static void PurgePools()
    {
      TweenManager.totTweeners -= TweenManager.totPooledTweeners;
      TweenManager.totSequences -= TweenManager.totPooledSequences;
      TweenManager.ClearTweenArray(TweenManager._pooledTweeners);
      TweenManager._PooledSequences.Clear();
      TweenManager.totPooledTweeners = TweenManager.totPooledSequences = 0;
      TweenManager._minPooledTweenerId = TweenManager._maxPooledTweenerId = -1;
    }

    public static void ResetCapacities()
    {
      TweenManager.SetCapacities(200, 50);
    }

    public static void SetCapacities(int tweenersCapacity, int sequencesCapacity)
    {
      if (tweenersCapacity < sequencesCapacity)
        tweenersCapacity = sequencesCapacity;
      TweenManager.maxActive = tweenersCapacity + sequencesCapacity;
      TweenManager.maxTweeners = tweenersCapacity;
      TweenManager.maxSequences = sequencesCapacity;
      Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
      Array.Resize<Tween>(ref TweenManager._pooledTweeners, tweenersCapacity);
      TweenManager._KillList.Capacity = TweenManager.maxActive;
    }

    public static int Validate()
    {
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      int num = 0;
      for (int index = 0; index < TweenManager._maxActiveLookupId + 1; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (!activeTween.Validate())
        {
          ++num;
          TweenManager.MarkForKilling(activeTween);
        }
      }
      if (num > 0)
      {
        TweenManager.DespawnActiveTweens(TweenManager._KillList);
        TweenManager._KillList.Clear();
      }
      return num;
    }

    public static void Update(UpdateType updateType, float deltaTime, float independentTime)
    {
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      TweenManager.isUpdateLoop = true;
      bool flag1 = false;
      int num1 = TweenManager._maxActiveLookupId + 1;
      for (int index = 0; index < num1; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween != null && activeTween.updateType == updateType)
        {
          if (!activeTween.active)
          {
            flag1 = true;
            TweenManager.MarkForKilling(activeTween);
          }
          else if (activeTween.isPlaying)
          {
            activeTween.creationLocked = true;
            float num2 = (activeTween.isIndependentUpdate ? independentTime : deltaTime) * activeTween.timeScale;
            if ((double) num2 > 0.0)
            {
              if (!activeTween.delayComplete)
              {
                num2 = activeTween.UpdateDelay(activeTween.elapsedDelay + num2);
                if ((double) num2 <= -1.0)
                {
                  flag1 = true;
                  TweenManager.MarkForKilling(activeTween);
                  continue;
                }
                if ((double) num2 > 0.0)
                {
                  if (activeTween.playedOnce && activeTween.onPlay != null)
                    Tween.OnTweenCallback(activeTween.onPlay);
                }
                else
                  continue;
              }
              if (!activeTween.startupDone && !activeTween.Startup())
              {
                flag1 = true;
                TweenManager.MarkForKilling(activeTween);
              }
              else
              {
                float position = activeTween.position;
                bool flag2 = (double) position >= (double) activeTween.duration;
                int toCompletedLoops = activeTween.completedLoops;
                float toPosition;
                if ((double) activeTween.duration <= 0.0)
                {
                  toPosition = 0.0f;
                  toCompletedLoops = activeTween.loops == -1 ? activeTween.completedLoops + 1 : activeTween.loops;
                }
                else
                {
                  if (activeTween.isBackwards)
                  {
                    for (toPosition = position - num2; (double) toPosition < 0.0 && toCompletedLoops > -1; --toCompletedLoops)
                      toPosition += activeTween.duration;
                    if (toCompletedLoops < 0 || flag2 && toCompletedLoops < 1)
                    {
                      toPosition = 0.0f;
                      toCompletedLoops = flag2 ? 1 : 0;
                    }
                  }
                  else
                  {
                    for (toPosition = position + num2; (double) toPosition >= (double) activeTween.duration && (activeTween.loops == -1 || toCompletedLoops < activeTween.loops); ++toCompletedLoops)
                      toPosition -= activeTween.duration;
                  }
                  if (flag2)
                    --toCompletedLoops;
                  if (activeTween.loops != -1 && toCompletedLoops >= activeTween.loops)
                    toPosition = activeTween.duration;
                }
                if (Tween.DoGoto(activeTween, toPosition, toCompletedLoops, UpdateMode.Update))
                {
                  flag1 = true;
                  TweenManager.MarkForKilling(activeTween);
                }
              }
            }
          }
        }
      }
      if (flag1)
      {
        if (TweenManager._despawnAllCalledFromUpdateLoopCallback)
          TweenManager._despawnAllCalledFromUpdateLoopCallback = false;
        else
          TweenManager.DespawnActiveTweens(TweenManager._KillList);
        TweenManager._KillList.Clear();
      }
      TweenManager.isUpdateLoop = false;
    }

    public static int FilteredOperation(OperationType operationType, FilterType filterType, object id, bool optionalBool, float optionalFloat, object optionalObj = null, object[] optionalArray = null)
    {
      int num1 = 0;
      bool flag1 = false;
      int num2 = optionalArray == null ? 0 : optionalArray.Length;
      bool flag2 = false;
      string str = (string) null;
      bool flag3 = false;
      int num3 = 0;
      if (filterType == FilterType.TargetOrId || filterType == FilterType.TargetAndId)
      {
        if (id is string)
        {
          flag2 = true;
          str = (string) id;
        }
        else if (id is int)
        {
          flag3 = true;
          num3 = (int) id;
        }
      }
      for (int maxActiveLookupId = TweenManager._maxActiveLookupId; maxActiveLookupId > -1; --maxActiveLookupId)
      {
        Tween activeTween = TweenManager._activeTweens[maxActiveLookupId];
        if (activeTween != null && activeTween.active)
        {
          bool flag4 = false;
          switch (filterType)
          {
            case FilterType.All:
              flag4 = true;
              break;
            case FilterType.TargetOrId:
              flag4 = !flag2 ? (!flag3 ? activeTween.id != null && id.Equals(activeTween.id) || activeTween.target != null && id.Equals(activeTween.target) : activeTween.intId == num3) : activeTween.stringId != null && activeTween.stringId == str;
              break;
            case FilterType.TargetAndId:
              flag4 = !flag2 ? (!flag3 ? activeTween.id != null && activeTween.target != null && (optionalObj != null && id.Equals(activeTween.id)) && optionalObj.Equals(activeTween.target) : activeTween.target != null && activeTween.intId == num3 && optionalObj != null && optionalObj.Equals(activeTween.target)) : activeTween.target != null && activeTween.stringId == str && optionalObj != null && optionalObj.Equals(activeTween.target);
              break;
            case FilterType.AllExceptTargetsOrIds:
              flag4 = true;
              for (int index = 0; index < num2; ++index)
              {
                object optional = optionalArray[index];
                if (optional is string)
                {
                  flag2 = true;
                  str = (string) optional;
                }
                else if (optional is int)
                {
                  flag3 = true;
                  num3 = (int) optional;
                }
                if (flag2 && activeTween.stringId == str)
                {
                  flag4 = false;
                  break;
                }
                if (flag3 && activeTween.intId == num3)
                {
                  flag4 = false;
                  break;
                }
                if (activeTween.id != null && optional.Equals(activeTween.id) || activeTween.target != null && optional.Equals(activeTween.target))
                {
                  flag4 = false;
                  break;
                }
              }
              break;
          }
          if (flag4)
          {
            switch (operationType)
            {
              case OperationType.Complete:
                bool autoKill = activeTween.autoKill;
                if (TweenManager.Complete(activeTween, false, (double) optionalFloat > 0.0 ? UpdateMode.Update : UpdateMode.Goto))
                {
                  num1 += !optionalBool ? 1 : (autoKill ? 1 : 0);
                  if (autoKill)
                  {
                    if (TweenManager.isUpdateLoop)
                    {
                      activeTween.active = false;
                      continue;
                    }
                    flag1 = true;
                    TweenManager._KillList.Add(activeTween);
                    continue;
                  }
                  continue;
                }
                continue;
              case OperationType.Despawn:
                ++num1;
                activeTween.active = false;
                if (!TweenManager.isUpdateLoop)
                {
                  TweenManager.Despawn(activeTween, false);
                  flag1 = true;
                  TweenManager._KillList.Add(activeTween);
                  continue;
                }
                continue;
              case OperationType.Flip:
                if (TweenManager.Flip(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.Goto:
                TweenManager.Goto(activeTween, optionalFloat, optionalBool, UpdateMode.Goto);
                ++num1;
                continue;
              case OperationType.Pause:
                if (TweenManager.Pause(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.Play:
                if (TweenManager.Play(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.PlayForward:
                if (TweenManager.PlayForward(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.PlayBackwards:
                if (TweenManager.PlayBackwards(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.Rewind:
                if (TweenManager.Rewind(activeTween, optionalBool))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.SmoothRewind:
                if (TweenManager.SmoothRewind(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.Restart:
                if (TweenManager.Restart(activeTween, optionalBool, optionalFloat))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.TogglePause:
                if (TweenManager.TogglePause(activeTween))
                {
                  ++num1;
                  continue;
                }
                continue;
              case OperationType.IsTweening:
                if ((!activeTween.isComplete || !activeTween.autoKill) && (!optionalBool || activeTween.isPlaying))
                {
                  ++num1;
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
      if (flag1)
      {
        for (int index = TweenManager._KillList.Count - 1; index > -1; --index)
        {
          Tween kill = TweenManager._KillList[index];
          if (kill.activeId != -1)
            TweenManager.RemoveActiveTween(kill);
        }
        TweenManager._KillList.Clear();
      }
      return num1;
    }

    public static bool Complete(Tween t, bool modifyActiveLists = true, UpdateMode updateMode = UpdateMode.Goto)
    {
      if (t.loops == -1 || t.isComplete)
        return false;
      Tween.DoGoto(t, t.duration, t.loops, updateMode);
      t.isPlaying = false;
      if (t.autoKill)
      {
        if (TweenManager.isUpdateLoop)
          t.active = false;
        else
          TweenManager.Despawn(t, modifyActiveLists);
      }
      return true;
    }

    public static bool Flip(Tween t)
    {
      t.isBackwards = !t.isBackwards;
      return true;
    }

    public static void ForceInit(Tween t, bool isSequenced = false)
    {
      if (t.startupDone || t.Startup() || isSequenced)
        return;
      if (TweenManager.isUpdateLoop)
        t.active = false;
      else
        TweenManager.RemoveActiveTween(t);
    }

    public static bool Goto(Tween t, float to, bool andPlay = false, UpdateMode updateMode = UpdateMode.Goto)
    {
      bool isPlaying = t.isPlaying;
      t.isPlaying = andPlay;
      t.delayComplete = true;
      t.elapsedDelay = t.delay;
      int toCompletedLoops = (double) t.duration <= 0.0 ? 1 : Mathf.FloorToInt(to / t.duration);
      float toPosition = to % t.duration;
      if (t.loops != -1 && toCompletedLoops >= t.loops)
      {
        toCompletedLoops = t.loops;
        toPosition = t.duration;
      }
      else if ((double) toPosition >= (double) t.duration)
        toPosition = 0.0f;
      bool flag = Tween.DoGoto(t, toPosition, toCompletedLoops, updateMode);
      if (!andPlay & isPlaying && !flag && t.onPause != null)
        Tween.OnTweenCallback(t.onPause);
      return flag;
    }

    public static bool Pause(Tween t)
    {
      if (!t.isPlaying)
        return false;
      t.isPlaying = false;
      if (t.onPause != null)
        Tween.OnTweenCallback(t.onPause);
      return true;
    }

    public static bool Play(Tween t)
    {
      if (t.isPlaying || (t.isBackwards || t.isComplete) && (!t.isBackwards || t.completedLoops <= 0 && (double) t.position <= 0.0))
        return false;
      t.isPlaying = true;
      if (t.playedOnce && t.delayComplete && t.onPlay != null)
        Tween.OnTweenCallback(t.onPlay);
      return true;
    }

    public static bool PlayBackwards(Tween t)
    {
      if (t.completedLoops == 0 && (double) t.position <= 0.0)
        TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
      if (t.isBackwards)
        return TweenManager.Play(t);
      t.isBackwards = true;
      TweenManager.Play(t);
      return true;
    }

    public static bool PlayForward(Tween t)
    {
      if (!t.isBackwards)
        return TweenManager.Play(t);
      t.isBackwards = false;
      TweenManager.Play(t);
      return true;
    }

    public static bool Restart(Tween t, bool includeDelay = true, float changeDelayTo = -1f)
    {
      int num = !t.isPlaying ? 1 : 0;
      t.isBackwards = false;
      if ((double) changeDelayTo >= 0.0)
        t.delay = changeDelayTo;
      TweenManager.Rewind(t, includeDelay);
      t.isPlaying = true;
      if (num != 0 && t.playedOnce && (t.delayComplete && t.onPlay != null))
        Tween.OnTweenCallback(t.onPlay);
      return true;
    }

    public static bool Rewind(Tween t, bool includeDelay = true)
    {
      bool isPlaying = t.isPlaying;
      t.isPlaying = false;
      bool flag = false;
      if ((double) t.delay > 0.0)
      {
        if (includeDelay)
        {
          flag = (double) t.delay > 0.0 && (double) t.elapsedDelay > 0.0;
          t.elapsedDelay = 0.0f;
          t.delayComplete = false;
        }
        else
        {
          flag = (double) t.elapsedDelay < (double) t.delay;
          t.elapsedDelay = t.delay;
          t.delayComplete = true;
        }
      }
      if ((double) t.position > 0.0 || t.completedLoops > 0 || !t.startupDone)
      {
        flag = true;
        if (!Tween.DoGoto(t, 0.0f, 0, UpdateMode.Goto) & isPlaying && t.onPause != null)
          Tween.OnTweenCallback(t.onPause);
      }
      else
        TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, false);
      return flag;
    }

    public static bool SmoothRewind(Tween t)
    {
      bool flag = false;
      if ((double) t.delay > 0.0)
      {
        flag = (double) t.elapsedDelay < (double) t.delay;
        t.elapsedDelay = t.delay;
        t.delayComplete = true;
      }
      if ((double) t.position > 0.0 || t.completedLoops > 0 || !t.startupDone)
      {
        flag = true;
        if (t.loopType == LoopType.Incremental)
        {
          t.PlayBackwards();
        }
        else
        {
          t.Goto(t.ElapsedDirectionalPercentage() * t.duration, false);
          t.PlayBackwards();
        }
      }
      else
      {
        t.isPlaying = false;
        TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
      }
      return flag;
    }

    public static bool TogglePause(Tween t)
    {
      if (t.isPlaying)
        return TweenManager.Pause(t);
      return TweenManager.Play(t);
    }

    public static int TotalPooledTweens()
    {
      return TweenManager.totPooledTweeners + TweenManager.totPooledSequences;
    }

    public static int TotalPlayingTweens()
    {
      if (!TweenManager.hasActiveTweens)
        return 0;
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      int num = 0;
      for (int index = 0; index < TweenManager._maxActiveLookupId + 1; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween != null && activeTween.isPlaying)
          ++num;
      }
      return num;
    }

    public static List<Tween> GetActiveTweens(bool playing, List<Tween> fillableList = null)
    {
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      if (TweenManager.totActiveTweens <= 0)
        return (List<Tween>) null;
      int totActiveTweens = TweenManager.totActiveTweens;
      if (fillableList == null)
        fillableList = new List<Tween>(totActiveTweens);
      for (int index = 0; index < totActiveTweens; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween.isPlaying == playing)
          fillableList.Add(activeTween);
      }
      if (fillableList.Count > 0)
        return fillableList;
      return (List<Tween>) null;
    }

    public static List<Tween> GetTweensById(object id, bool playingOnly, List<Tween> fillableList = null)
    {
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      if (TweenManager.totActiveTweens <= 0)
        return (List<Tween>) null;
      int totActiveTweens = TweenManager.totActiveTweens;
      if (fillableList == null)
        fillableList = new List<Tween>(totActiveTweens);
      for (int index = 0; index < totActiveTweens; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween != null && object.Equals(id, activeTween.id) && (!playingOnly || activeTween.isPlaying))
          fillableList.Add(activeTween);
      }
      if (fillableList.Count > 0)
        return fillableList;
      return (List<Tween>) null;
    }

    public static List<Tween> GetTweensByTarget(object target, bool playingOnly, List<Tween> fillableList = null)
    {
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      if (TweenManager.totActiveTweens <= 0)
        return (List<Tween>) null;
      int totActiveTweens = TweenManager.totActiveTweens;
      if (fillableList == null)
        fillableList = new List<Tween>(totActiveTweens);
      for (int index = 0; index < totActiveTweens; ++index)
      {
        Tween activeTween = TweenManager._activeTweens[index];
        if (activeTween.target == target && (!playingOnly || activeTween.isPlaying))
          fillableList.Add(activeTween);
      }
      if (fillableList.Count > 0)
        return fillableList;
      return (List<Tween>) null;
    }

    private static void MarkForKilling(Tween t)
    {
      t.active = false;
      TweenManager._KillList.Add(t);
    }

    private static void AddActiveTween(Tween t)
    {
      if (TweenManager._requiresActiveReorganization)
        TweenManager.ReorganizeActiveTweens();
      t.active = true;
      t.updateType = DOTween.defaultUpdateType;
      t.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
      t.activeId = TweenManager._maxActiveLookupId = TweenManager.totActiveTweens;
      TweenManager._activeTweens[TweenManager.totActiveTweens] = t;
      if (t.updateType == UpdateType.Normal)
      {
        ++TweenManager.totActiveDefaultTweens;
        TweenManager.hasActiveDefaultTweens = true;
      }
      else
      {
        switch (t.updateType)
        {
          case UpdateType.Late:
            ++TweenManager.totActiveLateTweens;
            TweenManager.hasActiveLateTweens = true;
            break;
          case UpdateType.Fixed:
            ++TweenManager.totActiveFixedTweens;
            TweenManager.hasActiveFixedTweens = true;
            break;
          default:
            ++TweenManager.totActiveManualTweens;
            TweenManager.hasActiveManualTweens = true;
            break;
        }
      }
      ++TweenManager.totActiveTweens;
      if (t.tweenType == TweenType.Tweener)
        ++TweenManager.totActiveTweeners;
      else
        ++TweenManager.totActiveSequences;
      TweenManager.hasActiveTweens = true;
    }

    private static void ReorganizeActiveTweens()
    {
      if (TweenManager.totActiveTweens <= 0)
      {
        TweenManager._maxActiveLookupId = -1;
        TweenManager._requiresActiveReorganization = false;
        TweenManager._reorganizeFromId = -1;
      }
      else if (TweenManager._reorganizeFromId == TweenManager._maxActiveLookupId)
      {
        --TweenManager._maxActiveLookupId;
        TweenManager._requiresActiveReorganization = false;
        TweenManager._reorganizeFromId = -1;
      }
      else
      {
        int num1 = 1;
        int num2 = TweenManager._maxActiveLookupId + 1;
        TweenManager._maxActiveLookupId = TweenManager._reorganizeFromId - 1;
        for (int index = TweenManager._reorganizeFromId + 1; index < num2; ++index)
        {
          Tween activeTween = TweenManager._activeTweens[index];
          if (activeTween == null)
          {
            ++num1;
          }
          else
          {
            activeTween.activeId = TweenManager._maxActiveLookupId = index - num1;
            TweenManager._activeTweens[index - num1] = activeTween;
            TweenManager._activeTweens[index] = (Tween) null;
          }
        }
        TweenManager._requiresActiveReorganization = false;
        TweenManager._reorganizeFromId = -1;
      }
    }

    private static void DespawnActiveTweens(List<Tween> tweens)
    {
      for (int index = tweens.Count - 1; index > -1; --index)
        TweenManager.Despawn(tweens[index], true);
    }

    private static void RemoveActiveTween(Tween t)
    {
      int activeId = t.activeId;
      t.activeId = -1;
      TweenManager._requiresActiveReorganization = true;
      if (TweenManager._reorganizeFromId == -1 || TweenManager._reorganizeFromId > activeId)
        TweenManager._reorganizeFromId = activeId;
      TweenManager._activeTweens[activeId] = (Tween) null;
      if (t.updateType == UpdateType.Normal)
      {
        if (TweenManager.totActiveDefaultTweens > 0)
        {
          --TweenManager.totActiveDefaultTweens;
          TweenManager.hasActiveDefaultTweens = TweenManager.totActiveDefaultTweens > 0;
        }
        else
          Debugger.LogRemoveActiveTweenError("totActiveDefaultTweens");
      }
      else
      {
        switch (t.updateType)
        {
          case UpdateType.Late:
            if (TweenManager.totActiveLateTweens > 0)
            {
              --TweenManager.totActiveLateTweens;
              TweenManager.hasActiveLateTweens = TweenManager.totActiveLateTweens > 0;
              break;
            }
            Debugger.LogRemoveActiveTweenError("totActiveLateTweens");
            break;
          case UpdateType.Fixed:
            if (TweenManager.totActiveFixedTweens > 0)
            {
              --TweenManager.totActiveFixedTweens;
              TweenManager.hasActiveFixedTweens = TweenManager.totActiveFixedTweens > 0;
              break;
            }
            Debugger.LogRemoveActiveTweenError("totActiveFixedTweens");
            break;
          default:
            if (TweenManager.totActiveManualTweens > 0)
            {
              --TweenManager.totActiveManualTweens;
              TweenManager.hasActiveManualTweens = TweenManager.totActiveManualTweens > 0;
              break;
            }
            Debugger.LogRemoveActiveTweenError("totActiveManualTweens");
            break;
        }
      }
      --TweenManager.totActiveTweens;
      TweenManager.hasActiveTweens = TweenManager.totActiveTweens > 0;
      if (t.tweenType == TweenType.Tweener)
        --TweenManager.totActiveTweeners;
      else
        --TweenManager.totActiveSequences;
      if (TweenManager.totActiveTweens < 0)
      {
        TweenManager.totActiveTweens = 0;
        Debugger.LogRemoveActiveTweenError("totActiveTweens");
      }
      if (TweenManager.totActiveTweeners < 0)
      {
        TweenManager.totActiveTweeners = 0;
        Debugger.LogRemoveActiveTweenError("totActiveTweeners");
      }
      if (TweenManager.totActiveSequences >= 0)
        return;
      TweenManager.totActiveSequences = 0;
      Debugger.LogRemoveActiveTweenError("totActiveSequences");
    }

    private static void ClearTweenArray(Tween[] tweens)
    {
      int length = tweens.Length;
      for (int index = 0; index < length; ++index)
        tweens[index] = (Tween) null;
    }

    private static void IncreaseCapacities(TweenManager.CapacityIncreaseMode increaseMode)
    {
      int num1 = 0;
      int num2 = Mathf.Max((int) ((double) TweenManager.maxTweeners * 1.5), 200);
      int num3 = Mathf.Max((int) ((double) TweenManager.maxSequences * 1.5), 50);
      int num4;
      switch (increaseMode)
      {
        case TweenManager.CapacityIncreaseMode.TweenersOnly:
          num4 = num1 + num2;
          TweenManager.maxTweeners += num2;
          Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
          break;
        case TweenManager.CapacityIncreaseMode.SequencesOnly:
          num4 = num1 + num3;
          TweenManager.maxSequences += num3;
          break;
        default:
          num4 = num1 + num2;
          TweenManager.maxTweeners += num2;
          TweenManager.maxSequences += num3;
          Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
          break;
      }
      TweenManager.maxActive = TweenManager.maxTweeners + TweenManager.maxSequences;
      Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
      if (num4 <= 0)
        return;
      TweenManager._KillList.Capacity += num4;
    }

    private static void ManageOnRewindCallbackWhenAlreadyRewinded(Tween t, bool isPlayBackwardsOrSmoothRewind)
    {
      if (t.onRewind == null)
        return;
      if (isPlayBackwardsOrSmoothRewind)
      {
        if (DOTween.rewindCallbackMode != RewindCallbackMode.FireAlways)
          return;
        t.onRewind();
      }
      else
      {
        if (DOTween.rewindCallbackMode == RewindCallbackMode.FireIfPositionChanged)
          return;
        t.onRewind();
      }
    }

    public enum CapacityIncreaseMode
    {
      TweenersAndSequences,
      TweenersOnly,
      SequencesOnly,
    }
  }
}
