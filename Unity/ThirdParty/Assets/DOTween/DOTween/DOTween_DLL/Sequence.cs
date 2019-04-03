// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Sequence
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using System;
using System.Collections.Generic;

namespace DG.Tweening
{
  /// <summary>Controls other tweens as a group</summary>
  public sealed class Sequence : Tween
  {
    public readonly List<Tween> sequencedTweens = new List<Tween>();
    private readonly List<ABSSequentiable> _sequencedObjs = new List<ABSSequentiable>();
    public float lastTweenInsertTime;

    public Sequence()
    {
      this.tweenType = TweenType.Sequence;
      this.Reset();
    }

    public static Sequence DoPrepend(Sequence inSequence, Tween t)
    {
      if (t.loops == -1)
        t.loops = 1;
      float num = t.delay + t.duration * (float) t.loops;
      inSequence.duration += num;
      int count = inSequence._sequencedObjs.Count;
      for (int index = 0; index < count; ++index)
      {
        ABSSequentiable sequencedObj = inSequence._sequencedObjs[index];
        sequencedObj.sequencedPosition += num;
        sequencedObj.sequencedEndPosition += num;
      }
      return Sequence.DoInsert(inSequence, t, 0.0f);
    }

    public static Sequence DoInsert(Sequence inSequence, Tween t, float atPosition)
    {
      TweenManager.AddActiveTweenToSequence(t);
      atPosition += t.delay;
      inSequence.lastTweenInsertTime = atPosition;
      t.isSequenced = t.creationLocked = true;
      t.sequenceParent = inSequence;
      if (t.loops == -1)
        t.loops = 1;
      float num = t.duration * (float) t.loops;
      t.autoKill = false;
      t.delay = t.elapsedDelay = 0.0f;
      t.delayComplete = true;
      t.isSpeedBased = false;
      t.sequencedPosition = atPosition;
      t.sequencedEndPosition = atPosition + num;
      if ((double) t.sequencedEndPosition > (double) inSequence.duration)
        inSequence.duration = t.sequencedEndPosition;
      inSequence._sequencedObjs.Add((ABSSequentiable) t);
      inSequence.sequencedTweens.Add(t);
      return inSequence;
    }

    public static Sequence DoAppendInterval(Sequence inSequence, float interval)
    {
      inSequence.lastTweenInsertTime = inSequence.duration;
      inSequence.duration += interval;
      return inSequence;
    }

    public static Sequence DoPrependInterval(Sequence inSequence, float interval)
    {
      inSequence.lastTweenInsertTime = 0.0f;
      inSequence.duration += interval;
      int count = inSequence._sequencedObjs.Count;
      for (int index = 0; index < count; ++index)
      {
        ABSSequentiable sequencedObj = inSequence._sequencedObjs[index];
        sequencedObj.sequencedPosition += interval;
        sequencedObj.sequencedEndPosition += interval;
      }
      return inSequence;
    }

    public static Sequence DoInsertCallback(Sequence inSequence, TweenCallback callback, float atPosition)
    {
      inSequence.lastTweenInsertTime = atPosition;
      SequenceCallback sequenceCallback = new SequenceCallback(atPosition, callback);
      sequenceCallback.sequencedPosition = sequenceCallback.sequencedEndPosition = atPosition;
      inSequence._sequencedObjs.Add((ABSSequentiable) sequenceCallback);
      if ((double) inSequence.duration < (double) atPosition)
        inSequence.duration = atPosition;
      return inSequence;
    }

    public override void Reset()
    {
      base.Reset();
      this.sequencedTweens.Clear();
      this._sequencedObjs.Clear();
      this.lastTweenInsertTime = 0.0f;
    }

    public override bool Validate()
    {
      int count = this.sequencedTweens.Count;
      for (int index = 0; index < count; ++index)
      {
        if (!this.sequencedTweens[index].Validate())
          return false;
      }
      return true;
    }

    public override bool Startup()
    {
      return Sequence.DoStartup(this);
    }

    public override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
    {
      return Sequence.DoApplyTween(this, prevPosition, prevCompletedLoops, newCompletedSteps, useInversePosition, updateMode);
    }

    public static void Setup(Sequence s)
    {
      s.autoKill = DOTween.defaultAutoKill;
      s.isRecyclable = DOTween.defaultRecyclable;
      s.isPlaying = DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlaySequences;
      s.loopType = DOTween.defaultLoopType;
      s.easeType = Ease.Linear;
      s.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
      s.easePeriod = DOTween.defaultEasePeriod;
    }

