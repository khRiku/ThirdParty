using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000008 RID: 8
	public class DOTween
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020D1 File Offset: 0x000002D1
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020D8 File Offset: 0x000002D8
		public static LogBehaviour logBehaviour
		{
			get
			{
				return DOTween._logBehaviour;
			}
			set
			{
				DOTween._logBehaviour = value;
				Debugger.SetLogPriority(DOTween._logBehaviour);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000020EA File Offset: 0x000002EA
		public static IDOTweenInit Init(bool? recycleAllByDefault = null, bool? useSafeMode = null, LogBehaviour? logBehaviour = null)
		{
			if (DOTween.initialized)
			{
				return DOTween.instance;
			}
			if (!Application.isPlaying || DOTween.isQuitting)
			{
				return null;
			}
			return DOTween.Init(Resources.Load("DOTweenSettings") as DOTweenSettings, recycleAllByDefault, useSafeMode, logBehaviour);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002120 File Offset: 0x00000320
		private static void AutoInit()
		{
			DOTween.Init(Resources.Load("DOTweenSettings") as DOTweenSettings, null, null, null);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002160 File Offset: 0x00000360
		private static IDOTweenInit Init(DOTweenSettings settings, bool? recycleAllByDefault, bool? useSafeMode, LogBehaviour? logBehaviour)
		{
			DOTween.initialized = true;
			if (recycleAllByDefault != null)
			{
				DOTween.defaultRecyclable = recycleAllByDefault.Value;
			}
			if (useSafeMode != null)
			{
				DOTween.useSafeMode = useSafeMode.Value;
			}
			if (logBehaviour != null)
			{
				DOTween.logBehaviour = logBehaviour.Value;
			}
			DOTweenComponent.Create();
			if (settings != null)
			{
				if (useSafeMode == null)
				{
					DOTween.useSafeMode = settings.useSafeMode;
				}
				if (logBehaviour == null)
				{
					DOTween.logBehaviour = settings.logBehaviour;
				}
				if (recycleAllByDefault == null)
				{
					DOTween.defaultRecyclable = settings.defaultRecyclable;
				}
				DOTween.timeScale = settings.timeScale;
				DOTween.useSmoothDeltaTime = settings.useSmoothDeltaTime;
				DOTween.maxSmoothUnscaledTime = settings.maxSmoothUnscaledTime;
				DOTween.rewindCallbackMode = settings.rewindCallbackMode;
				DOTween.defaultRecyclable = ((recycleAllByDefault == null) ? settings.defaultRecyclable : recycleAllByDefault.Value);
				DOTween.showUnityEditorReport = settings.showUnityEditorReport;
				DOTween.drawGizmos = settings.drawGizmos;
				DOTween.defaultAutoPlay = settings.defaultAutoPlay;
				DOTween.defaultUpdateType = settings.defaultUpdateType;
				DOTween.defaultTimeScaleIndependent = settings.defaultTimeScaleIndependent;
				DOTween.defaultEaseType = settings.defaultEaseType;
				DOTween.defaultEaseOvershootOrAmplitude = settings.defaultEaseOvershootOrAmplitude;
				DOTween.defaultEasePeriod = settings.defaultEasePeriod;
				DOTween.defaultAutoKill = settings.defaultAutoKill;
				DOTween.defaultLoopType = settings.defaultLoopType;
			}
			if (Debugger.logPriority >= 2)
			{
				Debugger.Log(string.Concat(new object[]
				{
					"DOTween initialization (useSafeMode: ",
					DOTween.useSafeMode.ToString(),
					", recycling: ",
					DOTween.defaultRecyclable ? "ON" : "OFF",
					", logBehaviour: ",
					DOTween.logBehaviour,
					")"
				}));
			}
			return DOTween.instance;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002323 File Offset: 0x00000523
		public static void SetTweensCapacity(int tweenersCapacity, int sequencesCapacity)
		{
			TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000232C File Offset: 0x0000052C
		public static void Clear(bool destroy = false)
		{
			TweenManager.PurgeAll();
			PluginsManager.PurgeAll();
			if (!destroy)
			{
				return;
			}
			DOTween.initialized = false;
			DOTween.useSafeMode = false;
			DOTween.showUnityEditorReport = false;
			DOTween.drawGizmos = true;
			DOTween.timeScale = 1f;
			DOTween.useSmoothDeltaTime = false;
			DOTween.logBehaviour = LogBehaviour.ErrorsOnly;
			DOTween.defaultEaseType = Ease.OutQuad;
			DOTween.defaultEaseOvershootOrAmplitude = 1.70158f;
			DOTween.defaultEasePeriod = 0f;
			DOTween.defaultUpdateType = UpdateType.Normal;
			DOTween.defaultTimeScaleIndependent = false;
			DOTween.defaultAutoPlay = AutoPlay.All;
			DOTween.defaultLoopType = LoopType.Restart;
			DOTween.defaultAutoKill = true;
			DOTween.defaultRecyclable = false;
			DOTween.maxActiveTweenersReached = (DOTween.maxActiveSequencesReached = 0);
			DOTweenComponent.DestroyInstance();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023C4 File Offset: 0x000005C4
		public static void ClearCachedTweens()
		{
			TweenManager.PurgePools();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023CB File Offset: 0x000005CB
		public static int Validate()
		{
			return TweenManager.Validate();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023D2 File Offset: 0x000005D2
		public static void ManualUpdate(float deltaTime, float unscaledDeltaTime)
		{
			DOTween.InitCheck();
			if (TweenManager.hasActiveManualTweens)
			{
				TweenManager.Update(UpdateType.Manual, deltaTime * DOTween.timeScale, unscaledDeltaTime * DOTween.timeScale);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023F4 File Offset: 0x000005F4
		public static TweenerCore<float, float, FloatOptions> To(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
		{
			return DOTween.ApplyTo<float, float, FloatOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002400 File Offset: 0x00000600
		public static TweenerCore<double, double, NoOptions> To(DOGetter<double> getter, DOSetter<double> setter, double endValue, float duration)
		{
			return DOTween.ApplyTo<double, double, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000240C File Offset: 0x0000060C
		public static Tweener To(DOGetter<int> getter, DOSetter<int> setter, int endValue, float duration)
		{
			return DOTween.ApplyTo<int, int, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002418 File Offset: 0x00000618
		public static Tweener To(DOGetter<uint> getter, DOSetter<uint> setter, uint endValue, float duration)
		{
			return DOTween.ApplyTo<uint, uint, UintOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002424 File Offset: 0x00000624
		public static Tweener To(DOGetter<long> getter, DOSetter<long> setter, long endValue, float duration)
		{
			return DOTween.ApplyTo<long, long, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002430 File Offset: 0x00000630
		public static Tweener To(DOGetter<ulong> getter, DOSetter<ulong> setter, ulong endValue, float duration)
		{
			return DOTween.ApplyTo<ulong, ulong, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000243C File Offset: 0x0000063C
		public static TweenerCore<string, string, StringOptions> To(DOGetter<string> getter, DOSetter<string> setter, string endValue, float duration)
		{
			return DOTween.ApplyTo<string, string, StringOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002448 File Offset: 0x00000648
		public static TweenerCore<Vector2, Vector2, VectorOptions> To(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector2, Vector2, VectorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002454 File Offset: 0x00000654
		public static TweenerCore<Vector3, Vector3, VectorOptions> To(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002460 File Offset: 0x00000660
		public static TweenerCore<Vector4, Vector4, VectorOptions> To(DOGetter<Vector4> getter, DOSetter<Vector4> setter, Vector4 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector4, Vector4, VectorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000246C File Offset: 0x0000066C
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> To(DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, Vector3 endValue, float duration)
		{
			return DOTween.ApplyTo<Quaternion, Vector3, QuaternionOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002478 File Offset: 0x00000678
		public static TweenerCore<Color, Color, ColorOptions> To(DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, float duration)
		{
			return DOTween.ApplyTo<Color, Color, ColorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002484 File Offset: 0x00000684
		public static TweenerCore<Rect, Rect, RectOptions> To(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect endValue, float duration)
		{
			return DOTween.ApplyTo<Rect, Rect, RectOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002490 File Offset: 0x00000690
		public static Tweener To(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset endValue, float duration)
		{
			return DOTween.ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000249C File Offset: 0x0000069C
		public static TweenerCore<T1, T2, TPlugOptions> To<T1, T2, TPlugOptions>(ABSTweenPlugin<T1, T2, TPlugOptions> plugin, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration) where TPlugOptions : struct, IPlugOptions
		{
			return DOTween.ApplyTo<T1, T2, TPlugOptions>(getter, setter, endValue, duration, plugin);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024A9 File Offset: 0x000006A9
		public static TweenerCore<Vector3, Vector3, VectorOptions> ToAxis(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, new Vector3(endValue, endValue, endValue), duration, null);
			tweenerCore.plugOptions.axisConstraint = axisConstraint;
			return tweenerCore;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024C9 File Offset: 0x000006C9
		public static Tweener ToAlpha(DOGetter<Color> getter, DOSetter<Color> setter, float endValue, float duration)
		{
			return DOTween.ApplyTo<Color, Color, ColorOptions>(getter, setter, new Color(0f, 0f, 0f, endValue), duration, null).SetOptions(true);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000024F0 File Offset: 0x000006F0
		public static Tweener To(DOSetter<float> setter, float startValue, float endValue, float duration)
		{
			return DOTween.To(() => startValue, delegate(float x)
			{
				startValue = x;
				setter(startValue);
			}, endValue, duration).NoFrom<float, float, FloatOptions>();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002538 File Offset: 0x00000738
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Punch(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 direction, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (elasticity > 1f)
			{
				elasticity = 1f;
			}
			else if (elasticity < 0f)
			{
				elasticity = 0f;
			}
			float num = direction.magnitude;
			int num2 = (int)((float)vibrato * duration);
			if (num2 < 2)
			{
				num2 = 2;
			}
			float num3 = num / (float)num2;
			float[] array = new float[num2];
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)(i + 1) / (float)num2;
				float num6 = duration * num5;
				num4 += num6;
				array[i] = num6;
			}
			float num7 = duration / num4;
			for (int j = 0; j < num2; j++)
			{
				array[j] *= num7;
			}
			Vector3[] array2 = new Vector3[num2];
			for (int k = 0; k < num2; k++)
			{
				if (k < num2 - 1)
				{
					if (k == 0)
					{
						array2[k] = direction;
					}
					else if (k % 2 != 0)
					{
						array2[k] = -Vector3.ClampMagnitude(direction, num * elasticity);
					}
					else
					{
						array2[k] = Vector3.ClampMagnitude(direction, num);
					}
					num -= num3;
				}
				else
				{
					array2[k] = Vector3.zero;
				}
			}
			return DOTween.ToArray(getter, setter, array2, array).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>().SetSpecialStartupMode(SpecialStartupMode.SetPunch);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002668 File Offset: 0x00000868
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool ignoreZAxis = true, bool fadeOut = true)
		{
			return DOTween.Shake(getter, setter, duration, new Vector3(strength, strength, strength), vibrato, randomness, ignoreZAxis, false, fadeOut);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002690 File Offset: 0x00000890
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			return DOTween.Shake(getter, setter, duration, strength, vibrato, randomness, false, true, fadeOut);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000026B0 File Offset: 0x000008B0
		private static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, Vector3 strength, int vibrato, float randomness, bool ignoreZAxis, bool vectorBased, bool fadeOut)
		{
			float num = vectorBased ? strength.magnitude : strength.x;
			int num2 = (int)((float)vibrato * duration);
			if (num2 < 2)
			{
				num2 = 2;
			}
			float num3 = num / (float)num2;
			float[] array = new float[num2];
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)(i + 1) / (float)num2;
				float num6 = fadeOut ? (duration * num5) : (duration / (float)num2);
				num4 += num6;
				array[i] = num6;
			}
			float num7 = duration / num4;
			for (int j = 0; j < num2; j++)
			{
				array[j] *= num7;
			}
			float num8 = UnityEngine.Random.Range(0f, 360f);
			Vector3[] array2 = new Vector3[num2];
			for (int k = 0; k < num2; k++)
			{
				if (k < num2 - 1)
				{
					if (k > 0)
					{
						num8 = num8 - 180f + UnityEngine.Random.Range(-randomness, randomness);
					}
					if (vectorBased)
					{
						Vector3 vector = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomness, randomness), Vector3.up) * Utils.Vector3FromAngle(num8, num);
						vector.x = Vector3.ClampMagnitude(vector, strength.x).x;
						vector.y = Vector3.ClampMagnitude(vector, strength.y).y;
						vector.z = Vector3.ClampMagnitude(vector, strength.z).z;
						array2[k] = vector;
						if (fadeOut)
						{
							num -= num3;
						}
						strength = Vector3.ClampMagnitude(strength, num);
					}
					else
					{
						if (ignoreZAxis)
						{
							array2[k] = Utils.Vector3FromAngle(num8, num);
						}
						else
						{
							Quaternion quaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomness, randomness), Vector3.up);
							array2[k] = quaternion * Utils.Vector3FromAngle(num8, num);
						}
						if (fadeOut)
						{
							num -= num3;
						}
					}
				}
				else
				{
					array2[k] = Vector3.zero;
				}
			}
			return DOTween.ToArray(getter, setter, array2, array).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>().SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000028A4 File Offset: 0x00000AA4
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> ToArray(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3[] endValues, float[] durations)
		{
			int num = durations.Length;
			if (num != endValues.Length)
			{
				Debugger.LogError("To Vector3 array tween: endValues and durations arrays must have the same length");
				return null;
			}
			Vector3[] array = new Vector3[num];
			float[] array2 = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = endValues[i];
				array2[i] = durations[i];
			}
			float num2 = 0f;
			for (int j = 0; j < num; j++)
			{
				num2 += array2[j];
			}
			TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> tweenerCore = DOTween.ApplyTo<Vector3, Vector3[], Vector3ArrayOptions>(getter, setter, array, num2, null).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>();
			tweenerCore.plugOptions.durations = array2;
			return tweenerCore;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002935 File Offset: 0x00000B35
		internal static TweenerCore<Color2, Color2, ColorOptions> To(DOGetter<Color2> getter, DOSetter<Color2> setter, Color2 endValue, float duration)
		{
			return DOTween.ApplyTo<Color2, Color2, ColorOptions>(getter, setter, endValue, duration, null);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002941 File Offset: 0x00000B41
		public static Sequence Sequence()
		{
			DOTween.InitCheck();
			Sequence sequence = TweenManager.GetSequence();
			DG.Tweening.Sequence.Setup(sequence);
			return sequence;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002953 File Offset: 0x00000B53
		public static int CompleteAll(bool withCallbacks = false)
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, false, (float)(withCallbacks ? 1 : 0), null, null);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002968 File Offset: 0x00000B68
		public static int Complete(object targetOrId, bool withCallbacks = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, false, (float)(withCallbacks ? 1 : 0), null, null);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002982 File Offset: 0x00000B82
		internal static int CompleteAndReturnKilledTot()
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, true, 0f, null, null);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002994 File Offset: 0x00000B94
		internal static int CompleteAndReturnKilledTot(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, true, 0f, null, null);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000029AB File Offset: 0x00000BAB
		internal static int CompleteAndReturnKilledTotExceptFor(params object[] excludeTargetsOrIds)
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.AllExceptTargetsOrIds, null, true, 0f, null, excludeTargetsOrIds);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000029BD File Offset: 0x00000BBD
		public static int FlipAll()
		{
			return TweenManager.FilteredOperation(OperationType.Flip, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000029CF File Offset: 0x00000BCF
		public static int Flip(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Flip, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000029E6 File Offset: 0x00000BE6
		public static int GotoAll(float to, bool andPlay = false)
		{
			return TweenManager.FilteredOperation(OperationType.Goto, FilterType.All, null, andPlay, to, null, null);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000029F4 File Offset: 0x00000BF4
		public static int Goto(object targetOrId, float to, bool andPlay = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Goto, FilterType.TargetOrId, targetOrId, andPlay, to, null, null);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A07 File Offset: 0x00000C07
		public static int KillAll(bool complete = false)
		{
			return (complete ? DOTween.CompleteAndReturnKilledTot() : 0) + TweenManager.DespawnAll();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A1A File Offset: 0x00000C1A
		public static int KillAll(bool complete, params object[] idsOrTargetsToExclude)
		{
			if (idsOrTargetsToExclude == null)
			{
				return (complete ? DOTween.CompleteAndReturnKilledTot() : 0) + TweenManager.DespawnAll();
			}
			return (complete ? DOTween.CompleteAndReturnKilledTotExceptFor(idsOrTargetsToExclude) : 0) + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.AllExceptTargetsOrIds, null, false, 0f, null, idsOrTargetsToExclude);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002A4E File Offset: 0x00000C4E
		public static int Kill(object targetOrId, bool complete = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return (complete ? DOTween.CompleteAndReturnKilledTot(targetOrId) : 0) + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002A72 File Offset: 0x00000C72
		public static int PauseAll()
		{
			return TweenManager.FilteredOperation(OperationType.Pause, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002A84 File Offset: 0x00000C84
		public static int Pause(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Pause, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002A9B File Offset: 0x00000C9B
		public static int PlayAll()
		{
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002AAD File Offset: 0x00000CAD
		public static int Play(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002AC4 File Offset: 0x00000CC4
		public static int Play(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002ADE File Offset: 0x00000CDE
		public static int PlayBackwardsAll()
		{
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public static int PlayBackwards(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002B07 File Offset: 0x00000D07
		public static int PlayBackwards(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002B21 File Offset: 0x00000D21
		public static int PlayForwardAll()
		{
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002B33 File Offset: 0x00000D33
		public static int PlayForward(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002B4A File Offset: 0x00000D4A
		public static int PlayForward(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetAndId, id, false, 0f, target, null);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002B64 File Offset: 0x00000D64
		public static int RestartAll(bool includeDelay = true)
		{
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.All, null, includeDelay, 0f, null, null);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002B77 File Offset: 0x00000D77
		public static int Restart(object targetOrId, bool includeDelay = true, float changeDelayTo = -1f)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetOrId, targetOrId, includeDelay, changeDelayTo, null, null);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B8B File Offset: 0x00000D8B
		public static int Restart(object target, object id, bool includeDelay = true, float changeDelayTo = -1f)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetAndId, id, includeDelay, changeDelayTo, target, null);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002BA2 File Offset: 0x00000DA2
		public static int RewindAll(bool includeDelay = true)
		{
			return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.All, null, includeDelay, 0f, null, null);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public static int Rewind(object targetOrId, bool includeDelay = true)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.TargetOrId, targetOrId, includeDelay, 0f, null, null);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002BCB File Offset: 0x00000DCB
		public static int SmoothRewindAll()
		{
			return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002BDE File Offset: 0x00000DDE
		public static int SmoothRewind(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002BF6 File Offset: 0x00000DF6
		public static int TogglePauseAll()
		{
			return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.All, null, false, 0f, null, null);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002C09 File Offset: 0x00000E09
		public static int TogglePause(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.TargetOrId, targetOrId, false, 0f, null, null);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002C21 File Offset: 0x00000E21
		public static bool IsTweening(object targetOrId, bool alsoCheckIfIsPlaying = false)
		{
			return TweenManager.FilteredOperation(OperationType.IsTweening, FilterType.TargetOrId, targetOrId, alsoCheckIfIsPlaying, 0f, null, null) > 0;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002C37 File Offset: 0x00000E37
		public static int TotalPlayingTweens()
		{
			return TweenManager.TotalPlayingTweens();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002C3E File Offset: 0x00000E3E
		public static List<Tween> PlayingTweens(List<Tween> fillableList = null)
		{
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetActiveTweens(true, fillableList);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002C50 File Offset: 0x00000E50
		public static List<Tween> PausedTweens(List<Tween> fillableList = null)
		{
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetActiveTweens(false, fillableList);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002C62 File Offset: 0x00000E62
		public static List<Tween> TweensById(object id, bool playingOnly = false, List<Tween> fillableList = null)
		{
			if (id == null)
			{
				return null;
			}
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetTweensById(id, playingOnly, fillableList);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002C7A File Offset: 0x00000E7A
		public static List<Tween> TweensByTarget(object target, bool playingOnly = false, List<Tween> fillableList = null)
		{
			if (fillableList != null)
			{
				fillableList.Clear();
			}
			return TweenManager.GetTweensByTarget(target, playingOnly, fillableList);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002C8D File Offset: 0x00000E8D
		private static void InitCheck()
		{
			if (DOTween.initialized || !Application.isPlaying || DOTween.isQuitting)
			{
				return;
			}
			DOTween.AutoInit();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002CAC File Offset: 0x00000EAC
		private static TweenerCore<T1, T2, TPlugOptions> ApplyTo<T1, T2, TPlugOptions>(DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct, IPlugOptions
		{
			DOTween.InitCheck();
			TweenerCore<T1, T2, TPlugOptions> tweener = TweenManager.GetTweener<T1, T2, TPlugOptions>();
			if (!Tweener.Setup<T1, T2, TPlugOptions>(tweener, getter, setter, endValue, duration, plugin))
			{
				TweenManager.Despawn(tweener, true);
				return null;
			}
			return tweener;
		}

		// Token: 0x0400000E RID: 14
		public static readonly string Version = "1.2.135";

		// Token: 0x0400000F RID: 15
		public static bool useSafeMode = true;

		// Token: 0x04000010 RID: 16
		public static bool showUnityEditorReport = false;

		// Token: 0x04000011 RID: 17
		public static float timeScale = 1f;

		// Token: 0x04000012 RID: 18
		public static bool useSmoothDeltaTime;

		// Token: 0x04000013 RID: 19
		public static float maxSmoothUnscaledTime = 0.15f;

		// Token: 0x04000014 RID: 20
		internal static RewindCallbackMode rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;

		// Token: 0x04000015 RID: 21
		private static LogBehaviour _logBehaviour = LogBehaviour.ErrorsOnly;

		// Token: 0x04000016 RID: 22
		public static bool drawGizmos = true;

		// Token: 0x04000017 RID: 23
		public static UpdateType defaultUpdateType = UpdateType.Normal;

		// Token: 0x04000018 RID: 24
		public static bool defaultTimeScaleIndependent = false;

		// Token: 0x04000019 RID: 25
		public static AutoPlay defaultAutoPlay = AutoPlay.All;

		// Token: 0x0400001A RID: 26
		public static bool defaultAutoKill = true;

		// Token: 0x0400001B RID: 27
		public static LoopType defaultLoopType = LoopType.Restart;

		// Token: 0x0400001C RID: 28
		public static bool defaultRecyclable;

		// Token: 0x0400001D RID: 29
		public static Ease defaultEaseType = Ease.OutQuad;

		// Token: 0x0400001E RID: 30
		public static float defaultEaseOvershootOrAmplitude = 1.70158f;

		// Token: 0x0400001F RID: 31
		public static float defaultEasePeriod = 0f;

		// Token: 0x04000020 RID: 32
		public static DOTweenComponent instance;

		// Token: 0x04000021 RID: 33
		internal static int maxActiveTweenersReached;

		// Token: 0x04000022 RID: 34
		internal static int maxActiveSequencesReached;

		// Token: 0x04000023 RID: 35
		internal static readonly List<TweenCallback> GizmosDelegates = new List<TweenCallback>();

		// Token: 0x04000024 RID: 36
		internal static bool initialized;

		// Token: 0x04000025 RID: 37
		internal static bool isQuitting;
	}
}
