using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x0200004A RID: 74
	[AddComponentMenu("")]
	public class DOTweenComponent : MonoBehaviour, IDOTweenInit
	{
		// Token: 0x0600025D RID: 605 RVA: 0x0000D390 File Offset: 0x0000B590
		private void Awake()
		{
			if (!(DOTween.instance == null))
			{
				Debugger.LogWarning("Duplicate DOTweenComponent instance found in scene: destroying it");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			DOTween.instance = this;
			this.inspectorUpdater = 0;
			this._unscaledTime = Time.realtimeSinceStartup;
			Type looseScriptType = Utils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils");
			if (looseScriptType == null)
			{
				Debugger.LogError("Couldn't load Modules system");
				return;
			}
			looseScriptType.GetMethod("Init", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D408 File Offset: 0x0000B608
		private void Start()
		{
			if (DOTween.instance != this)
			{
				this._duplicateToDestroy = true;
			    UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000D42C File Offset: 0x0000B62C
		private void Update()
		{
			this._unscaledDeltaTime = Time.realtimeSinceStartup - this._unscaledTime;
			if (DOTween.useSmoothDeltaTime && this._unscaledDeltaTime > DOTween.maxSmoothUnscaledTime)
			{
				this._unscaledDeltaTime = DOTween.maxSmoothUnscaledTime;
			}
			if (TweenManager.hasActiveDefaultTweens)
			{
				TweenManager.Update(UpdateType.Normal, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
			}
			this._unscaledTime = Time.realtimeSinceStartup;
			if (TweenManager.isUnityEditor)
			{
				this.inspectorUpdater++;
				if (DOTween.showUnityEditorReport && TweenManager.hasActiveTweens)
				{
					if (TweenManager.totActiveTweeners > DOTween.maxActiveTweenersReached)
					{
						DOTween.maxActiveTweenersReached = TweenManager.totActiveTweeners;
					}
					if (TweenManager.totActiveSequences > DOTween.maxActiveSequencesReached)
					{
						DOTween.maxActiveSequencesReached = TweenManager.totActiveSequences;
					}
				}
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000D4F6 File Offset: 0x0000B6F6
		private void LateUpdate()
		{
			if (TweenManager.hasActiveLateTweens)
			{
				TweenManager.Update(UpdateType.Late, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000D52C File Offset: 0x0000B72C
		private void FixedUpdate()
		{
			if (TweenManager.hasActiveFixedTweens && Time.timeScale > 0f)
			{
				TweenManager.Update(UpdateType.Fixed, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) / Time.timeScale * DOTween.timeScale);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000D58C File Offset: 0x0000B78C
		private void OnDrawGizmos()
		{
			if (!DOTween.drawGizmos || !TweenManager.isUnityEditor)
			{
				return;
			}
			int count = DOTween.GizmosDelegates.Count;
			if (count == 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				DOTween.GizmosDelegates[i]();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
		private void OnDestroy()
		{
			if (this._duplicateToDestroy)
			{
				return;
			}
			if (DOTween.showUnityEditorReport)
			{
				Debugger.LogReport(string.Concat(new object[]
				{
					"Max overall simultaneous active Tweeners/Sequences: ",
					DOTween.maxActiveTweenersReached,
					"/",
					DOTween.maxActiveSequencesReached
				}));
			}
			if (DOTween.instance == this)
			{
				DOTween.instance = null;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000D63E File Offset: 0x0000B83E
		public void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				this._pausedTime = Time.realtimeSinceStartup;
				return;
			}
			this._unscaledTime += Time.realtimeSinceStartup - this._pausedTime;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000D668 File Offset: 0x0000B868
		private void OnApplicationQuit()
		{
			DOTween.isQuitting = true;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000D670 File Offset: 0x0000B870
		public IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity)
		{
			TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
			return this;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000D67A File Offset: 0x0000B87A
		internal IEnumerator WaitForCompletion(Tween t)
		{
			while (t.active && !t.isComplete)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000D689 File Offset: 0x0000B889
		internal IEnumerator WaitForRewind(Tween t)
		{
			while (t.active && (!t.playedOnce || t.position * (float)(t.completedLoops + 1) > 0f))
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000D698 File Offset: 0x0000B898
		internal IEnumerator WaitForKill(Tween t)
		{
			while (t.active)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D6A7 File Offset: 0x0000B8A7
		internal IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
		{
			while (t.active && t.completedLoops < elapsedLoops)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D6BD File Offset: 0x0000B8BD
		internal IEnumerator WaitForPosition(Tween t, float position)
		{
			while (t.active && t.position * (float)(t.completedLoops + 1) < position)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D6D3 File Offset: 0x0000B8D3
		internal IEnumerator WaitForStart(Tween t)
		{
			while (t.active && !t.playedOnce)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D6E2 File Offset: 0x0000B8E2
		internal static void Create()
		{
			if (DOTween.instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[DOTween]");
		    UnityEngine.Object.DontDestroyOnLoad(gameObject);
			DOTween.instance = gameObject.AddComponent<DOTweenComponent>();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D70C File Offset: 0x0000B90C
		internal static void DestroyInstance()
		{
			if (DOTween.instance != null)
			{
			    UnityEngine.Object.Destroy(DOTween.instance.gameObject);
			}
			DOTween.instance = null;
		}

		// Token: 0x0400011E RID: 286
		public int inspectorUpdater;

		// Token: 0x0400011F RID: 287
		private float _unscaledTime;

		// Token: 0x04000120 RID: 288
		private float _unscaledDeltaTime;

		// Token: 0x04000121 RID: 289
		private float _pausedTime;

		// Token: 0x04000122 RID: 290
		private bool _duplicateToDestroy;
	}
}
