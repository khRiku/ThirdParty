// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.ABSSequentiable
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening.Core
{
    public abstract class ABSSequentiable
    {
        public TweenType tweenType;
        public float sequencedPosition;
        public float sequencedEndPosition;

        /// <summary>Called the first time the tween is set in a playing state, after any eventual delay</summary>
        internal TweenCallback onStart;
    }
}
