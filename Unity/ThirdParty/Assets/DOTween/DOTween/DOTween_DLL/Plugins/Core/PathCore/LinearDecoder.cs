// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.PathCore.LinearDecoder
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
  internal class LinearDecoder : ABSPathDecoder
  {
    internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
    {
      p.controlPoints = (ControlPoint[]) null;
      p.subdivisions = wps.Length * p.subdivisionsXSegment;
      this.SetTimeToLengthTables(p, p.subdivisions);
    }

    internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints)
    {
      if ((double) perc <= 0.0)
      {
        p.linearWPIndex = 1;
        return wps[0];
      }
      int index1 = 0;
      int index2 = 0;
      int length = p.timesTable.Length;
      for (int index3 = 1; index3 < length; ++index3)
      {
        if ((double) p.timesTable[index3] >= (double) perc)
        {
          index1 = index3 - 1;
          index2 = index3;
          break;
        }
      }
      float num1 = p.timesTable[index1];
      float num2 = perc - num1;
      float maxLength = p.length * num2;
      Vector3 wp1 = wps[index1];
      Vector3 wp2 = wps[index2];
      p.linearWPIndex = index2;
      return wp1 + Vector3.ClampMagnitude(wp2 - wp1, maxLength);
    }

    internal void SetTimeToLengthTables(Path p, int subdivisions)
    {
      float num1 = 0.0f;
      int length = p.wps.Length;
      float[] numArray1 = new float[length];
      Vector3 b = p.wps[0];
      for (int index = 0; index < length; ++index)
      {
        Vector3 wp = p.wps[index];
        float num2 = Vector3.Distance(wp, b);
        num1 += num2;
        b = wp;
        numArray1[index] = num2;
      }
      float[] numArray2 = new float[length];
      float num3 = 0.0f;
      for (int index = 1; index < length; ++index)
      {
        num3 += numArray1[index];
        numArray2[index] = num3 / num1;
      }
      p.length = num1;
      p.wpLengths = numArray1;
      p.timesTable = numArray2;
    }

    internal void SetWaypointsLengths(Path p, int subdivisions)
    {
    }
  }
}
