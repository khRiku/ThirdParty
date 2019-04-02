using System;
using DG.Tweening.Core;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000002 RID: 2
	[AddComponentMenu("")]
	public class DOTweenVisualManager : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Awake()
		{
			this._animComponent = base.GetComponent<ABSAnimationComponent>();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205E File Offset: 0x0000025E
		private void Update()
		{
			if (!this._requiresRestartFromSpawnPoint || this._animComponent == null)
			{
				return;
			}
			this._requiresRestartFromSpawnPoint = false;
			this._animComponent.DORestart(true);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000208C File Offset: 0x0000028C
		private void OnEnable()
		{
			switch (this.onEnableBehaviour)
			{
			case OnEnableBehaviour.Play:
				if (this._animComponent != null)
				{
					this._animComponent.DOPlay();
					return;
				}
				break;
			case OnEnableBehaviour.Restart:
				if (this._animComponent != null)
				{
					this._animComponent.DORestart(false);
					return;
				}
				break;
			case OnEnableBehaviour.RestartFromSpawnPoint:
				this._requiresRestartFromSpawnPoint = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000002F4
		private void OnDisable()
		{
			this._requiresRestartFromSpawnPoint = false;
			switch (this.onDisableBehaviour)
			{
			case OnDisableBehaviour.Pause:
				if (this._animComponent != null)
				{
					this._animComponent.DOPause();
					return;
				}
				break;
			case OnDisableBehaviour.Rewind:
				if (this._animComponent != null)
				{
					this._animComponent.DORewind();
					return;
				}
				break;
			case OnDisableBehaviour.Kill:
				if (this._animComponent != null)
				{
					this._animComponent.DOKill();
					return;
				}
				break;
			case OnDisableBehaviour.KillAndComplete:
				if (this._animComponent != null)
				{
					this._animComponent.DOComplete();
					this._animComponent.DOKill();
					return;
				}
				break;
			case OnDisableBehaviour.DestroyGameObject:
				if (this._animComponent != null)
				{
					this._animComponent.DOKill();
				}
				UnityEngine.Object.Destroy(base.gameObject);
				break;
			default:
				return;
			}
		}

		// Token: 0x04000001 RID: 1
		public VisualManagerPreset preset;

		// Token: 0x04000002 RID: 2
		public OnEnableBehaviour onEnableBehaviour;

		// Token: 0x04000003 RID: 3
		public OnDisableBehaviour onDisableBehaviour;

		// Token: 0x04000004 RID: 4
		private bool _requiresRestartFromSpawnPoint;

		// Token: 0x04000005 RID: 5
		private ABSAnimationComponent _animComponent;
	}
}
