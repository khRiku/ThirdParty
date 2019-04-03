// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.SpiralOptions
// Assembly: DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0FFB13C2-E5DA-4737-82C8-2ACE533F01F7
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\DOTweenPro.dll

using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
  public struct SpiralOptions : IPlugOptions
  {
    public float depth;
    public float frequency;
    public float speed;
    public SpiralMode mode;
    public bool snapping;
    internal float unit;
    internal Quaternion axisQ;

    public void Reset()
    {
      this.depth = this.frequency = this.speed = 0.0f;
      this.mode = SpiralMode.Expand;
      this.snapping = false;
    }
  }
}
