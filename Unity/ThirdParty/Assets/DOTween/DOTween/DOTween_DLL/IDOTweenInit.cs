// Decompiled with JetBrains decompiler
// Type: DG.Tweening.IDOTweenInit
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening
{
  /// <summary>Used to allow method chaining with DOTween.Init</summary>
  public interface IDOTweenInit
  {
    /// <summary>
    /// Directly sets the current max capacity of Tweeners and Sequences
    /// (meaning how many Tweeners and Sequences can be running at the same time),
    /// so that DOTween doesn't need to automatically increase them in case the max is reached
    /// (which might lead to hiccups when that happens).
    /// Sequences capacity must be less or equal to Tweeners capacity
    /// (if you pass a low Tweener capacity it will be automatically increased to match the Sequence's).
    /// Beware: use this method only when there are no tweens running.
    /// </summary>
    /// <param name="tweenersCapacity">Max Tweeners capacity.
    /// Default: 200</param>
    /// <param name="sequencesCapacity">Max Sequences capacity.
    /// Default: 50</param>
    IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity);
  }
}
