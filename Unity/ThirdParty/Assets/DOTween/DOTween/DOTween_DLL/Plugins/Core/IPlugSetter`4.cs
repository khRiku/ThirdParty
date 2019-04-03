// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.IPlugSetter`4
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;

namespace DG.Tweening.Plugins.Core
{
  public interface IPlugSetter<T1, out T2, TPlugin, out TPlugOptions>
  {
    DOGetter<T1> Getter();

    DOSetter<T1> Setter();

    T2 EndValue();

    TPlugOptions GetOptions();
  }
}
