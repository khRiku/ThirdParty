// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.PathCore.Path
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
  [Serializable]
  public class Path
  {
    public int linearWPIndex = -1;
    public Color gizmoColor = new Color(1f, 1f, 1f, 0.7f);
    private static CatmullRomDecoder _catmullRomDecoder;
    private static LinearDecoder _linearDecoder;
    public float[] wpLengths;
    [SerializeField]
    public PathType type;
    [SerializeField]
    public int subdivisionsXSegment;
    [SerializeField]
    public int subdivisions;
    [SerializeField]
    public Vector3[] wps;
    [SerializeField]
    public ControlPoint[] controlPoints;
    [SerializeField]
    public float length;
    [SerializeField]
    public bool isFinalized;
    [SerializeField]
    public float[] timesTable;
    [SerializeField]
    public float[] lengthsTable;
    private Path _incrementalClone;
    private int _incrementalIndex;
    private ABSPathDecoder _decoder;
    private bool _changed;
    public Vector3[] nonLinearDrawWps;
    public Vector3 targetPosition;
    public Vector3? lookAtPosition;

    public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
    {
      this.type = type;
      this.subdivisionsXSegment = subdivisionsXSegment;
      if (gizmoColor.HasValue)
        this.gizmoColor = gizmoColor.Value;
      this.AssignWaypoints(waypoints, true);
      this.AssignDecoder(type);
      if (!TweenManager.isUnityEditor)
        return;
      DOTween.GizmosDelegates.Add(new TweenCallback(this.Draw));
    }

    public Path()
    {
    }

    public void FinalizePath(bool isClosedPath, AxisConstraint lockPositionAxes, Vector3 currTargetVal)
    {
      if (lockPositionAxes != AxisConstraint.None)
      {
        bool flag1 = (lockPositionAxes & AxisConstraint.X) == AxisConstraint.X;
        bool flag2 = (lockPositionAxes & AxisConstraint.Y) == AxisConstraint.Y;
        bool flag3 = (lockPositionAxes & AxisConstraint.Z) == AxisConstraint.Z;
        for (int index = 0; index < this.wps.Length; ++index)
        {
          Vector3 wp = this.wps[index];
          this.wps[index] = new Vector3(flag1 ? currTargetVal.x : wp.x, flag2 ? currTargetVal.y : wp.y, flag3 ? currTargetVal.z : wp.z);
        }
      }
      this._decoder.FinalizePath(this, this.wps, isClosedPath);
      this.isFinalized = true;
    }

    /// <summary>
    /// Gets the point on the path at the given percentage (0 to 1)
    /// </summary>
    /// <param name="perc">The percentage (0 to 1) at which to get the point</param>
    /// <param name="convertToConstantPerc">If TRUE constant speed is taken into account, otherwise not</param>
    public Vector3 GetPoint(float perc, bool convertToConstantPerc = false)
    {
      if (convertToConstantPerc)
        perc = this.ConvertToConstantPathPerc(perc);
      return this._decoder.GetPoint(perc, this.wps, this, this.controlPoints);
    }

    public float ConvertToConstantPathPerc(float perc)
    {
      if (this.type == PathType.Linear)
        return perc;
      if ((double) perc > 0.0 && (double) perc < 1.0)
      {
        float num1 = this.length * perc;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        float num5 = 0.0f;
        int length = this.lengthsTable.Length;
        for (int index = 0; index < length; ++index)
        {
          if ((double) this.lengthsTable[index] > (double) num1)
          {
            num4 = this.timesTable[index];
            num5 = this.lengthsTable[index];
            if (index > 0)
            {
              num3 = this.lengthsTable[index - 1];
              break;
            }
            break;
          }
          num2 = this.timesTable[index];
        }
        perc = num2 + (float) (((double) num1 - (double) num3) / ((double) num5 - (double) num3) * ((double) num4 - (double) num2));
      }
      if ((double) perc > 1.0)
        perc = 1f;
      else if ((double) perc < 0.0)
        perc = 0.0f;
      return perc;
    }

    public int GetWaypointIndexFromPerc(float perc, bool isMovingForward)
    {
      if ((double) perc >= 1.0)
        return this.wps.Length - 1;
      if ((double) perc <= 0.0)
        return 0;
      float num1 = this.length * perc;
      float num2 = 0.0f;
      int index = 0;
      for (int length = this.wpLengths.Length; index < length; ++index)
      {
        num2 += this.wpLengths[index];
        if (index == length - 1)
        {
          if (!isMovingForward)
            return index;
          return index - 1;
        }
        if ((double) num2 >= (double) num1)
        {
          if ((double) num2 > (double) num1 && isMovingForward)
            return index - 1;
          return index;
        }
      }
      return 0;
    }

    public static Vector3[] GetDrawPoints(Path p, int drawSubdivisionsXSegment)
    {
      int length = p.wps.Length;
      if (p.type == PathType.Linear)
        return p.wps;
      int num = length * drawSubdivisionsXSegment;
      Vector3[] vector3Array = new Vector3[num + 1];
      for (int index = 0; index <= num; ++index)
      {
        float perc = (float) index / (float) num;
        Vector3 point = p.GetPoint(perc, false);
        vector3Array[index] = point;
      }
      return vector3Array;
    }

    public static void RefreshNonLinearDrawWps(Path p)
    {
      int num = p.wps.Length * 10;
      if (p.nonLinearDrawWps == null || p.nonLinearDrawWps.Length != num + 1)
        p.nonLinearDrawWps = new Vector3[num + 1];
      for (int index = 0; index <= num; ++index)
      {
        float perc = (float) index / (float) num;
        Vector3 point = p.GetPoint(perc, false);
        p.nonLinearDrawWps[index] = point;
      }
    }

    public void Destroy()
    {
      if (TweenManager.isUnityEditor)
        DOTween.GizmosDelegates.Remove(new TweenCallback(this.Draw));
      this.wps = (Vector3[]) null;
      this.wpLengths = this.timesTable = this.lengthsTable = (float[]) null;
      this.nonLinearDrawWps = (Vector3[]) null;
      this.isFinalized = false;
    }

    public Path CloneIncremental(int loopIncrement)
    {
      if (this._incrementalClone != null)
      {
        if (this._incrementalIndex == loopIncrement)
          return this._incrementalClone;
        this._incrementalClone.Destroy();
      }
      int length1 = this.wps.Length;
      Vector3 vector3 = this.wps[length1 - 1] - this.wps[0];
      Vector3[] vector3Array1 = new Vector3[this.wps.Length];
      for (int index = 0; index < length1; ++index)
        vector3Array1[index] = this.wps[index] + vector3 * (float) loopIncrement;
      int length2 = this.controlPoints.Length;
      ControlPoint[] controlPointArray = new ControlPoint[length2];
      for (int index = 0; index < length2; ++index)
        controlPointArray[index] = this.controlPoints[index] + vector3 * (float) loopIncrement;
      Vector3[] vector3Array2 = (Vector3[]) null;
      if (this.nonLinearDrawWps != null)
      {
        int length3 = this.nonLinearDrawWps.Length;
        vector3Array2 = new Vector3[length3];
        for (int index = 0; index < length3; ++index)
          vector3Array2[index] = this.nonLinearDrawWps[index] + vector3 * (float) loopIncrement;
      }
      this._incrementalClone = new Path();
      this._incrementalIndex = loopIncrement;
      this._incrementalClone.type = this.type;
      this._incrementalClone.subdivisionsXSegment = this.subdivisionsXSegment;
      this._incrementalClone.subdivisions = this.subdivisions;
      this._incrementalClone.wps = vector3Array1;
      this._incrementalClone.controlPoints = controlPointArray;
      if (TweenManager.isUnityEditor)
        DOTween.GizmosDelegates.Add(new TweenCallback(this._incrementalClone.Draw));
      this._incrementalClone.length = this.length;
      this._incrementalClone.wpLengths = this.wpLengths;
      this._incrementalClone.timesTable = this.timesTable;
      this._incrementalClone.lengthsTable = this.lengthsTable;
      this._incrementalClone._decoder = this._decoder;
      this._incrementalClone.nonLinearDrawWps = vector3Array2;
      this._incrementalClone.targetPosition = this.targetPosition;
      this._incrementalClone.lookAtPosition = this.lookAtPosition;
      this._incrementalClone.isFinalized = true;
      return this._incrementalClone;
    }

    public void AssignWaypoints(Vector3[] newWps, bool cloneWps = false)
    {
      if (cloneWps)
      {
        int length = newWps.Length;
        this.wps = new Vector3[length];
        for (int index = 0; index < length; ++index)
          this.wps[index] = newWps[index];
      }
      else
        this.wps = newWps;
    }

    public void AssignDecoder(PathType pathType)
    {
      this.type = pathType;
      if (pathType == PathType.Linear)
      {
        if (Path._linearDecoder == null)
          Path._linearDecoder = new LinearDecoder();
        this._decoder = (ABSPathDecoder) Path._linearDecoder;
      }
      else
      {
        if (Path._catmullRomDecoder == null)
          Path._catmullRomDecoder = new CatmullRomDecoder();
        this._decoder = (ABSPathDecoder) Path._catmullRomDecoder;
      }
    }

    public void Draw()
    {
      Path.Draw(this);
    }

    private static void Draw(Path p)
    {
      if (p.timesTable == null)
        return;
      Color gizmoColor = p.gizmoColor;
      gizmoColor.a *= 0.5f;
      Gizmos.color = p.gizmoColor;
      int length1 = p.wps.Length;
      if (p._changed || p.type != PathType.Linear && p.nonLinearDrawWps == null)
      {
        p._changed = false;
        if (p.type != PathType.Linear)
          Path.RefreshNonLinearDrawWps(p);
      }
      if (p.type == PathType.Linear)
      {
        Vector3 to = p.wps[0];
        for (int index = 0; index < length1; ++index)
        {
          Vector3 wp = p.wps[index];
          Gizmos.DrawLine(wp, to);
          to = wp;
        }
      }
      else
      {
        Vector3 to = p.nonLinearDrawWps[0];
        int length2 = p.nonLinearDrawWps.Length;
        for (int index = 1; index < length2; ++index)
        {
          Vector3 nonLinearDrawWp = p.nonLinearDrawWps[index];
          Gizmos.DrawLine(nonLinearDrawWp, to);
          to = nonLinearDrawWp;
        }
      }
      Gizmos.color = gizmoColor;
      for (int index = 0; index < length1; ++index)
        Gizmos.DrawSphere(p.wps[index], 0.075f);
      if (!p.lookAtPosition.HasValue)
        return;
      Vector3 vector3 = p.lookAtPosition.Value;
      Gizmos.DrawLine(p.targetPosition, vector3);
      Gizmos.DrawWireSphere(vector3, 0.075f);
    }
  }
}
