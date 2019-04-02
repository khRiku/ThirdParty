using System;

namespace DG.DOTweenEditor
{
	// Token: 0x02000008 RID: 8
	internal static class DOTweenDefines
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002EE4 File Offset: 0x000010E4
		public static void RemoveAllDefines()
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002EE8 File Offset: 0x000010E8
		public static void RemoveAllLegacyDefines()
		{
			EditorUtils.RemoveGlobalDefine("DOTAUDIO");
			EditorUtils.RemoveGlobalDefine("DOTPHYSICS");
			EditorUtils.RemoveGlobalDefine("DOTPHYSICS2D");
			EditorUtils.RemoveGlobalDefine("DOTSPRITE");
			EditorUtils.RemoveGlobalDefine("DOTUI");
			EditorUtils.RemoveGlobalDefine("DOTWEEN_NORBODY");
			EditorUtils.RemoveGlobalDefine("DOTWEEN_TK2D");
			EditorUtils.RemoveGlobalDefine("DOTWEEN_TMP");
		}

		// Token: 0x04000016 RID: 22
		public const string GlobalDefine_Legacy_AudioModule = "DOTAUDIO";

		// Token: 0x04000017 RID: 23
		public const string GlobalDefine_Legacy_PhysicsModule = "DOTPHYSICS";

		// Token: 0x04000018 RID: 24
		public const string GlobalDefine_Legacy_Physics2DModule = "DOTPHYSICS2D";

		// Token: 0x04000019 RID: 25
		public const string GlobalDefine_Legacy_SpriteModule = "DOTSPRITE";

		// Token: 0x0400001A RID: 26
		public const string GlobalDefine_Legacy_UIModule = "DOTUI";

		// Token: 0x0400001B RID: 27
		public const string GlobalDefine_Legacy_TK2D = "DOTWEEN_TK2D";

		// Token: 0x0400001C RID: 28
		public const string GlobalDefine_Legacy_TextMeshPro = "DOTWEEN_TMP";

		// Token: 0x0400001D RID: 29
		public const string GlobalDefine_Legacy_NoRigidbody = "DOTWEEN_NORBODY";
	}
}
