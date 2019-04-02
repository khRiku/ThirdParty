using System;

namespace DG.Tweening.Core
{
	// Token: 0x0200004E RID: 78
	internal class SequenceCallback : ABSSequentiable
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0000D83E File Offset: 0x0000BA3E
		public SequenceCallback(float sequencedPosition, TweenCallback callback)
		{
			this.tweenType = TweenType.Callback;
			this.sequencedPosition = sequencedPosition;
			this.onStart = callback;
		}
	}
}
