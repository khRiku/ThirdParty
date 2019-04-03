// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.Utils
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System;
using System.Reflection;
using UnityEngine;

namespace DG.Tweening.Core
{
  public static class Utils
  {
    private static readonly string[] _defAssembliesToQuery = new string[2]
    {
      "Assembly-CSharp",
      "Assembly-CSharp-firstpass"
    };
    private static Assembly[] _loadedAssemblies;

    /// <summary>Returns a Vector3 with z = 0</summary>
    public static Vector3 Vector3FromAngle(float degrees, float magnitude)
    {
      float f = degrees * ((float) Math.PI / 180f);
      return new Vector3(magnitude * Mathf.Cos(f), magnitude * Mathf.Sin(f), 0.0f);
    }

    /// <summary>Returns the 2D angle between two vectors</summary>
    public static float Angle2D(Vector3 from, Vector3 to)
    {
      Vector2 right = Vector2.right;
      to -= from;
      float num = Vector2.Angle(right, (Vector2) to);
      if ((double) Vector3.Cross((Vector3) right, to).z > 0.0)
        num = 360f - num;
      return num * -1f;
    }

    /// <summary>
    /// Uses approximate equality on each axis instead of Unity's Vector3 equality,
    /// because the latter fails (in some cases) when assigning a Vector3 to a transform.position and then checking it.
    /// </summary>
    public static bool Vector3AreApproximatelyEqual(Vector3 a, Vector3 b)
    {
      if (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y))
        return Mathf.Approximately(a.z, b.z);
      return false;
    }

    /// <summary>
    /// Looks for the type withing all possible project assembly names
    /// </summary>
    public static System.Type GetLooseScriptType(string typeName)
    {
      for (int index = 0; index < Utils._defAssembliesToQuery.Length; ++index)
      {
        System.Type type = System.Type.GetType(string.Format("{0}, {1}", (object) typeName, (object) Utils._defAssembliesToQuery[index]));
        if (type != null)
          return type;
      }
      if (Utils._loadedAssemblies == null)
        Utils._loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
      for (int index = 0; index < Utils._loadedAssemblies.Length; ++index)
      {
        System.Type type = System.Type.GetType(string.Format("{0}, {1}", (object) typeName, (object) Utils._loadedAssemblies[index].GetName()));
        if (type != null)
          return type;
      }
      return (System.Type) null;
    }
  }
}
