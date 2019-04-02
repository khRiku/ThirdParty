using System;
using DG.DemiEditor;
using UnityEngine;

namespace DG.DOTweenEditor.Core
{
	// Token: 0x02000009 RID: 9
	public class StylePalette : DeStylePalette
	{
		// Token: 0x04000029 RID: 41
		public readonly StylePalette.Custom custom = new StylePalette.Custom();

		// Token: 0x0200000C RID: 12
		public class Custom : DeStyleSubPalette
		{
			// Token: 0x06000029 RID: 41 RVA: 0x00004350 File Offset: 0x00002550
			public override void Init()
			{
				this.stickyToolbar = new GUIStyle(DeGUI.styles.toolbar.flat);
				this.stickyTitle = GUIStyleExtensions.ContentOffsetX(GUIStyleExtensions.MarginBottom(GUIStyleExtensions.Clone(new GUIStyle(GUI.skin.label), new object[]
				{
					1,
					11
				}), 0), -2f);
				this.warningLabel = GUIStyleExtensions.Background(GUIStyleExtensions.Add(new GUIStyle(GUI.skin.label), new object[]
				{
					Color.black,
					0
				}), DeStylePalette.orangeSquare, null);
			}

			// Token: 0x0400002D RID: 45
			public GUIStyle stickyToolbar;

			// Token: 0x0400002E RID: 46
			public GUIStyle stickyTitle;

			// Token: 0x0400002F RID: 47
			public GUIStyle warningLabel;
		}
	}
}