    public static bool DoStartup(Sequence s)
    {
      if (s.sequencedTweens.Count == 0 && s._sequencedObjs.Count == 0 && (s.onComplete == null && s.onKill == null) && (s.onPause == null && s.onPlay == null && (s.onRewind == null && s.onStart == null)) && (s.onStepComplete == null && s.onUpdate == null))
        return false;
      s.startupDone = true;
      s.fullDuration = s.loops > -1 ? s.duration * (float) s.loops : float.PositiveInfinity;
      Sequence.StableSortSequencedObjs(s._sequencedObjs);
      if (s.isRelative)
      {
        int count = s.sequencedTweens.Count;
        for (int index = 0; index < count; ++index)
        {
          Tween sequencedTween = s.sequencedTweens[index];
          if (!s.isBlendable)
            s.sequencedTweens[index].isRelative = true;
        }
      }
      return true;
    }

    public static bool DoApplyTween(Sequence s, float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode)
    {
      float time1 = prevPosition;
      float time2 = s.position;
      if (s.easeType != Ease.Linear)
      {
        time1 = s.duration * EaseManager.Evaluate(s.easeType, s.customEase, time1, s.duration, s.easeOvershootOrAmplitude, s.easePeriod);
        time2 = s.duration * EaseManager.Evaluate(s.easeType, s.customEase, time2, s.duration, s.easeOvershootOrAmplitude, s.easePeriod);
      }
      float toPos = 0.0f;
      bool prevPosIsInverse = s.loopType == LoopType.Yoyo && ((double) time1 < (double) s.duration ? (uint) (prevCompletedLoops % 2) > 0U : prevCompletedLoops % 2 == 0);
      if (s.isBackwards)
        prevPosIsInverse = !prevPosIsInverse;
      if (newCompletedSteps > 0)
      {
        int completedLoops = s.completedLoops;
        float position = s.position;
        int num1 = newCompletedSteps;
        int num2 = 0;
        float fromPos = time1;
        if (updateMode == UpdateMode.Update)
        {
          while (num2 < num1)
          {
            if (num2 > 0)
              fromPos = toPos;
            else if (prevPosIsInverse && !s.isBackwards)
              fromPos = s.duration - fromPos;
            toPos = prevPosIsInverse ? 0.0f : s.duration;
            if (Sequence.ApplyInternalCycle(s, fromPos, toPos, updateMode, useInversePosition, prevPosIsInverse, true))
              return true;
            ++num2;
            if (s.loopType == LoopType.Yoyo)
              prevPosIsInverse = !prevPosIsInverse;
          }
          if (completedLoops != s.completedLoops || (double) Math.Abs(position - s.position) > 1.40129846432482E-45)
            return !s.active;
        }
        else
        {
          if (s.loopType == LoopType.Yoyo && newCompletedSteps % 2 != 0)
          {
            prevPosIsInverse = !prevPosIsInverse;
            time1 = s.duration - time1;
          }
          newCompletedSteps = 0;
        }
      }
      if (newCompletedSteps == 1 && s.isComplete)
        return false;
      float fromPos1;
      if (newCompletedSteps > 0 && !s.isComplete)
      {
        fromPos1 = useInversePosition ? s.duration : 0.0f;
        if (s.loopType == LoopType.Restart && (double) toPos > 0.0)
          Sequence.ApplyInternalCycle(s, s.duration, 0.0f, UpdateMode.Goto, false, false, false);
      }
      else
        fromPos1 = useInversePosition ? s.duration - time1 : time1;
      return Sequence.ApplyInternalCycle(s, fromPos1, useInversePosition ? s.duration - time2 : time2, updateMode, useInversePosition, prevPosIsInverse, false);
    }

