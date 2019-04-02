using System;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Core
{
	// Token: 0x0200004C RID: 76
	public static class Extensions
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000D79A File Offset: 0x0000B99A
		public static T SetSpecialStartupMode<T>(this T t, SpecialStartupMode mode) where T : Tween
		{
			t.specialStartupMode = mode;
			return t;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000D7A9 File Offset: 0x0000B9A9
		public static TweenerCore<T1, T2, TPlugOptions> Blendable<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			t.isBlendable = true;
			return t;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000D7B3 File Offset: 0x0000B9B3
		public static TweenerCore<T1, T2, TPlugOptions> NoFrom<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
		{
			t.isFromAllowed = false;
			return t;
		}
	}
}
