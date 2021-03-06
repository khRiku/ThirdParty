﻿// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.Extensions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Core
{
  /// <summary>
  /// Public only so custom shortcuts can access some of these methods
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// INTERNAL: used by DO shortcuts and Modules to set special startup mode
    /// </summary>
    public static T SetSpecialStartupMode<T>(this T t, SpecialStartupMode mode) where T : Tween
    {
      t.specialStartupMode = mode;
      return t;
    }

    /// <summary>
    /// INTERNAL: used by DO shortcuts and Modules to set the tween as blendable
    /// </summary>
    public static TweenerCore<T1, T2, TPlugOptions> Blendable<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
    {
      t.isBlendable = true;
      return t;
    }

    /// <summary>
    /// INTERNAL: used by DO shortcuts and Modules to prevent a tween from using a From setup even if passed
    /// </summary>
    public static TweenerCore<T1, T2, TPlugOptions> NoFrom<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
    {
      t.isFromAllowed = false;
      return t;
    }
  }
}
