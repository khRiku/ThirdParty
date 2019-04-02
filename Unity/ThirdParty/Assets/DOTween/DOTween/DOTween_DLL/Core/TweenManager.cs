using System;
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x0200004F RID: 79
	public static class TweenManager
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000D85C File Offset: 0x0000BA5C
		static TweenManager()
		{
			TweenManager.isUnityEditor = Application.isEditor;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D8E0 File Offset: 0x0000BAE0
		internal static TweenerCore<T1, T2, TPlugOptions> GetTweener<T1, T2, TPlugOptions>() where TPlugOptions : struct, IPlugOptions
		{
			if (TweenManager.totPooledTweeners > 0)
			{
				Type typeFromHandle = typeof(T1);
				Type typeFromHandle2 = typeof(T2);
				Type typeFromHandle3 = typeof(TPlugOptions);
				for (int i = TweenManager._maxPooledTweenerId; i > TweenManager._minPooledTweenerId - 1; i--)
				{
					Tween tween = TweenManager._pooledTweeners[i];
					if (tween != null && tween.typeofT1 == typeFromHandle && tween.typeofT2 == typeFromHandle2 && tween.typeofTPlugOptions == typeFromHandle3)
					{
						TweenerCore<T1, T2, TPlugOptions> tweenerCore = (TweenerCore<T1, T2, TPlugOptions>)tween;
						TweenManager.AddActiveTween(tweenerCore);
						TweenManager._pooledTweeners[i] = null;
						if (TweenManager._maxPooledTweenerId != TweenManager._minPooledTweenerId)
						{
							if (i == TweenManager._maxPooledTweenerId)
							{
								TweenManager._maxPooledTweenerId--;
							}
							else if (i == TweenManager._minPooledTweenerId)
							{
								TweenManager._minPooledTweenerId++;
							}
						}
						TweenManager.totPooledTweeners--;
						return tweenerCore;
					}
				}
				if (TweenManager.totTweeners >= TweenManager.maxTweeners)
				{
					TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId] = null;
					TweenManager._maxPooledTweenerId--;
					TweenManager.totPooledTweeners--;
					TweenManager.totTweeners--;
				}
			}
			else if (TweenManager.totTweeners >= TweenManager.maxTweeners - 1)
			{
				int num = TweenManager.maxTweeners;
				int num2 = TweenManager.maxSequences;
				TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.TweenersOnly);
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", num + "/" + num2).Replace("#1", TweenManager.maxTweeners + "/" + TweenManager.maxSequences));
				}
			}
			TweenerCore<T1, T2, TPlugOptions> tweenerCore2 = new TweenerCore<T1, T2, TPlugOptions>();
			TweenManager.totTweeners++;
			TweenManager.AddActiveTween(tweenerCore2);
			return tweenerCore2;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000DA90 File Offset: 0x0000BC90
		internal static Sequence GetSequence()
		{
			if (TweenManager.totPooledSequences > 0)
			{
				Sequence sequence = (Sequence)TweenManager._PooledSequences.Pop();
				TweenManager.AddActiveTween(sequence);
				TweenManager.totPooledSequences--;
				return sequence;
			}
			if (TweenManager.totSequences >= TweenManager.maxSequences - 1)
			{
				int num = TweenManager.maxTweeners;
				int num2 = TweenManager.maxSequences;
				TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.SequencesOnly);
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", num + "/" + num2).Replace("#1", TweenManager.maxTweeners + "/" + TweenManager.maxSequences));
				}
			}
			Sequence sequence2 = new Sequence();
			TweenManager.totSequences++;
			TweenManager.AddActiveTween(sequence2);
			return sequence2;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000DB58 File Offset: 0x0000BD58
		internal static void SetUpdateType(Tween t, UpdateType updateType, bool isIndependentUpdate)
		{
			if (!t.active || t.updateType == updateType)
			{
				t.updateType = updateType;
				t.isIndependentUpdate = isIndependentUpdate;
				return;
			}
			if (t.updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens--;
				TweenManager.hasActiveDefaultTweens = (TweenManager.totActiveDefaultTweens > 0);
			}
			else
			{
				UpdateType updateType2 = t.updateType;
				if (updateType2 != UpdateType.Late)
				{
					if (updateType2 == UpdateType.Fixed)
					{
						TweenManager.totActiveFixedTweens--;
						TweenManager.hasActiveFixedTweens = (TweenManager.totActiveFixedTweens > 0);
					}
					else
					{
						TweenManager.totActiveManualTweens--;
						TweenManager.hasActiveManualTweens = (TweenManager.totActiveManualTweens > 0);
					}
				}
				else
				{
					TweenManager.totActiveLateTweens--;
					TweenManager.hasActiveLateTweens = (TweenManager.totActiveLateTweens > 0);
				}
			}
			t.updateType = updateType;
			t.isIndependentUpdate = isIndependentUpdate;
			if (updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens++;
				TweenManager.hasActiveDefaultTweens = true;
				return;
			}
			if (updateType == UpdateType.Late)
			{
				TweenManager.totActiveLateTweens++;
				TweenManager.hasActiveLateTweens = true;
				return;
			}
			if (updateType == UpdateType.Fixed)
			{
				TweenManager.totActiveFixedTweens++;
				TweenManager.hasActiveFixedTweens = true;
				return;
			}
			TweenManager.totActiveManualTweens++;
			TweenManager.hasActiveManualTweens = true;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000DC6A File Offset: 0x0000BE6A
		internal static void AddActiveTweenToSequence(Tween t)
		{
			TweenManager.RemoveActiveTween(t);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000DC74 File Offset: 0x0000BE74
		internal static int DespawnAll()
		{
			int result = TweenManager.totActiveTweens;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null)
				{
					TweenManager.Despawn(tween, false);
				}
			}
			TweenManager.ClearTweenArray(TweenManager._activeTweens);
			TweenManager.hasActiveTweens = (TweenManager.hasActiveDefaultTweens = (TweenManager.hasActiveLateTweens = (TweenManager.hasActiveFixedTweens = (TweenManager.hasActiveManualTweens = false))));
			TweenManager.totActiveTweens = (TweenManager.totActiveDefaultTweens = (TweenManager.totActiveLateTweens = (TweenManager.totActiveFixedTweens = (TweenManager.totActiveManualTweens = 0))));
			TweenManager.totActiveTweeners = (TweenManager.totActiveSequences = 0);
			TweenManager._maxActiveLookupId = (TweenManager._reorganizeFromId = -1);
			TweenManager._requiresActiveReorganization = false;
			if (TweenManager.isUpdateLoop)
			{
				TweenManager._despawnAllCalledFromUpdateLoopCallback = true;
			}
			return result;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000DD20 File Offset: 0x0000BF20
		internal static void Despawn(Tween t, bool modifyActiveLists = true)
		{
			if (t.onKill != null)
			{
				Tween.OnTweenCallback(t.onKill);
			}
			if (modifyActiveLists)
			{
				TweenManager.RemoveActiveTween(t);
			}
			if (t.isRecyclable)
			{
				TweenType tweenType = t.tweenType;
				if (tweenType != TweenType.Tweener)
				{
					if (tweenType == TweenType.Sequence)
					{
						TweenManager._PooledSequences.Push(t);
						TweenManager.totPooledSequences++;
						Sequence sequence = (Sequence)t;
						int count = sequence.sequencedTweens.Count;
						for (int i = 0; i < count; i++)
						{
							TweenManager.Despawn(sequence.sequencedTweens[i], false);
						}
					}
				}
				else
				{
					if (TweenManager._maxPooledTweenerId == -1)
					{
						TweenManager._maxPooledTweenerId = TweenManager.maxTweeners - 1;
						TweenManager._minPooledTweenerId = TweenManager.maxTweeners - 1;
					}
					if (TweenManager._maxPooledTweenerId < TweenManager.maxTweeners - 1)
					{
						TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId + 1] = t;
						TweenManager._maxPooledTweenerId++;
						if (TweenManager._minPooledTweenerId > TweenManager._maxPooledTweenerId)
						{
							TweenManager._minPooledTweenerId = TweenManager._maxPooledTweenerId;
						}
					}
					else
					{
						int j = TweenManager._maxPooledTweenerId;
						while (j > -1)
						{
							if (TweenManager._pooledTweeners[j] == null)
							{
								TweenManager._pooledTweeners[j] = t;
								if (j < TweenManager._minPooledTweenerId)
								{
									TweenManager._minPooledTweenerId = j;
								}
								if (TweenManager._maxPooledTweenerId < TweenManager._minPooledTweenerId)
								{
									TweenManager._maxPooledTweenerId = TweenManager._minPooledTweenerId;
									break;
								}
								break;
							}
							else
							{
								j--;
							}
						}
					}
					TweenManager.totPooledTweeners++;
				}
			}
			else
			{
				TweenType tweenType = t.tweenType;
				if (tweenType != TweenType.Tweener)
				{
					if (tweenType == TweenType.Sequence)
					{
						TweenManager.totSequences--;
						Sequence sequence2 = (Sequence)t;
						int count2 = sequence2.sequencedTweens.Count;
						for (int k = 0; k < count2; k++)
						{
							TweenManager.Despawn(sequence2.sequencedTweens[k], false);
						}
					}
				}
				else
				{
					TweenManager.totTweeners--;
				}
			}
			t.active = false;
			t.Reset();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		internal static void PurgeAll()
		{
			for (int i = 0; i < TweenManager.totActiveTweens; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null)
				{
					tween.active = false;
					if (tween.onKill != null)
					{
						Tween.OnTweenCallback(tween.onKill);
					}
				}
			}
			TweenManager.ClearTweenArray(TweenManager._activeTweens);
			TweenManager.hasActiveTweens = (TweenManager.hasActiveDefaultTweens = (TweenManager.hasActiveLateTweens = (TweenManager.hasActiveFixedTweens = (TweenManager.hasActiveManualTweens = false))));
			TweenManager.totActiveTweens = (TweenManager.totActiveDefaultTweens = (TweenManager.totActiveLateTweens = (TweenManager.totActiveFixedTweens = (TweenManager.totActiveManualTweens = 0))));
			TweenManager.totActiveTweeners = (TweenManager.totActiveSequences = 0);
			TweenManager._maxActiveLookupId = (TweenManager._reorganizeFromId = -1);
			TweenManager._requiresActiveReorganization = false;
			TweenManager.PurgePools();
			TweenManager.ResetCapacities();
			TweenManager.totTweeners = (TweenManager.totSequences = 0);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		internal static void PurgePools()
		{
			TweenManager.totTweeners -= TweenManager.totPooledTweeners;
			TweenManager.totSequences -= TweenManager.totPooledSequences;
			TweenManager.ClearTweenArray(TweenManager._pooledTweeners);
			TweenManager._PooledSequences.Clear();
			TweenManager.totPooledTweeners = (TweenManager.totPooledSequences = 0);
			TweenManager._minPooledTweenerId = (TweenManager._maxPooledTweenerId = -1);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DFF9 File Offset: 0x0000C1F9
		internal static void ResetCapacities()
		{
			TweenManager.SetCapacities(200, 50);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000E008 File Offset: 0x0000C208
		internal static void SetCapacities(int tweenersCapacity, int sequencesCapacity)
		{
			if (tweenersCapacity < sequencesCapacity)
			{
				tweenersCapacity = sequencesCapacity;
			}
			TweenManager.maxActive = tweenersCapacity + sequencesCapacity;
			TweenManager.maxTweeners = tweenersCapacity;
			TweenManager.maxSequences = sequencesCapacity;
			Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
			Array.Resize<Tween>(ref TweenManager._pooledTweeners, tweenersCapacity);
			TweenManager._KillList.Capacity = TweenManager.maxActive;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000E05C File Offset: 0x0000C25C
		internal static int Validate()
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			int num = 0;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (!tween.Validate())
				{
					num++;
					TweenManager.MarkForKilling(tween);
				}
			}
			if (num > 0)
			{
				TweenManager.DespawnActiveTweens(TweenManager._KillList);
				TweenManager._KillList.Clear();
			}
			return num;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E0BC File Offset: 0x0000C2BC
		internal static void Update(UpdateType updateType, float deltaTime, float independentTime)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			TweenManager.isUpdateLoop = true;
			bool flag = false;
			int num = TweenManager._maxActiveLookupId + 1;
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.updateType == updateType)
				{
					if (!tween.active)
					{
						flag = true;
						TweenManager.MarkForKilling(tween);
					}
					else if (tween.isPlaying)
					{
						tween.creationLocked = true;
						float num2 = (tween.isIndependentUpdate ? independentTime : deltaTime) * tween.timeScale;
						if (num2 > 0f)
						{
							if (!tween.delayComplete)
							{
								num2 = tween.UpdateDelay(tween.elapsedDelay + num2);
								if (num2 <= -1f)
								{
									flag = true;
									TweenManager.MarkForKilling(tween);
									goto IL_20E;
								}
								if (num2 <= 0f)
								{
									goto IL_20E;
								}
								if (tween.playedOnce && tween.onPlay != null)
								{
									Tween.OnTweenCallback(tween.onPlay);
								}
							}
							if (!tween.startupDone && !tween.Startup())
							{
								flag = true;
								TweenManager.MarkForKilling(tween);
							}
							else
							{
								float num3 = tween.position;
								bool flag2 = num3 >= tween.duration;
								int num4 = tween.completedLoops;
								if (tween.duration <= 0f)
								{
									num3 = 0f;
									num4 = ((tween.loops == -1) ? (tween.completedLoops + 1) : tween.loops);
								}
								else
								{
									if (tween.isBackwards)
									{
										num3 -= num2;
										while (num3 < 0f && num4 > -1)
										{
											num3 += tween.duration;
											num4--;
										}
										if (num4 < 0 || (flag2 && num4 < 1))
										{
											num3 = 0f;
											num4 = (flag2 ? 1 : 0);
										}
									}
									else
									{
										num3 += num2;
										while (num3 >= tween.duration && (tween.loops == -1 || num4 < tween.loops))
										{
											num3 -= tween.duration;
											num4++;
										}
									}
									if (flag2)
									{
										num4--;
									}
									if (tween.loops != -1 && num4 >= tween.loops)
									{
										num3 = tween.duration;
									}
								}
								if (Tween.DoGoto(tween, num3, num4, UpdateMode.Update))
								{
									flag = true;
									TweenManager.MarkForKilling(tween);
								}
							}
						}
					}
				}
				IL_20E:;
			}
			if (flag)
			{
				if (TweenManager._despawnAllCalledFromUpdateLoopCallback)
				{
					TweenManager._despawnAllCalledFromUpdateLoopCallback = false;
				}
				else
				{
					TweenManager.DespawnActiveTweens(TweenManager._KillList);
				}
				TweenManager._KillList.Clear();
			}
			TweenManager.isUpdateLoop = false;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E310 File Offset: 0x0000C510
		internal static int FilteredOperation(OperationType operationType, FilterType filterType, object id, bool optionalBool, float optionalFloat, object optionalObj = null, object[] optionalArray = null)
		{
			int num = 0;
			bool flag = false;
			int num2 = (optionalArray == null) ? 0 : optionalArray.Length;
			bool flag2 = false;
			string b = null;
			bool flag3 = false;
			int num3 = 0;
			if (filterType == FilterType.TargetOrId || filterType == FilterType.TargetAndId)
			{
				if (id is string)
				{
					flag2 = true;
					b = (string)id;
				}
				else if (id is int)
				{
					flag3 = true;
					num3 = (int)id;
				}
			}
			for (int i = TweenManager._maxActiveLookupId; i > -1; i--)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.active)
				{
					bool flag4 = false;
					switch (filterType)
					{
					case FilterType.All:
						flag4 = true;
						break;
					case FilterType.TargetOrId:
						if (flag2)
						{
							flag4 = (tween.stringId != null && tween.stringId == b);
						}
						else if (flag3)
						{
							flag4 = (tween.intId == num3);
						}
						else
						{
							flag4 = ((tween.id != null && id.Equals(tween.id)) || (tween.target != null && id.Equals(tween.target)));
						}
						break;
					case FilterType.TargetAndId:
						if (flag2)
						{
							flag4 = (tween.target != null && tween.stringId == b && optionalObj != null && optionalObj.Equals(tween.target));
						}
						else if (flag3)
						{
							flag4 = (tween.target != null && tween.intId == num3 && optionalObj != null && optionalObj.Equals(tween.target));
						}
						else
						{
							flag4 = (tween.id != null && tween.target != null && optionalObj != null && id.Equals(tween.id) && optionalObj.Equals(tween.target));
						}
						break;
					case FilterType.AllExceptTargetsOrIds:
						flag4 = true;
						for (int j = 0; j < num2; j++)
						{
							object obj = optionalArray[j];
							if (obj is string)
							{
								flag2 = true;
								b = (string)obj;
							}
							else if (obj is int)
							{
								flag3 = true;
								num3 = (int)obj;
							}
							if (flag2 && tween.stringId == b)
							{
								flag4 = false;
								break;
							}
							if (flag3 && tween.intId == num3)
							{
								flag4 = false;
								break;
							}
							if ((tween.id != null && obj.Equals(tween.id)) || (tween.target != null && obj.Equals(tween.target)))
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
						{
							bool autoKill = tween.autoKill;
							if (TweenManager.Complete(tween, false, (optionalFloat > 0f) ? UpdateMode.Update : UpdateMode.Goto))
							{
								num += ((!optionalBool) ? 1 : (autoKill ? 1 : 0));
								if (autoKill)
								{
									if (TweenManager.isUpdateLoop)
									{
										tween.active = false;
									}
									else
									{
										flag = true;
										TweenManager._KillList.Add(tween);
									}
								}
							}
							break;
						}
						case OperationType.Despawn:
							num++;
							tween.active = false;
							if (!TweenManager.isUpdateLoop)
							{
								TweenManager.Despawn(tween, false);
								flag = true;
								TweenManager._KillList.Add(tween);
							}
							break;
						case OperationType.Flip:
							if (TweenManager.Flip(tween))
							{
								num++;
							}
							break;
						case OperationType.Goto:
							TweenManager.Goto(tween, optionalFloat, optionalBool, UpdateMode.Goto);
							num++;
							break;
						case OperationType.Pause:
							if (TweenManager.Pause(tween))
							{
								num++;
							}
							break;
						case OperationType.Play:
							if (TweenManager.Play(tween))
							{
								num++;
							}
							break;
						case OperationType.PlayForward:
							if (TweenManager.PlayForward(tween))
							{
								num++;
							}
							break;
						case OperationType.PlayBackwards:
							if (TweenManager.PlayBackwards(tween))
							{
								num++;
							}
							break;
						case OperationType.Rewind:
							if (TweenManager.Rewind(tween, optionalBool))
							{
								num++;
							}
							break;
						case OperationType.SmoothRewind:
							if (TweenManager.SmoothRewind(tween))
							{
								num++;
							}
							break;
						case OperationType.Restart:
							if (TweenManager.Restart(tween, optionalBool, optionalFloat))
							{
								num++;
							}
							break;
						case OperationType.TogglePause:
							if (TweenManager.TogglePause(tween))
							{
								num++;
							}
							break;
						case OperationType.IsTweening:
							if ((!tween.isComplete || !tween.autoKill) && (!optionalBool || tween.isPlaying))
							{
								num++;
							}
							break;
						}
					}
				}
			}
			if (flag)
			{
				for (int k = TweenManager._KillList.Count - 1; k > -1; k--)
				{
					Tween tween2 = TweenManager._KillList[k];
					if (tween2.activeId != -1)
					{
						TweenManager.RemoveActiveTween(tween2);
					}
				}
				TweenManager._KillList.Clear();
			}
			return num;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E788 File Offset: 0x0000C988
		internal static bool Complete(Tween t, bool modifyActiveLists = true, UpdateMode updateMode = UpdateMode.Goto)
		{
			if (t.loops == -1)
			{
				return false;
			}
			if (!t.isComplete)
			{
				Tween.DoGoto(t, t.duration, t.loops, updateMode);
				t.isPlaying = false;
				if (t.autoKill)
				{
					if (TweenManager.isUpdateLoop)
					{
						t.active = false;
					}
					else
					{
						TweenManager.Despawn(t, modifyActiveLists);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000E7E5 File Offset: 0x0000C9E5
		internal static bool Flip(Tween t)
		{
			t.isBackwards = !t.isBackwards;
			return true;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000E7F7 File Offset: 0x0000C9F7
		internal static void ForceInit(Tween t, bool isSequenced = false)
		{
			if (t.startupDone)
			{
				return;
			}
			if (!t.Startup() && !isSequenced)
			{
				if (TweenManager.isUpdateLoop)
				{
					t.active = false;
					return;
				}
				TweenManager.RemoveActiveTween(t);
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000E824 File Offset: 0x0000CA24
		internal static bool Goto(Tween t, float to, bool andPlay = false, UpdateMode updateMode = UpdateMode.Goto)
		{
			bool isPlaying = t.isPlaying;
			t.isPlaying = andPlay;
			t.delayComplete = true;
			t.elapsedDelay = t.delay;
			int num = (t.duration <= 0f) ? 1 : Mathf.FloorToInt(to / t.duration);
			float num2 = to % t.duration;
			if (t.loops != -1 && num >= t.loops)
			{
				num = t.loops;
				num2 = t.duration;
			}
			else if (num2 >= t.duration)
			{
				num2 = 0f;
			}
			bool flag = Tween.DoGoto(t, num2, num, updateMode);
			if (!andPlay && isPlaying && !flag && t.onPause != null)
			{
				Tween.OnTweenCallback(t.onPause);
			}
			return flag;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		internal static bool Pause(Tween t)
		{
			if (t.isPlaying)
			{
				t.isPlaying = false;
				if (t.onPause != null)
				{
					Tween.OnTweenCallback(t.onPause);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		internal static bool Play(Tween t)
		{
			if (!t.isPlaying && ((!t.isBackwards && !t.isComplete) || (t.isBackwards && (t.completedLoops > 0 || t.position > 0f))))
			{
				t.isPlaying = true;
				if (t.playedOnce && t.delayComplete && t.onPlay != null)
				{
					Tween.OnTweenCallback(t.onPlay);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000E96D File Offset: 0x0000CB6D
		internal static bool PlayBackwards(Tween t)
		{
			if (t.completedLoops == 0 && t.position <= 0f)
			{
				TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, true);
			}
			if (!t.isBackwards)
			{
				t.isBackwards = true;
				TweenManager.Play(t);
				return true;
			}
			return TweenManager.Play(t);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000E9A9 File Offset: 0x0000CBA9
		internal static bool PlayForward(Tween t)
		{
			if (t.isBackwards)
			{
				t.isBackwards = false;
				TweenManager.Play(t);
				return true;
			}
			return TweenManager.Play(t);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000E9CC File Offset: 0x0000CBCC
		internal static bool Restart(Tween t, bool includeDelay = true, float changeDelayTo = -1f)
		{
			bool flag = !t.isPlaying;
			t.isBackwards = false;
			if (changeDelayTo >= 0f)
			{
				t.delay = changeDelayTo;
			}
			TweenManager.Rewind(t, includeDelay);
			t.isPlaying = true;
			if (flag && t.playedOnce && t.delayComplete && t.onPlay != null)
			{
				Tween.OnTweenCallback(t.onPlay);
			}
			return true;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000EA30 File Offset: 0x0000CC30
		internal static bool Rewind(Tween t, bool includeDelay = true)
		{
			bool isPlaying = t.isPlaying;
			t.isPlaying = false;
			bool result = false;
			if (t.delay > 0f)
			{
				if (includeDelay)
				{
					result = (t.delay > 0f && t.elapsedDelay > 0f);
					t.elapsedDelay = 0f;
					t.delayComplete = false;
				}
				else
				{
					result = (t.elapsedDelay < t.delay);
					t.elapsedDelay = t.delay;
					t.delayComplete = true;
				}
			}
			if (t.position > 0f || t.completedLoops > 0 || !t.startupDone)
			{
				result = true;
				if (!Tween.DoGoto(t, 0f, 0, UpdateMode.Goto) && isPlaying && t.onPause != null)
				{
					Tween.OnTweenCallback(t.onPause);
				}
			}
			else
			{
				TweenManager.ManageOnRewindCallbackWhenAlreadyRewinded(t, false);
			}
			return result;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000EB04 File Offset: 0x0000CD04
		internal static bool SmoothRewind(Tween t)
		{
			bool result = false;
			if (t.delay > 0f)
			{
				result = (t.elapsedDelay < t.delay);
				t.elapsedDelay = t.delay;
				t.delayComplete = true;
			}
			if (t.position > 0f || t.completedLoops > 0 || !t.startupDone)
			{
				result = true;
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
			return result;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000EB9E File Offset: 0x0000CD9E
		internal static bool TogglePause(Tween t)
		{
			if (t.isPlaying)
			{
				return TweenManager.Pause(t);
			}
			return TweenManager.Play(t);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000EBB5 File Offset: 0x0000CDB5
		internal static int TotalPooledTweens()
		{
			return TweenManager.totPooledTweeners + TweenManager.totPooledSequences;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		internal static int TotalPlayingTweens()
		{
			if (!TweenManager.hasActiveTweens)
			{
				return 0;
			}
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			int num = 0;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.isPlaying)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000EC14 File Offset: 0x0000CE14
		internal static List<Tween> GetActiveTweens(bool playing, List<Tween> fillableList = null)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			if (fillableList == null)
			{
				fillableList = new List<Tween>(num);
			}
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween.isPlaying == playing)
				{
					fillableList.Add(tween);
				}
			}
			if (fillableList.Count > 0)
			{
				return fillableList;
			}
			return null;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000EC78 File Offset: 0x0000CE78
		internal static List<Tween> GetTweensById(object id, bool playingOnly, List<Tween> fillableList = null)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			if (fillableList == null)
			{
				fillableList = new List<Tween>(num);
			}
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && object.Equals(id, tween.id) && (!playingOnly || tween.isPlaying))
				{
					fillableList.Add(tween);
				}
			}
			if (fillableList.Count > 0)
			{
				return fillableList;
			}
			return null;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		internal static List<Tween> GetTweensByTarget(object target, bool playingOnly, List<Tween> fillableList = null)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			if (fillableList == null)
			{
				fillableList = new List<Tween>(num);
			}
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween.target == target && (!playingOnly || tween.isPlaying))
				{
					fillableList.Add(tween);
				}
			}
			if (fillableList.Count > 0)
			{
				return fillableList;
			}
			return null;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000ED5F File Offset: 0x0000CF5F
		private static void MarkForKilling(Tween t)
		{
			t.active = false;
			TweenManager._KillList.Add(t);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000ED74 File Offset: 0x0000CF74
		private static void AddActiveTween(Tween t)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			t.active = true;
			t.updateType = DOTween.defaultUpdateType;
			t.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
			t.activeId = (TweenManager._maxActiveLookupId = TweenManager.totActiveTweens);
			TweenManager._activeTweens[TweenManager.totActiveTweens] = t;
			if (t.updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens++;
				TweenManager.hasActiveDefaultTweens = true;
			}
			else
			{
				UpdateType updateType = t.updateType;
				if (updateType != UpdateType.Late)
				{
					if (updateType == UpdateType.Fixed)
					{
						TweenManager.totActiveFixedTweens++;
						TweenManager.hasActiveFixedTweens = true;
					}
					else
					{
						TweenManager.totActiveManualTweens++;
						TweenManager.hasActiveManualTweens = true;
					}
				}
				else
				{
					TweenManager.totActiveLateTweens++;
					TweenManager.hasActiveLateTweens = true;
				}
			}
			TweenManager.totActiveTweens++;
			if (t.tweenType == TweenType.Tweener)
			{
				TweenManager.totActiveTweeners++;
			}
			else
			{
				TweenManager.totActiveSequences++;
			}
			TweenManager.hasActiveTweens = true;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000EE60 File Offset: 0x0000D060
		private static void ReorganizeActiveTweens()
		{
			if (TweenManager.totActiveTweens <= 0)
			{
				TweenManager._maxActiveLookupId = -1;
				TweenManager._requiresActiveReorganization = false;
				TweenManager._reorganizeFromId = -1;
				return;
			}
			if (TweenManager._reorganizeFromId == TweenManager._maxActiveLookupId)
			{
				TweenManager._maxActiveLookupId--;
				TweenManager._requiresActiveReorganization = false;
				TweenManager._reorganizeFromId = -1;
				return;
			}
			int num = 1;
			int num2 = TweenManager._maxActiveLookupId + 1;
			TweenManager._maxActiveLookupId = TweenManager._reorganizeFromId - 1;
			for (int i = TweenManager._reorganizeFromId + 1; i < num2; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween == null)
				{
					num++;
				}
				else
				{
					tween.activeId = (TweenManager._maxActiveLookupId = i - num);
					TweenManager._activeTweens[i - num] = tween;
					TweenManager._activeTweens[i] = null;
				}
			}
			TweenManager._requiresActiveReorganization = false;
			TweenManager._reorganizeFromId = -1;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000EF14 File Offset: 0x0000D114
		private static void DespawnActiveTweens(List<Tween> tweens)
		{
			for (int i = tweens.Count - 1; i > -1; i--)
			{
				TweenManager.Despawn(tweens[i], true);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000EF44 File Offset: 0x0000D144
		private static void RemoveActiveTween(Tween t)
		{
			int activeId = t.activeId;
			t.activeId = -1;
			TweenManager._requiresActiveReorganization = true;
			if (TweenManager._reorganizeFromId == -1 || TweenManager._reorganizeFromId > activeId)
			{
				TweenManager._reorganizeFromId = activeId;
			}
			TweenManager._activeTweens[activeId] = null;
			if (t.updateType == UpdateType.Normal)
			{
				if (TweenManager.totActiveDefaultTweens > 0)
				{
					TweenManager.totActiveDefaultTweens--;
					TweenManager.hasActiveDefaultTweens = (TweenManager.totActiveDefaultTweens > 0);
				}
				else
				{
					Debugger.LogRemoveActiveTweenError("totActiveDefaultTweens");
				}
			}
			else
			{
				UpdateType updateType = t.updateType;
				if (updateType != UpdateType.Late)
				{
					if (updateType == UpdateType.Fixed)
					{
						if (TweenManager.totActiveFixedTweens > 0)
						{
							TweenManager.totActiveFixedTweens--;
							TweenManager.hasActiveFixedTweens = (TweenManager.totActiveFixedTweens > 0);
						}
						else
						{
							Debugger.LogRemoveActiveTweenError("totActiveFixedTweens");
						}
					}
					else if (TweenManager.totActiveManualTweens > 0)
					{
						TweenManager.totActiveManualTweens--;
						TweenManager.hasActiveManualTweens = (TweenManager.totActiveManualTweens > 0);
					}
					else
					{
						Debugger.LogRemoveActiveTweenError("totActiveManualTweens");
					}
				}
				else if (TweenManager.totActiveLateTweens > 0)
				{
					TweenManager.totActiveLateTweens--;
					TweenManager.hasActiveLateTweens = (TweenManager.totActiveLateTweens > 0);
				}
				else
				{
					Debugger.LogRemoveActiveTweenError("totActiveLateTweens");
				}
			}
			TweenManager.totActiveTweens--;
			TweenManager.hasActiveTweens = (TweenManager.totActiveTweens > 0);
			if (t.tweenType == TweenType.Tweener)
			{
				TweenManager.totActiveTweeners--;
			}
			else
			{
				TweenManager.totActiveSequences--;
			}
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
			if (TweenManager.totActiveSequences < 0)
			{
				TweenManager.totActiveSequences = 0;
				Debugger.LogRemoveActiveTweenError("totActiveSequences");
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
		private static void ClearTweenArray(Tween[] tweens)
		{
			int num = tweens.Length;
			for (int i = 0; i < num; i++)
			{
				tweens[i] = null;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000F104 File Offset: 0x0000D304
		private static void IncreaseCapacities(TweenManager.CapacityIncreaseMode increaseMode)
		{
			int num = 0;
			int num2 = Mathf.Max((int)((float)TweenManager.maxTweeners * 1.5f), 200);
			int num3 = Mathf.Max((int)((float)TweenManager.maxSequences * 1.5f), 50);
			if (increaseMode != TweenManager.CapacityIncreaseMode.TweenersOnly)
			{
				if (increaseMode != TweenManager.CapacityIncreaseMode.SequencesOnly)
				{
					num += num2;
					TweenManager.maxTweeners += num2;
					TweenManager.maxSequences += num3;
					Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
				}
				else
				{
					num += num3;
					TweenManager.maxSequences += num3;
				}
			}
			else
			{
				num += num2;
				TweenManager.maxTweeners += num2;
				Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
			}
			TweenManager.maxActive = TweenManager.maxTweeners + TweenManager.maxSequences;
			Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
			if (num > 0)
			{
				TweenManager._KillList.Capacity += num;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F1DD File Offset: 0x0000D3DD
		private static void ManageOnRewindCallbackWhenAlreadyRewinded(Tween t, bool isPlayBackwardsOrSmoothRewind)
		{
			if (t.onRewind == null)
			{
				return;
			}
			if (isPlayBackwardsOrSmoothRewind)
			{
				if (DOTween.rewindCallbackMode == RewindCallbackMode.FireAlways)
				{
					t.onRewind();
					return;
				}
			}
			else if (DOTween.rewindCallbackMode != RewindCallbackMode.FireIfPositionChanged)
			{
				t.onRewind();
			}
		}

		// Token: 0x0400013A RID: 314
		private const int _DefaultMaxTweeners = 200;

		// Token: 0x0400013B RID: 315
		private const int _DefaultMaxSequences = 50;

		// Token: 0x0400013C RID: 316
		private const string _MaxTweensReached = "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup";

		// Token: 0x0400013D RID: 317
		internal static bool isUnityEditor;

		// Token: 0x0400013E RID: 318
		public static bool isDebugBuild;

		// Token: 0x0400013F RID: 319
		internal static int maxActive = 250;

		// Token: 0x04000140 RID: 320
		internal static int maxTweeners = 200;

		// Token: 0x04000141 RID: 321
		internal static int maxSequences = 50;

		// Token: 0x04000142 RID: 322
		internal static bool hasActiveTweens;

		// Token: 0x04000143 RID: 323
		internal static bool hasActiveDefaultTweens;

		// Token: 0x04000144 RID: 324
		internal static bool hasActiveLateTweens;

		// Token: 0x04000145 RID: 325
		internal static bool hasActiveFixedTweens;

		// Token: 0x04000146 RID: 326
		internal static bool hasActiveManualTweens;

		// Token: 0x04000147 RID: 327
		internal static int totActiveTweens;

		// Token: 0x04000148 RID: 328
		internal static int totActiveDefaultTweens;

		// Token: 0x04000149 RID: 329
		internal static int totActiveLateTweens;

		// Token: 0x0400014A RID: 330
		internal static int totActiveFixedTweens;

		// Token: 0x0400014B RID: 331
		internal static int totActiveManualTweens;

		// Token: 0x0400014C RID: 332
		internal static int totActiveTweeners;

		// Token: 0x0400014D RID: 333
		internal static int totActiveSequences;

		// Token: 0x0400014E RID: 334
		internal static int totPooledTweeners;

		// Token: 0x0400014F RID: 335
		internal static int totPooledSequences;

		// Token: 0x04000150 RID: 336
		internal static int totTweeners;

		// Token: 0x04000151 RID: 337
		internal static int totSequences;

		// Token: 0x04000152 RID: 338
		internal static bool isUpdateLoop;

		// Token: 0x04000153 RID: 339
		internal static Tween[] _activeTweens = new Tween[250];

		// Token: 0x04000154 RID: 340
		private static Tween[] _pooledTweeners = new Tween[200];

		// Token: 0x04000155 RID: 341
		private static readonly Stack<Tween> _PooledSequences = new Stack<Tween>();

		// Token: 0x04000156 RID: 342
		private static readonly List<Tween> _KillList = new List<Tween>(250);

		// Token: 0x04000157 RID: 343
		private static int _maxActiveLookupId = -1;

		// Token: 0x04000158 RID: 344
		private static bool _requiresActiveReorganization;

		// Token: 0x04000159 RID: 345
		private static int _reorganizeFromId = -1;

		// Token: 0x0400015A RID: 346
		private static int _minPooledTweenerId = -1;

		// Token: 0x0400015B RID: 347
		private static int _maxPooledTweenerId = -1;

		// Token: 0x0400015C RID: 348
		private static bool _despawnAllCalledFromUpdateLoopCallback;

		// Token: 0x020000AF RID: 175
		internal enum CapacityIncreaseMode
		{
			// Token: 0x0400021E RID: 542
			TweenersAndSequences,
			// Token: 0x0400021F RID: 543
			TweenersOnly,
			// Token: 0x04000220 RID: 544
			SequencesOnly
		}
	}
}
