// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.PathPlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
  /// <summary>Path plugin works exclusively with Transforms</summary>
  public class PathPlugin : ABSTweenPlugin<Vector3, Path, PathOptions>
  {
    public const float MinLookAhead = 0.0001f;

    public override void Reset(TweenerCore<Vector3, Path, PathOptions> t)
    {
      t.endValue.Destroy();
      t.startValue = t.endValue = t.changeValue = (Path) null;
    }

    public override void SetFrom(TweenerCore<Vector3, Path, PathOptions> t, bool isRelative)
    {
    }

    public static ABSTweenPlugin<Vector3, Path, PathOptions> Get()
    {
      return PluginsManager.GetCustomPlugin<PathPlugin, Vector3, Path, PathOptions>();
    }

    public override Path ConvertToStartValue(TweenerCore<Vector3, Path, PathOptions> t, Vector3 value)
    {
      return t.endValue;
    }

    public override void SetRelativeEndValue(TweenerCore<Vector3, Path, PathOptions> t)
    {
      if (t.endValue.isFinalized)
        return;
      Vector3 vector3 = t.getter();
      int length = t.endValue.wps.Length;
      for (int index = 0; index < length; ++index)
        t.endValue.wps[index] += vector3;
    }

    public override void SetChangeValue(TweenerCore<Vector3, Path, PathOptions> t)
    {
      Transform transform = ((Component) t.target).transform;
      if (t.plugOptions.orientType == OrientType.ToPath && t.plugOptions.useLocalPosition)
        t.plugOptions.parent = transform.parent;
      if (t.endValue.isFinalized)
      {
        t.changeValue = t.endValue;
      }
      else
      {
        Vector3 vector3 = t.getter();
        Path endValue = t.endValue;
        int length = endValue.wps.Length;
        int num1 = 0;
        bool flag1 = false;
        bool flag2 = false;
        if (!Utils.Vector3AreApproximatelyEqual(endValue.wps[0], vector3))
        {
          flag1 = true;
          ++num1;
        }
        if (t.plugOptions.isClosedPath && endValue.wps[length - 1] != vector3)
        {
          flag2 = true;
          ++num1;
        }
        Vector3[] vector3Array = new Vector3[length + num1];
        int num2 = flag1 ? 1 : 0;
        if (flag1)
          vector3Array[0] = vector3;
        for (int index = 0; index < length; ++index)
          vector3Array[index + num2] = endValue.wps[index];
        if (flag2)
          vector3Array[vector3Array.Length - 1] = vector3Array[0];
        endValue.wps = vector3Array;
        endValue.FinalizePath(t.plugOptions.isClosedPath, t.plugOptions.lockPositionAxis, vector3);
        t.plugOptions.startupRot = transform.rotation;
        t.plugOptions.startupZRot = transform.eulerAngles.z;
        t.changeValue = t.endValue;
      }
    }

    public override float GetSpeedBasedDuration(PathOptions options, float unitsXSecond, Path changeValue)
    {
      return changeValue.length / unitsXSecond;
    }

    public override void EvaluateAndApply(PathOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Path startValue, Path changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      if (t.loopType == LoopType.Incremental && !options.isClosedPath)
      {
        int loopIncrement = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
        if (loopIncrement > 0)
          changeValue = changeValue.CloneIncremental(loopIncrement);
      }
      float perc = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      float constantPathPerc = changeValue.ConvertToConstantPathPerc(perc);
      Vector3 point = changeValue.GetPoint(constantPathPerc, false);
      changeValue.targetPosition = point;
      setter(point);
      if (options.mode != PathMode.Ignore && options.orientType != OrientType.None)
        this.SetOrientation(options, t, changeValue, constantPathPerc, point, updateNotice);
      bool isMovingForward = !usingInversePosition;
      if (t.isBackwards)
        isMovingForward = !isMovingForward;
      int waypointIndexFromPerc = changeValue.GetWaypointIndexFromPerc(perc, isMovingForward);
      if (waypointIndexFromPerc == t.miscInt)
        return;
      int miscInt = t.miscInt;
      t.miscInt = waypointIndexFromPerc;
      if (t.onWaypointChange == null)
        return;
      if (waypointIndexFromPerc < miscInt)
      {
        for (int index = miscInt - 1; index > waypointIndexFromPerc - 1; --index)
          Tween.OnTweenCallback<int>(t.onWaypointChange, index);
      }
      else
      {
        for (int index = miscInt + 1; index < waypointIndexFromPerc + 1; ++index)
          Tween.OnTweenCallback<int>(t.onWaypointChange, index);
      }
    }

    public void SetOrientation(PathOptions options, Tween t, Path path, float pathPerc, Vector3 tPos, UpdateNotice updateNotice)
    {
      Transform transform = ((Component) t.target).transform;
      Quaternion newRot = Quaternion.identity;
      if (updateNotice == UpdateNotice.RewindStep)
        transform.rotation = options.startupRot;
      switch (options.orientType)
      {
        case OrientType.ToPath:
          Vector3 vector3;
          if (path.type == PathType.Linear && (double) options.lookAhead <= 9.99999974737875E-05)
          {
            vector3 = tPos + path.wps[path.linearWPIndex] - path.wps[path.linearWPIndex - 1];
          }
          else
          {
            float perc = pathPerc + options.lookAhead;
            if ((double) perc > 1.0)
              perc = options.isClosedPath ? perc - 1f : (path.type == PathType.Linear ? 1f : 1.00001f);
            vector3 = path.GetPoint(perc, false);
          }
          if (path.type == PathType.Linear)
          {
            Vector3 wp = path.wps[path.wps.Length - 1];
            if (vector3 == wp)
              vector3 = tPos == wp ? wp + (wp - path.wps[path.wps.Length - 2]) : wp;
          }
          Vector3 upwards = transform.up;
          if (options.useLocalPosition && (Object) options.parent != (Object) null)
            vector3 = options.parent.TransformPoint(vector3);
          if (options.lockRotationAxis != AxisConstraint.None)
          {
            if ((options.lockRotationAxis & AxisConstraint.X) == AxisConstraint.X)
            {
              Vector3 position = transform.InverseTransformPoint(vector3);
              position.y = 0.0f;
              vector3 = transform.TransformPoint(position);
              upwards = !options.useLocalPosition || !((Object) options.parent != (Object) null) ? Vector3.up : options.parent.up;
            }
            if ((options.lockRotationAxis & AxisConstraint.Y) == AxisConstraint.Y)
            {
              Vector3 position = transform.InverseTransformPoint(vector3);
              if ((double) position.z < 0.0)
                position.z = -position.z;
              position.x = 0.0f;
              vector3 = transform.TransformPoint(position);
            }
            if ((options.lockRotationAxis & AxisConstraint.Z) == AxisConstraint.Z)
            {
              upwards = !options.useLocalPosition || !((Object) options.parent != (Object) null) ? transform.TransformDirection(Vector3.up) : options.parent.TransformDirection(Vector3.up);
              upwards.z = options.startupZRot;
            }
          }
          if (options.mode == PathMode.Full3D)
          {
            Vector3 forward = vector3 - transform.position;
            if (forward == Vector3.zero)
              forward = transform.forward;
            newRot = Quaternion.LookRotation(forward, upwards);
            break;
          }
          float y = 0.0f;
          float z = Utils.Angle2D(transform.position, vector3);
          if ((double) z < 0.0)
            z = 360f + z;
          if (options.mode == PathMode.Sidescroller2D)
          {
            y = (double) vector3.x < (double) transform.position.x ? 180f : 0.0f;
            if ((double) z > 90.0 && (double) z < 270.0)
              z = 180f - z;
          }
          newRot = Quaternion.Euler(0.0f, y, z);
          break;
        case OrientType.LookAtTransform:
          if ((Object) options.lookAtTransform != (Object) null)
          {
            path.lookAtPosition = new Vector3?(options.lookAtTransform.position);
            newRot = Quaternion.LookRotation(options.lookAtTransform.position - transform.position, transform.up);
            break;
          }
          break;
        case OrientType.LookAtPosition:
          path.lookAtPosition = new Vector3?(options.lookAtPosition);
          newRot = Quaternion.LookRotation(options.lookAtPosition - transform.position, transform.up);
          break;
      }
      if (options.hasCustomForwardDirection)
        newRot *= options.forward;
      DOTweenExternalCommand.Dispatch_SetOrientationOnPath(options, t, newRot, transform);
    }
  }
}
