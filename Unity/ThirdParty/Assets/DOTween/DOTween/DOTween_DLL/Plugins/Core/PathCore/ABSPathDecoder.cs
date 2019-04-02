using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000041 RID: 65
	internal abstract class ABSPathDecoder
	{
		// Token: 0x06000225 RID: 549
		internal abstract void FinalizePath(Path p, Vector3[] wps, bool isClosedPath);

		// Token: 0x06000226 RID: 550
		internal abstract Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints);
	}
}
