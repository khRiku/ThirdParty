// Decompiled with JetBrains decompiler
// Type: DG.Tweening.DOTweenVisualManager
// Assembly: DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0FFB13C2-E5DA-4737-82C8-2ACE533F01F7
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\DOTweenPro.dll

using DG.Tweening.Core;
using UnityEngine;

namespace DG.Tweening
{
  [AddComponentMenu("")]
  public class DOTweenVisualManager : MonoBehaviour
  {
    public VisualManagerPreset preset;
    public OnEnableBehaviour onEnableBehaviour;
    public OnDisableBehaviour onDisableBehaviour;
    private bool _requiresRestartFromSpawnPoint;
    private ABSAnimationComponent _animComponent;

    private void Awake()
    {
      this._animComponent = this.GetComponent<ABSAnimationComponent>();
    }

    private void Update()
    {
      if (!this._requiresRestartFromSpawnPoint || (Object) this._animComponent == (Object) null)
        return;
      this._requiresRestartFromSpawnPoint = false;
      this._animComponent.DORestart(true);
    }

    private void OnEnable()
    {
      switch (this.onEnableBehaviour)
      {
        case OnEnableBehaviour.Play:
          if (!((Object) this._animComponent != (Object) null))
            break;
          this._animComponent.DOPlay();
          break;
        case OnEnableBehaviour.Restart:
          if (!((Object) this._animComponent != (Object) null))
            break;
          this._animComponent.DORestart(false);
          break;
        case OnEnableBehaviour.RestartFromSpawnPoint:
          this._requiresRestartFromSpawnPoint = true;
          break;
      }
    }

    private void OnDisable()
    {
      this._requiresRestartFromSpawnPoint = false;
      switch (this.onDisableBehaviour)
      {
        case OnDisableBehaviour.Pause:
          if (!((Object) this._animComponent != (Object) null))
            break;
          this._animComponent.DOPause();
          break;
        case OnDisableBehaviour.Rewind:
          if (!((Object) this._animComponent != (Object) null))
            break;
          this._animComponent.DORewind();
          break;
        case OnDisableBehaviour.Kill:
          if (!((Object) this._animComponent != (Object) null))
            break;
          this._animComponent.DOKill();
          break;
        case OnDisableBehaviour.KillAndComplete:
          if (!((Object) this._animComponent != (Object) null))
            break;
          this._animComponent.DOComplete();
          this._animComponent.DOKill();
          break;
        case OnDisableBehaviour.DestroyGameObject:
          if ((Object) this._animComponent != (Object) null)
            this._animComponent.DOKill();
          Object.Destroy((Object) this.gameObject);
          break;
      }
    }
  }
}
