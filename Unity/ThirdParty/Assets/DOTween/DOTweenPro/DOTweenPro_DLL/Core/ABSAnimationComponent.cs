// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.ABSAnimationComponent
// Assembly: DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0FFB13C2-E5DA-4737-82C8-2ACE533F01F7
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\DOTweenPro.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace DG.Tweening.Core
{
  [AddComponentMenu("")]
  public abstract class ABSAnimationComponent : MonoBehaviour
  {
    public UpdateType updateType;
    public bool isSpeedBased;
    public bool hasOnStart;
    public bool hasOnPlay;
    public bool hasOnUpdate;
    public bool hasOnStepComplete;
    public bool hasOnComplete;
    public bool hasOnTweenCreated;
    public bool hasOnRewind;
    public UnityEvent onStart;
    public UnityEvent onPlay;
    public UnityEvent onUpdate;
    public UnityEvent onStepComplete;
    public UnityEvent onComplete;
    public UnityEvent onTweenCreated;
    public UnityEvent onRewind;
    [NonSerialized]
    public Tween tween;

    public abstract void DOPlay();

    public abstract void DOPlayBackwards();

    public abstract void DOPlayForward();

    public abstract void DOPause();

    public abstract void DOTogglePause();

    public abstract void DORewind();

    /// <summary>Restarts the tween</summary>
    /// <param name="fromHere">If TRUE, re-evaluates the tween's start and end values from its current position.
    /// Set it to TRUE when spawning the same DOTweenPath in different positions (like when using a pooling system)</param>
    public abstract void DORestart(bool fromHere = false);

    public abstract void DOComplete();

    public abstract void DOKill();
  }
}
