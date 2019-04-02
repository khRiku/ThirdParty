using System;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x0200004D RID: 77
	public static class DOTweenExternalCommand
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000274 RID: 628 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
		// (remove) Token: 0x06000275 RID: 629 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public static event Action<PathOptions, Tween, Quaternion, Transform> SetOrientationOnPath;

		// Token: 0x06000276 RID: 630 RVA: 0x0000D827 File Offset: 0x0000BA27
		internal static void Dispatch_SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
		{
			if (DOTweenExternalCommand.SetOrientationOnPath != null)
			{
				DOTweenExternalCommand.SetOrientationOnPath(options, t, newRot, trans);
			}
		}
	}
}
