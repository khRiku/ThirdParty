using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;

namespace DG.Tweening
{
	// Token: 0x02000013 RID: 19
	public sealed class Sequence : Tween
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00003AEE File Offset: 0x00001CEE
		internal Sequence()
		{
			this.tweenType = TweenType.Sequence;
			this.Reset();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003B1C File Offset: 0x00001D1C
		internal static Sequence DoPrepend(Sequence inSequence, Tween t)
		{
			if (t.loops == -1)
			{
				t.loops = 1;
			}
			float num = t.delay + t.duration * (float)t.loops;
			inSequence.duration += num;
			int count = inSequence._sequencedObjs.Count;
			for (int i = 0; i < count; i++)
			{
				ABSSequentiable abssequentiable = inSequence._sequencedObjs[i];
				abssequentiable.sequencedPosition += num;
				abssequentiable.sequencedEndPosition += num;
			}
			return Sequence.DoInsert(inSequence, t, 0f);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003BA8 File Offset: 0x00001DA8
		internal static Sequence DoInsert(Sequence inSequence, Tween t, float atPosition)
		{
			TweenManager.AddActiveTweenToSequence(t);
			atPosition += t.delay;
			inSequence.lastTweenInsertTime = atPosition;
			t.isSequenced = (t.creationLocked = true);
			t.sequenceParent = inSequence;
			if (t.loops == -1)
			{
				t.loops = 1;
			}
			float num = t.duration * (float)t.loops;
			t.autoKill = false;
			t.delay = (t.elapsedDelay = 0f);
			t.delayComplete = true;
			t.isSpeedBased = false;
			t.sequencedPosition = atPosition;
			t.sequencedEndPosition = atPosition + num;
			if (t.sequencedEndPosition > inSequence.duration)
			{
				inSequence.duration = t.sequencedEndPosition;
			}
			inSequence._sequencedObjs.Add(t);
			inSequence.sequencedTweens.Add(t);
			return inSequence;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003C6E File Offset: 0x00001E6E
		internal static Sequence DoAppendInterval(Sequence inSequence, float interval)
		{
			inSequence.lastTweenInsertTime = inSequence.duration;
			inSequence.duration += interval;
			return inSequence;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003C8C File Offset: 0x00001E8C
		internal static Sequence DoPrependInterval(Sequence inSequence, float interval)
		{
			inSequence.lastTweenInsertTime = 0f;
			inSequence.duration += interval;
			int count = inSequence._sequencedObjs.Count;
			for (int i = 0; i < count; i++)
			{
				ABSSequentiable abssequentiable = inSequence._sequencedObjs[i];
				abssequentiable.sequencedPosition += interval;
				abssequentiable.sequencedEndPosition += interval;
			}
			return inSequence;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003CF4 File Offset: 0x00001EF4
		internal static Sequence DoInsertCallback(Sequence inSequence, TweenCallback callback, float atPosition)
		{
			inSequence.lastTweenInsertTime = atPosition;
			SequenceCallback sequenceCallback = new SequenceCallback(atPosition, callback);
			ABSSequentiable abssequentiable = sequenceCallback;
			sequenceCallback.sequencedEndPosition = atPosition;
			abssequentiable.sequencedPosition = atPosition;
			inSequence._sequencedObjs.Add(sequenceCallback);
			if (inSequence.duration < atPosition)
			{
				inSequence.duration = atPosition;
			}
			return inSequence;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003D3D File Offset: 0x00001F3D
		internal override void Reset()
		{
			base.Reset();
			this.sequencedTweens.Clear();
			this._sequencedObjs.Clear();
			this.lastTweenInsertTime = 0f;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003D68 File Offset: 0x00001F68
		internal override bool Validate()
		{
			int count = this.sequencedTweens.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.sequencedTweens[i].Validate())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003DA3 File Offset: 0x00001FA3
		internal override bool Startup()
		{
			return Sequence.DoStartup(this);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003DAB File Offset: 0x00001FAB
		internal override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
		{
			return Sequence.DoApplyTween(this, prevPosition, prevCompletedLoops, newCompletedSteps, useInversePosition, updateMode);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003DBC File Offset: 0x00001FBC
		internal static void Setup(Sequence s)
		{
			s.autoKill = DOTween.defaultAutoKill;
			s.isRecyclable = DOTween.defaultRecyclable;
			s.isPlaying = (DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlaySequences);
			s.loopType = DOTween.defaultLoopType;
			s.easeType = Ease.Linear;
			s.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			s.easePeriod = DOTween.defaultEasePeriod;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003E20 File Offset: 0x00002020
		internal static bool DoStartup(Sequence s)
		{
			if (s.sequencedTweens.Count == 0 && s._sequencedObjs.Count == 0 && s.onComplete == null && s.onKill == null && s.onPause == null && s.onPlay == null && s.onRewind == null && s.onStart == null && s.onStepComplete == null && s.onUpdate == null)
			{
				return false;
			}
			s.startupDone = true;
			s.fullDuration = ((s.loops > -1) ? (s.duration * (float)s.loops) : float.PositiveInfinity);
			Sequence.StableSortSequencedObjs(s._sequencedObjs);
			if (s.isRelative)
			{
				int count = s.sequencedTweens.Count;
				for (int i = 0; i < count; i++)
				{
					Tween tween = s.sequencedTweens[i];
					if (!s.isBlendable)
					{
						s.sequencedTweens[i].isRelative = true;
					}
				}
			}
			return true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003F08 File Offset: 0x00002108
		internal static bool DoApplyTween(Sequence s, float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode)
		{
			float num = prevPosition;
			float num2 = s.position;
			if (s.easeType != Ease.Linear)
			{
				num = s.duration * EaseManager.Evaluate(s.easeType, s.customEase, num, s.duration, s.easeOvershootOrAmplitude, s.easePeriod);
				num2 = s.duration * EaseManager.Evaluate(s.easeType, s.customEase, num2, s.duration, s.easeOvershootOrAmplitude, s.easePeriod);
			}
			float num3 = 0f;
			bool flag = s.loopType == LoopType.Yoyo && ((num < s.duration) ? (prevCompletedLoops % 2 != 0) : (prevCompletedLoops % 2 == 0));
			if (s.isBackwards)
			{
				flag = !flag;
			}
			float num5;
			if (newCompletedSteps > 0)
			{
				int completedLoops = s.completedLoops;
				float position = s.position;
				int num4 = newCompletedSteps;
				int i = 0;
				num5 = num;
				if (updateMode == UpdateMode.Update)
				{
					while (i < num4)
					{
						if (i > 0)
						{
							num5 = num3;
						}
						else if (flag && !s.isBackwards)
						{
							num5 = s.duration - num5;
						}
						num3 = (flag ? 0f : s.duration);
						if (Sequence.ApplyInternalCycle(s, num5, num3, updateMode, useInversePosition, flag, true))
						{
							return true;
						}
						i++;
						if (s.loopType == LoopType.Yoyo)
						{
							flag = !flag;
						}
					}
					if (completedLoops != s.completedLoops || Math.Abs(position - s.position) > 1.401298E-45f)
					{
						return !s.active;
					}
				}
				else
				{
					if (s.loopType == LoopType.Yoyo && newCompletedSteps % 2 != 0)
					{
						flag = !flag;
						num = s.duration - num;
					}
					newCompletedSteps = 0;
				}
			}
			if (newCompletedSteps == 1 && s.isComplete)
			{
				return false;
			}
			if (newCompletedSteps > 0 && !s.isComplete)
			{
				num5 = (useInversePosition ? s.duration : 0f);
				if (s.loopType == LoopType.Restart && num3 > 0f)
				{
					Sequence.ApplyInternalCycle(s, s.duration, 0f, UpdateMode.Goto, false, false, false);
				}
			}
			else
			{
				num5 = (useInversePosition ? (s.duration - num) : num);
			}
			return Sequence.ApplyInternalCycle(s, num5, useInversePosition ? (s.duration - num2) : num2, updateMode, useInversePosition, flag, false);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000410C File Offset: 0x0000230C
		private static bool ApplyInternalCycle(Sequence s, float fromPos, float toPos, UpdateMode updateMode, bool useInverse, bool prevPosIsInverse, bool multiCycleStep = false)
		{
			if (toPos < fromPos)
			{
				int num = s._sequencedObjs.Count - 1;
				for (int i = num; i > -1; i--)
				{
					if (!s.active)
					{
						return true;
					}
					ABSSequentiable abssequentiable = s._sequencedObjs[i];
					if (abssequentiable.sequencedEndPosition >= toPos && abssequentiable.sequencedPosition <= fromPos)
					{
						if (abssequentiable.tweenType == TweenType.Callback)
						{
							if (updateMode == UpdateMode.Update && prevPosIsInverse)
							{
								Tween.OnTweenCallback(abssequentiable.onStart);
							}
						}
						else
						{
							float num2 = toPos - abssequentiable.sequencedPosition;
							if (num2 < 0f)
							{
								num2 = 0f;
							}
							Tween tween = (Tween)abssequentiable;
							if (tween.startupDone)
							{
								tween.isBackwards = true;
								if (TweenManager.Goto(tween, num2, false, updateMode))
								{
									TweenManager.Despawn(tween, false);
									s._sequencedObjs.RemoveAt(i);
									i--;
									num--;
								}
								else if (multiCycleStep && tween.tweenType == TweenType.Sequence)
								{
									if (s.position <= 0f && s.completedLoops == 0)
									{
										tween.position = 0f;
									}
									else
									{
										bool flag = s.completedLoops == 0 || (s.isBackwards && (s.completedLoops < s.loops || s.loops == -1));
										if (tween.isBackwards)
										{
											flag = !flag;
										}
										if (useInverse)
										{
											flag = !flag;
										}
										if (s.isBackwards && !useInverse && !prevPosIsInverse)
										{
											flag = !flag;
										}
										tween.position = (flag ? 0f : tween.duration);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				int num3 = s._sequencedObjs.Count;
				for (int j = 0; j < num3; j++)
				{
					if (!s.active)
					{
						return true;
					}
					ABSSequentiable abssequentiable2 = s._sequencedObjs[j];
					if (abssequentiable2.sequencedPosition <= toPos && (abssequentiable2.sequencedPosition <= 0f || abssequentiable2.sequencedEndPosition > fromPos) && (abssequentiable2.sequencedPosition > 0f || abssequentiable2.sequencedEndPosition >= fromPos))
					{
						if (abssequentiable2.tweenType == TweenType.Callback)
						{
							if (updateMode == UpdateMode.Update && ((!s.isBackwards && !useInverse && !prevPosIsInverse) || (s.isBackwards && useInverse && !prevPosIsInverse)))
							{
								Tween.OnTweenCallback(abssequentiable2.onStart);
							}
						}
						else
						{
							float num4 = toPos - abssequentiable2.sequencedPosition;
							if (num4 < 0f)
							{
								num4 = 0f;
							}
							Tween tween2 = (Tween)abssequentiable2;
							if (toPos >= abssequentiable2.sequencedEndPosition)
							{
								if (!tween2.startupDone)
								{
									TweenManager.ForceInit(tween2, true);
								}
								if (num4 < tween2.fullDuration)
								{
									num4 = tween2.fullDuration;
								}
							}
							tween2.isBackwards = false;
							if (TweenManager.Goto(tween2, num4, false, updateMode))
							{
								TweenManager.Despawn(tween2, false);
								s._sequencedObjs.RemoveAt(j);
								j--;
								num3--;
							}
							else if (multiCycleStep && tween2.tweenType == TweenType.Sequence)
							{
								if (s.position <= 0f && s.completedLoops == 0)
								{
									tween2.position = 0f;
								}
								else
								{
									bool flag2 = s.completedLoops == 0 || (!s.isBackwards && (s.completedLoops < s.loops || s.loops == -1));
									if (tween2.isBackwards)
									{
										flag2 = !flag2;
									}
									if (useInverse)
									{
										flag2 = !flag2;
									}
									if (s.isBackwards && !useInverse && !prevPosIsInverse)
									{
										flag2 = !flag2;
									}
									tween2.position = (flag2 ? 0f : tween2.duration);
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000044C0 File Offset: 0x000026C0
		private static void StableSortSequencedObjs(List<ABSSequentiable> list)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				int num = i;
				ABSSequentiable abssequentiable = list[i];
				while (num > 0 && list[num - 1].sequencedPosition > abssequentiable.sequencedPosition)
				{
					list[num] = list[num - 1];
					num--;
				}
				list[num] = abssequentiable;
			}
		}

		// Token: 0x04000065 RID: 101
		internal readonly List<Tween> sequencedTweens = new List<Tween>();

		// Token: 0x04000066 RID: 102
		private readonly List<ABSSequentiable> _sequencedObjs = new List<ABSSequentiable>();

		// Token: 0x04000067 RID: 103
		internal float lastTweenInsertTime;
	}
}
