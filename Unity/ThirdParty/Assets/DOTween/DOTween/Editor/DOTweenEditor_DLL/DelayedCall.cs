using System;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
	// Token: 0x02000002 RID: 2
	public class DelayedCall
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public DelayedCall(float delay, Action callback)
		{
			this.delay = delay;
			this.callback = callback;
			this._startupTime = Time.realtimeSinceStartup;
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000209C File Offset: 0x0000029C
		private void Update()
		{
			if (Time.realtimeSinceStartup - this._startupTime >= this.delay)
			{
				if (EditorApplication.update != null)
				{
					EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
				}
				if (this.callback != null)
				{
					this.callback();
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public float delay;

		// Token: 0x04000002 RID: 2
		public Action callback;

		// Token: 0x04000003 RID: 3
		private float _startupTime;
	}
}
