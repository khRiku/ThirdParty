// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.SpecialPluginsUtils
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
  internal static class SpecialPluginsUtils
  {
    internal static bool SetLookAt(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
    {
      Transform target = t.target as Transform;
      Vector3 forward = t.endValue - target.position;
      switch (t.plugOptions.axisConstraint)
      {
        case AxisConstraint.X:
          forward.x = 0.0f;
          break;
        case AxisConstraint.Y:
          forward.y = 0.0f;
          break;
        case AxisConstraint.Z:
          forward.z = 0.0f;
          break;
      }
      Vector3 eulerAngles = Quaternion.LookRotation(forward, t.plugOptions.up).eulerAngles;
      t.endValue = eulerAngles;
      return true;
    }

    internal static bool SetPunch(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
    {
      Vector3 vector3;
      try
      {
        vector3 = t.getter();
      }
      catch
      {
        return false;
      }
      t.isRelative = t.isSpeedBased = false;
      t.easeType = Ease.OutQuad;
      t.customEase = (EaseFunction) null;
      int length = t.endValue.Length;
      for (int index = 0; index < length; ++index)
        t.endValue[index] = t.endValue[index] + vector3;
      return true;
    }

    internal static bool SetShake(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
    {
      if (!SpecialPluginsUtils.SetPunch(t))
        return false;
      t.easeType = Ease.Linear;
      return true;
    }

    internal static bool SetCameraShakePosition(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
    {
      if (!SpecialPluginsUtils.SetShake(t))
        return false;
      Camera target = t.target as Camera;
      if ((Object) target == (Object) null)
        return false;
      Vector3 vector3_1 = t.getter();
      Transform transform = target.transform;
      int length = t.endValue.Length;
      for (int index = 0; index < length; ++index)
      {
        Vector3 vector3_2 = t.endValue[index];
        t.endValue[index] = transform.localRotation * (vector3_2 - vector3_1) + vector3_1;
      }
      return true;
    }
  }
}
