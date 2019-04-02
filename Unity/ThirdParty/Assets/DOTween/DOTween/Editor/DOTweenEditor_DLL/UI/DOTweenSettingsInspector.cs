using System;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
	// Token: 0x0200000C RID: 12
	[CustomEditor(typeof(DOTweenSettings))]
	public class DOTweenSettingsInspector : Editor
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00004AC0 File Offset: 0x00002CC0
		private void OnEnable()
		{
			this._src = (base.target as DOTweenSettings);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004AD3 File Offset: 0x00002CD3
		public override void OnInspectorGUI()
		{
			GUI.enabled = false;
			base.DrawDefaultInspector();
			GUI.enabled = true;
		}

		// Token: 0x04000048 RID: 72
		private DOTweenSettings _src;
	}
}
