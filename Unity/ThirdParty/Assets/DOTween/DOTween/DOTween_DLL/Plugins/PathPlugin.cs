using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000021 RID: 33
	public class PathPlugin : ABSTweenPlugin<Vector3, Path, PathOptions>
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00008D34 File Offset: 0x00006F34
		public override void Reset(TweenerCore<Vector3, Path, PathOptions> t)
		{
			t.endValue.Destroy();
			t.startValue = (t.endValue = (t.changeValue = null));
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void SetFrom(TweenerCore<Vector3, Path, PathOptions> t, bool isRelative)
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008D65 File Offset: 0x00006F65
		public static ABSTweenPlugin<Vector3, Path, PathOptions> Get()
		{
			return PluginsManager.GetCustomPlugin<PathPlugin, Vector3, Path, PathOptions>();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008D6C File Offset: 0x00006F6C
		public override Path ConvertToStartValue(TweenerCore<Vector3, Path, PathOptions> t, Vector3 value)
		{
			return t.endValue;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008D74 File Offset: 0x00006F74
		public override void SetRelativeEndValue(TweenerCore<Vector3, Path, PathOptions> t)
		{
			if (t.endValue.isFinalized)
			{
				return;
			}
			Vector3 vector = t.getter();
			int num = t.endValue.wps.Length;
			for (int i = 0; i < num; i++)
			{
				t.endValue.wps[i] += vector;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008DD8 File Offset: 0x00006FD8
		public override void SetChangeValue(TweenerCore<Vector3, Path, PathOptions> t)
		{
			Transform transform = ((Component)t.target).transform;
			if (t.plugOptions.orientType == OrientType.ToPath && t.plugOptions.useLocalPosition)
			{
				t.plugOptions.parent = transform.parent;
			}
			if (t.endValue.isFinalized)
			{
				t.changeValue = t.endValue;
				return;
			}
			Vector3 vector = t.getter();
			Path endValue = t.endValue;
			int num = endValue.wps.Length;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			if (!Utils.Vector3AreApproximatelyEqual(endValue.wps[0], vector))
			{
				flag = true;
				num2++;
			}
			if (t.plugOptions.isClosedPath && endValue.wps[num - 1] != vector)
			{
				flag2 = true;
				num2++;
			}
			Vector3[] array = new Vector3[num + num2];
			int num3 = flag ? 1 : 0;
			if (flag)
			{
				array[0] = vector;
			}
			for (int i = 0; i < num; i++)
			{
				array[i + num3] = endValue.wps[i];
			}
			if (flag2)
			{
				array[array.Length - 1] = array[0];
			}
			endValue.wps = array;
			endValue.FinalizePath(t.plugOptions.isClosedPath, t.plugOptions.lockPositionAxis, vector);
			t.plugOptions.startupRot = transform.rotation;
			t.plugOptions.startupZRot = transform.eulerAngles.z;
			t.changeValue = t.endValue;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008F66 File Offset: 0x00007166
		public override float GetSpeedBasedDuration(PathOptions options, float unitsXSecond, Path changeValue)
		{
			return changeValue.length / unitsXSecond;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008F70 File Offset: 0x00007170
		public override void EvaluateAndApply(PathOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Path startValue, Path changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental && !options.isClosedPath)
			{
				int num = t.isComplete ? (t.completedLoops - 1) : t.completedLoops;
				if (num > 0)
				{
					changeValue = changeValue.CloneIncremental(num);
				}
			}
			float perc = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			float num2 = changeValue.ConvertToConstantPathPerc(perc);
			Vector3 point = changeValue.GetPoint(num2, false);
			changeValue.targetPosition = point;
			setter(point);
			if (options.mode != PathMode.Ignore && options.orientType != OrientType.None)
			{
				this.SetOrientation(options, t, changeValue, num2, point, updateNotice);
			}
			bool flag = !usingInversePosition;
			if (t.isBackwards)
			{
				flag = !flag;
			}
			int waypointIndexFromPerc = changeValue.GetWaypointIndexFromPerc(perc, flag);
			if (waypointIndexFromPerc != t.miscInt)
			{
				int miscInt = t.miscInt;
				t.miscInt = waypointIndexFromPerc;
				if (t.onWaypointChange != null)
				{
					if (waypointIndexFromPerc < miscInt)
					{
						for (int i = miscInt - 1; i > waypointIndexFromPerc - 1; i--)
						{
							Tween.OnTweenCallback<int>(t.onWaypointChange, i);
						}
						return;
					}
					for (int j = miscInt + 1; j < waypointIndexFromPerc + 1; j++)
					{
						Tween.OnTweenCallback<int>(t.onWaypointChange, j);
					}
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000090AC File Offset: 0x000072AC
		public void SetOrientation(PathOptions options, Tween t, Path path, float pathPerc, Vector3 tPos, UpdateNotice updateNotice)
		{
			Transform transform = ((Component)t.target).transform;
			Quaternion quaternion = Quaternion.identity;
			if (updateNotice == UpdateNotice.RewindStep)
			{
				transform.rotation = options.startupRot;
			}
			switch (options.orientType)
			{
			case OrientType.ToPath:
			{
				Vector3 vector;
				if (path.type == PathType.Linear && options.lookAhead <= 0.0001f)
				{
					vector = tPos + path.wps[path.linearWPIndex] - path.wps[path.linearWPIndex - 1];
				}
				else
				{
					float num = pathPerc + options.lookAhead;
					if (num > 1f)
					{
						num = (options.isClosedPath ? (num - 1f) : ((path.type == PathType.Linear) ? 1f : 1.00001f));
					}
					vector = path.GetPoint(num, false);
				}
				if (path.type == PathType.Linear)
				{
					Vector3 vector2 = path.wps[path.wps.Length - 1];
					if (vector == vector2)
					{
						vector = ((tPos == vector2) ? (vector2 + (vector2 - path.wps[path.wps.Length - 2])) : vector2);
					}
				}
				Vector3 vector3 = transform.up;
				if (options.useLocalPosition && options.parent != null)
				{
					vector = options.parent.TransformPoint(vector);
				}
				if (options.lockRotationAxis != AxisConstraint.None)
				{
					if ((options.lockRotationAxis & AxisConstraint.X) == AxisConstraint.X)
					{
						Vector3 vector4 = transform.InverseTransformPoint(vector);
						vector4.y = 0f;
						vector = transform.TransformPoint(vector4);
						vector3 = ((options.useLocalPosition && options.parent != null) ? options.parent.up : Vector3.up);
					}
					if ((options.lockRotationAxis & AxisConstraint.Y) == AxisConstraint.Y)
					{
						Vector3 vector5 = transform.InverseTransformPoint(vector);
						if (vector5.z < 0f)
						{
							vector5.z = -vector5.z;
						}
						vector5.x = 0f;
						vector = transform.TransformPoint(vector5);
					}
					if ((options.lockRotationAxis & AxisConstraint.Z) == AxisConstraint.Z)
					{
						if (options.useLocalPosition && options.parent != null)
						{
							vector3 = options.parent.TransformDirection(Vector3.up);
						}
						else
						{
							vector3 = transform.TransformDirection(Vector3.up);
						}
						vector3.z = options.startupZRot;
					}
				}
				if (options.mode == PathMode.Full3D)
				{
					Vector3 vector6 = vector - transform.position;
					if (vector6 == Vector3.zero)
					{
						vector6 = transform.forward;
					}
					quaternion = Quaternion.LookRotation(vector6, vector3);
				}
				else
				{
					float num2 = 0f;
					float num3 = Utils.Angle2D(transform.position, vector);
					if (num3 < 0f)
					{
						num3 = 360f + num3;
					}
					if (options.mode == PathMode.Sidescroller2D)
					{
						num2 = (float)((vector.x < transform.position.x) ? 180 : 0);
						if (num3 > 90f && num3 < 270f)
						{
							num3 = 180f - num3;
						}
					}
					quaternion = Quaternion.Euler(0f, num2, num3);
				}
				break;
			}
			case OrientType.LookAtTransform:
				if (options.lookAtTransform != null)
				{
					path.lookAtPosition = new Vector3?(options.lookAtTransform.position);
					quaternion = Quaternion.LookRotation(options.lookAtTransform.position - transform.position, transform.up);
				}
				break;
			case OrientType.LookAtPosition:
				path.lookAtPosition = new Vector3?(options.lookAtPosition);
				quaternion = Quaternion.LookRotation(options.lookAtPosition - transform.position, transform.up);
				break;
			}
			if (options.hasCustomForwardDirection)
			{
				quaternion *= options.forward;
			}
			DOTweenExternalCommand.Dispatch_SetOrientationOnPath(options, t, quaternion, transform);
		}

		// Token: 0x040000C0 RID: 192
		public const float MinLookAhead = 0.0001f;
	}
}
