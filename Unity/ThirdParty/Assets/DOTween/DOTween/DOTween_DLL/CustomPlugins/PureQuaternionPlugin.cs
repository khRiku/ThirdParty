// Decompiled with JetBrains decompiler
// Type: DG.Tweening.CustomPlugins.PureQuaternionPlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.CustomPlugins
{
  /// <summary>
  /// Straight Quaternion plugin. Instead of using Vector3 values accepts Quaternion values directly.
  /// <para>Beware: doesn't work with LoopType.Incremental (neither directly nor if inside a LoopType.Incremental Sequence).</para>
  /// <para>To use it, call DOTween.To with the plugin parameter overload, passing it <c>PureQuaternionPlugin.Plug()</c> as first parameter
  /// (do not use any of the other public PureQuaternionPlugin methods):</para>
  /// <code>DOTween.To(PureQuaternionPlugin.Plug(), ()=&gt; myQuaternionProperty, x=&gt; myQuaternionProperty = x, myQuaternionEndValue, duration);</code>
  /// </summary>
  public class PureQuaternionPlugin : ABSTweenPlugin<Quaternion, Quaternion, NoOptions>
  {
    private static PureQuaternionPlugin _plug;

    /// <summary>
    /// Plug this plugin inside a DOTween.To call.
    /// <para>Example:</para>
    /// <code>DOTween.To(PureQuaternionPlugin.Plug(), ()=&gt; myQuaternionProperty, x=&gt; myQuaternionProperty = x, myQuaternionEndValue, duration);</code>
    /// </summary>
    public static PureQuaternionPlugin Plug()
    {
      if (PureQuaternionPlugin._plug == null)
        PureQuaternionPlugin._plug = new PureQuaternionPlugin();
      return PureQuaternionPlugin._plug;
    }

    /// <summary>INTERNAL: do not use</summary>
    public override void Reset(TweenerCore<Quaternion, Quaternion, NoOptions> t)
    {
    }

    /// <summary>INTERNAL: do not use</summary>
    public override void SetFrom(TweenerCore<Quaternion, Quaternion, NoOptions> t, bool isRelative)
    {
      Quaternion endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = isRelative ? t.endValue * endValue : endValue;
      t.setter(t.startValue);
    }

    /// <summary>INTERNAL: do not use</summary>
    public override Quaternion ConvertToStartValue(TweenerCore<Quaternion, Quaternion, NoOptions> t, Quaternion value)
    {
      return value;
    }

    /// <summary>INTERNAL: do not use</summary>
    public override void SetRelativeEndValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
    {
      t.endValue *= t.startValue;
    }

    /// <summary>INTERNAL: do not use</summary>
    public override void SetChangeValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
    {
      t.changeValue.x = t.endValue.x - t.startValue.x;
      t.changeValue.y = t.endValue.y - t.startValue.y;
      t.changeValue.z = t.endValue.z - t.startValue.z;
      t.changeValue.w = t.endValue.w - t.startValue.w;
    }

    /// <summary>INTERNAL: do not use</summary>
    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Quaternion changeValue)
    {
      return changeValue.eulerAngles.magnitude / unitsXSecond;
    }

    /// <summary>INTERNAL: do not use</summary>
    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Quaternion startValue, Quaternion changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
      startValue.x += changeValue.x * num;
      startValue.y += changeValue.y * num;
      startValue.z += changeValue.z * num;
      startValue.w += changeValue.w * num;
      setter(startValue);
    }
  }
}
