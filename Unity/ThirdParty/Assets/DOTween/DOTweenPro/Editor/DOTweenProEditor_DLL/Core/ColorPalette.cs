using System;
using DG.DemiLib;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class ColorPalette : DeColorPalette
	{
		// Token: 0x04000028 RID: 40
		public ColorPalette.Custom custom = new ColorPalette.Custom();

		// Token: 0x0200000B RID: 11
		[Serializable]
		public class Custom
		{
			// Token: 0x0400002C RID: 44
			public DeSkinColor stickyDivider = new DeSkinColor(Color.black, new Color(0.5f, 0.5f, 0.5f));
		}
	}
}