    private static bool ApplyInternalCycle(Sequence s, float fromPos, float toPos, UpdateMode updateMode, bool useInverse, bool prevPosIsInverse, bool multiCycleStep = false)
    {
      if ((double) toPos < (double) fromPos)
      {
        int num = s._sequencedObjs.Count - 1;
        for (int index = num; index > -1; --index)
        {
          if (!s.active)
            return true;
          ABSSequentiable sequencedObj = s._sequencedObjs[index];
          if ((double) sequencedObj.sequencedEndPosition >= (double) toPos && (double) sequencedObj.sequencedPosition <= (double) fromPos)
          {
            if (sequencedObj.tweenType == TweenType.Callback)
            {
              if (updateMode == UpdateMode.Update & prevPosIsInverse)
                Tween.OnTweenCallback(sequencedObj.onStart);
            }
            else
            {
              float to = toPos - sequencedObj.sequencedPosition;
              if ((double) to < 0.0)
                to = 0.0f;
              Tween t = (Tween) sequencedObj;
              if (t.startupDone)
              {
                t.isBackwards = true;
                if (TweenManager.Goto(t, to, false, updateMode))
                {
                  TweenManager.Despawn(t, false);
                  s._sequencedObjs.RemoveAt(index);
                  --index;
                  --num;
                }
                else if (multiCycleStep && t.tweenType == TweenType.Sequence)
                {
                  if ((double) s.position <= 0.0 && s.completedLoops == 0)
                  {
                    t.position = 0.0f;
                  }
                  else
                  {
                    bool flag = s.completedLoops == 0 || s.isBackwards && (s.completedLoops < s.loops || s.loops == -1);
                    if (t.isBackwards)
                      flag = !flag;
                    if (useInverse)
                      flag = !flag;
                    if (s.isBackwards && !useInverse && !prevPosIsInverse)
                      flag = !flag;
                    t.position = flag ? 0.0f : t.duration;
                  }
                }
              }
            }
          }
        }
      }
      else
      {
        int count = s._sequencedObjs.Count;
        for (int index = 0; index < count; ++index)
        {
          if (!s.active)
            return true;
          ABSSequentiable sequencedObj = s._sequencedObjs[index];
          if ((double) sequencedObj.sequencedPosition <= (double) toPos && ((double) sequencedObj.sequencedPosition <= 0.0 || (double) sequencedObj.sequencedEndPosition > (double) fromPos) && ((double) sequencedObj.sequencedPosition > 0.0 || (double) sequencedObj.sequencedEndPosition >= (double) fromPos))
          {
            if (sequencedObj.tweenType == TweenType.Callback)
            {
              if (updateMode == UpdateMode.Update && (s.isBackwards || useInverse || prevPosIsInverse ? (!(s.isBackwards & useInverse) ? 0 : (!prevPosIsInverse ? 1 : 0)) : 1) != 0)
                Tween.OnTweenCallback(sequencedObj.onStart);
            }
            else
            {
              float to = toPos - sequencedObj.sequencedPosition;
              if ((double) to < 0.0)
                to = 0.0f;
              Tween t = (Tween) sequencedObj;
              if ((double) toPos >= (double) sequencedObj.sequencedEndPosition)
              {
                if (!t.startupDone)
                  TweenManager.ForceInit(t, true);
                if ((double) to < (double) t.fullDuration)
                  to = t.fullDuration;
              }
              t.isBackwards = false;
              if (TweenManager.Goto(t, to, false, updateMode))
              {
                TweenManager.Despawn(t, false);
                s._sequencedObjs.RemoveAt(index);
                --index;
                --count;
              }
              else if (multiCycleStep && t.tweenType == TweenType.Sequence)
              {
                if ((double) s.position <= 0.0 && s.completedLoops == 0)
                {
                  t.position = 0.0f;
                }
                else
                {
                  bool flag = s.completedLoops == 0 || !s.isBackwards && (s.completedLoops < s.loops || s.loops == -1);
                  if (t.isBackwards)
                    flag = !flag;
                  if (useInverse)
                    flag = !flag;
                  if (s.isBackwards && !useInverse && !prevPosIsInverse)
                    flag = !flag;
                  t.position = flag ? 0.0f : t.duration;
                }
              }
            }
          }
        }
      }
      return false;
    }

    private static void StableSortSequencedObjs(List<ABSSequentiable> list)
    {
      int count = list.Count;
      for (int index1 = 1; index1 < count; ++index1)
      {
        int index2 = index1;
        ABSSequentiable absSequentiable;
        for (absSequentiable = list[index1]; index2 > 0 && (double) list[index2 - 1].sequencedPosition > (double) absSequentiable.sequencedPosition; --index2)
          list[index2] = list[index2 - 1];
        list[index2] = absSequentiable;
      }
    }
  }
}
