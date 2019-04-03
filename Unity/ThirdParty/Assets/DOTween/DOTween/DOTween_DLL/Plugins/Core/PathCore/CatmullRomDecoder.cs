// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.PathCore.CatmullRomDecoder
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
  internal class CatmullRomDecoder : ABSPathDecoder
  {
    internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
    {
      int length = wps.Length;
      if (p.controlPoints == null || p.controlPoints.Length != 2)
        p.controlPoints = new ControlPoint[2];
      if (isClosedPath)
      {
        p.controlPoints[0] = new ControlPoint(wps[length - 2], Vector3.zero);
        p.controlPoints[1] = new ControlPoint(wps[1], Vector3.zero);
      }
      else
      {
        p.controlPoints[0] = new ControlPoint(wps[1], Vector3.zero);
        Vector3 wp = wps[length - 1];
        Vector3 vector3 = wp - wps[length - 2];
        p.controlPoints[1] = new ControlPoint(wp + vector3, Vector3.zero);
      }
      p.subdivisions = length * p.subdivisionsXSegment;
      this.SetTimeToLengthTables(p, p.subdivisions);
      this.SetWaypointsLengths(p, p.subdivisionsXSegment);
    }

    internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints)
    {
      int num1 = wps.Length - 1;
      int num2 = (int) Math.Floor((double) perc * (double) num1);
      int index = num1 - 1;
      if (index > num2)
        index = num2;
      float num3 = perc * (float) num1 - (float) index;
      Vector3 vector3_1 = index == 0 ? controlPoints[0].a : wps[index - 1];
      Vector3 wp1 = wps[index];
      Vector3 wp2 = wps[index + 1];
      Vector3 vector3_2 = index + 2 > wps.Length - 1 ? controlPoints[1].a : wps[index + 2];
      return 0.5f * ((-vector3_1 + 3f * wp1 - 3f * wp2 + vector3_2) * (num3 * num3 * num3) + (2f * vector3_1 - 5f * wp1 + 4f * wp2 - vector3_2) * (num3 * num3) + (-vector3_1 + wp2) * num3 + 2f * wp1);
    }

    internal void SetTimeToLengthTables(Path p, int subdivisions)
    {
      float num1 = 0.0f;
      float num2 = 1f / (float) subdivisions;
      float[] numArray1 = new float[subdivisions];
      float[] numArray2 = new float[subdivisions];
      Vector3 b = this.GetPoint(0.0f, p.wps, p, p.controlPoints);
      for (int index = 1; index < subdivisions + 1; ++index)
      {
        float perc = num2 * (float) index;
        Vector3 point = this.GetPoint(perc, p.wps, p, p.controlPoints);
        num1 += Vector3.Distance(point, b);
        b = point;
        numArray1[index - 1] = perc;
        numArray2[index - 1] = num1;
      }
      p.length = num1;
      p.timesTable = numArray1;
      p.lengthsTable = numArray2;
    }

    internal void SetWaypointsLengths(Path p, int subdivisions)
    {
      int length = p.wps.Length;
      float[] numArray = new float[length];
      numArray[0] = 0.0f;
      ControlPoint[] controlPoints = new ControlPoint[2];
      Vector3[] wps = new Vector3[2];
      for (int index1 = 1; index1 < length; ++index1)
      {
        controlPoints[0].a = index1 == 1 ? p.controlPoints[0].a : p.wps[index1 - 2];
        wps[0] = p.wps[index1 - 1];
        wps[1] = p.wps[index1];
        controlPoints[1].a = index1 == length - 1 ? p.controlPoints[1].a : p.wps[index1 + 1];
        float num1 = 0.0f;
        float num2 = 1f / (float) subdivisions;
        Vector3 b = this.GetPoint(0.0f, wps, p, controlPoints);
        for (int index2 = 1; index2 < subdivisions + 1; ++index2)
        {
          Vector3 point = this.GetPoint(num2 * (float) index2, wps, p, controlPoints);
          num1 += Vector3.Distance(point, b);
          b = point;
        }
        numArray[index1] = num1;
      }
      p.wpLengths = numArray;
    }
  }
}
