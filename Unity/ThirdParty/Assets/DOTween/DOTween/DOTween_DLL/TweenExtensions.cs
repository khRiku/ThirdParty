using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000011 RID: 17
	public static class TweenExtensions
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00002ED9 File Offset: 0x000010D9
		public static void Complete(this Tween t)
		{
			t.Complete(false);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002EE4 File Offset: 0x000010E4
		public static void Complete(this Tween t, bool withCallbacks)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			UpdateMode updateMode = TweenManager.isUpdateLoop ? UpdateMode.IgnoreOnComplete : (withCallbacks ? UpdateMode.Update : UpdateMode.Goto);
			TweenManager.Complete(t, true, updateMode);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002F4C File Offset: 0x0000114C
		public static void Flip(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Flip(t);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002FA0 File Offset: 0x000011A0
		public static void ForceInit(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.ForceInit(t, false);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002FF4 File Offset: 0x000011F4
		public static void Goto(this Tween t, float to, bool andPlay = false)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			if (to < 0f)
			{
				to = 0f;
			}
			TweenManager.Goto(t, to, andPlay, UpdateMode.Goto);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000305C File Offset: 0x0000125C
		public static void Kill(this Tween t, bool complete = false)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			if (complete)
			{
				TweenManager.Complete(t, true, UpdateMode.Goto);
				if (t.autoKill && t.loops >= 0)
				{
					return;
				}
			}
			if (TweenManager.isUpdateLoop)
			{
				t.active = false;
				return;
			}
			TweenManager.Despawn(t, true);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000030E0 File Offset: 0x000012E0
		public static T Pause<T>(this T t) where T : Tween
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return t;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return t;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return t;
			}
			TweenManager.Pause(t);
			return t;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000315C File Offset: 0x0000135C
		public static T Play<T>(this T t) where T : Tween
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return t;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return t;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return t;
			}
			TweenManager.Play(t);
			return t;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000031D8 File Offset: 0x000013D8
		public static void PlayBackwards(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.PlayBackwards(t);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000322C File Offset: 0x0000142C
		public static void PlayForward(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.PlayForward(t);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003280 File Offset: 0x00001480
		public static void Restart(this Tween t, bool includeDelay = true, float changeDelayTo = -1f)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Restart(t, includeDelay, changeDelayTo);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000032D8 File Offset: 0x000014D8
		public static void Rewind(this Tween t, bool includeDelay = true)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.Rewind(t, includeDelay);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003330 File Offset: 0x00001530
		public static void SmoothRewind(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.SmoothRewind(t);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003384 File Offset: 0x00001584
		public static void TogglePause(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenManager.TogglePause(t);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000033D8 File Offset: 0x000015D8
		public static void GotoWaypoint(this Tween t, int waypointIndex, bool andPlay = false)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return;
			}
			if (!t.startupDone)
			{
				TweenManager.ForceInit(t, false);
			}
			if (waypointIndex < 0)
			{
				waypointIndex = 0;
			}
			else if (waypointIndex > tweenerCore.changeValue.wps.Length - 1)
			{
				waypointIndex = tweenerCore.changeValue.wps.Length - 1;
			}
			float num = 0f;
			for (int i = 0; i < waypointIndex + 1; i++)
			{
				num += tweenerCore.changeValue.wpLengths[i];
			}
			float num2 = num / tweenerCore.changeValue.length;
			if (t.loopType == LoopType.Yoyo && ((t.position < t.duration) ? (t.completedLoops % 2 != 0) : (t.completedLoops % 2 == 0)))
			{
				num2 = 1f - num2;
			}
			float to = (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops) * t.duration + num2 * t.duration;
			TweenManager.Goto(t, to, andPlay, UpdateMode.Goto);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003520 File Offset: 0x00001720
		public static YieldInstruction WaitForCompletion(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForCompletion(t));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000354F File Offset: 0x0000174F
		public static YieldInstruction WaitForRewind(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForRewind(t));
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000357E File Offset: 0x0000177E
		public static YieldInstruction WaitForKill(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForKill(t));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000035AD File Offset: 0x000017AD
		public static YieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForElapsedLoops(t, elapsedLoops));
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000035DD File Offset: 0x000017DD
		public static YieldInstruction WaitForPosition(this Tween t, float position)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForPosition(t, position));
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000360D File Offset: 0x0000180D
		public static Coroutine WaitForStart(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return DOTween.instance.StartCoroutine(DOTween.instance.WaitForStart(t));
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000363C File Offset: 0x0000183C
		public static int CompletedLoops(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0;
			}
			return t.completedLoops;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000365C File Offset: 0x0000185C
		public static float Delay(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			return t.delay;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003680 File Offset: 0x00001880
		public static float Duration(this Tween t, bool includeLoops = true)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			if (!includeLoops)
			{
				return t.duration;
			}
			if (t.loops != -1)
			{
				return t.duration * (float)t.loops;
			}
			return float.PositiveInfinity;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000036D0 File Offset: 0x000018D0
		public static float Elapsed(this Tween t, bool includeLoops = true)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			if (includeLoops)
			{
				return (float)((t.position >= t.duration) ? (t.completedLoops - 1) : t.completedLoops) * t.duration + t.position;
			}
			return t.position;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003730 File Offset: 0x00001930
		public static float ElapsedPercentage(this Tween t, bool includeLoops = true)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			if (!includeLoops)
			{
				return t.position / t.duration;
			}
			if (t.fullDuration <= 0f)
			{
				return 0f;
			}
			return ((float)((t.position >= t.duration) ? (t.completedLoops - 1) : t.completedLoops) * t.duration + t.position) / t.fullDuration;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000037B4 File Offset: 0x000019B4
		public static float ElapsedDirectionalPercentage(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0f;
			}
			float num = t.position / t.duration;
			if (t.completedLoops <= 0 || t.loopType != LoopType.Yoyo || ((t.isComplete || t.completedLoops % 2 == 0) && (!t.isComplete || t.completedLoops % 2 != 0)))
			{
				return num;
			}
			return 1f - num;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003836 File Offset: 0x00001A36
		public static bool IsActive(this Tween t)
		{
			return t.active;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000383E File Offset: 0x00001A3E
		public static bool IsBackwards(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.isBackwards;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000385E File Offset: 0x00001A5E
		public static bool IsComplete(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.isComplete;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000387E File Offset: 0x00001A7E
		public static bool IsInitialized(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.startupDone;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000389E File Offset: 0x00001A9E
		public static bool IsPlaying(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return false;
			}
			return t.isPlaying;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000038BE File Offset: 0x00001ABE
		public static int Loops(this Tween t)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return 0;
			}
			return t.loops;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000038E0 File Offset: 0x00001AE0
		public static Vector3 PathGetPoint(this Tween t, float pathPercentage)
		{
			if (pathPercentage > 1f)
			{
				pathPercentage = 1f;
			}
			else if (pathPercentage < 0f)
			{
				pathPercentage = 0f;
			}
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return Vector3.zero;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return Vector3.zero;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return Vector3.zero;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return Vector3.zero;
			}
			if (!tweenerCore.endValue.isFinalized)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogWarning("The path is not finalized yet");
				}
				return Vector3.zero;
			}
			return tweenerCore.endValue.GetPoint(pathPercentage, true);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000039AC File Offset: 0x00001BAC
		public static Vector3[] PathGetDrawPoints(this Tween t, int subdivisionsXSegment = 10)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return null;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return null;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return null;
			}
			if (!tweenerCore.endValue.isFinalized)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogWarning("The path is not finalized yet");
				}
				return null;
			}
			return Path.GetDrawPoints(tweenerCore.endValue, subdivisionsXSegment);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003A44 File Offset: 0x00001C44
		public static float PathLength(this Tween t)
		{
			if (t == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(t);
				}
				return -1f;
			}
			if (!t.active)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogInvalidTween(t);
				}
				return -1f;
			}
			if (t.isSequenced)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNestedTween(t);
				}
				return -1f;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = t as TweenerCore<Vector3, Path, PathOptions>;
			if (tweenerCore == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNonPathTween(t);
				}
				return -1f;
			}
			if (!tweenerCore.endValue.isFinalized)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogWarning("The path is not finalized yet");
				}
				return -1f;
			}
			return tweenerCore.endValue.length;
		}
	}
}
