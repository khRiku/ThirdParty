// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.ABSTweenPlugin`3
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins.Core
{
  public abstract class ABSTweenPlugin<T1, T2, TPlugOptions> : ITweenPlugin where TPlugOptions : struct, IPlugOptions
  {
    public abstract void Reset(TweenerCore<T1, T2, TPlugOptions> t);

    public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, bool isRelative);

    public abstract T2 ConvertToStartValue(TweenerCore<T1, T2, TPlugOptions> t, T1 value);

    public abstract void SetRelativeEndValue(TweenerCore<T1, T2, TPlugOptions> t);

    public abstract void SetChangeValue(TweenerCore<T1, T2, TPlugOptions> t);

    public abstract float GetSpeedBasedDuration(TPlugOptions options, float unitsXSecond, T2 changeValue);

    public abstract void EvaluateAndApply(TPlugOptions options, Tween t, bool isRelative, DOGetter<T1> getter, DOSetter<T1> setter, float elapsed, T2 startValue, T2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice);
  }
}
