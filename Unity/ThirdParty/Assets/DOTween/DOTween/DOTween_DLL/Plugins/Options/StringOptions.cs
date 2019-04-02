using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000038 RID: 56
	public struct StringOptions : IPlugOptions
	{
		// Token: 0x0600020E RID: 526 RVA: 0x0000BD5C File Offset: 0x00009F5C
		public void Reset()
		{
			this.richTextEnabled = false;
			this.scrambleMode = ScrambleMode.None;
			this.scrambledChars = null;
			this.startValueStrippedLength = (this.changeValueStrippedLength = 0);
		}

		// Token: 0x040000E7 RID: 231
		public bool richTextEnabled;

		// Token: 0x040000E8 RID: 232
		public ScrambleMode scrambleMode;

		// Token: 0x040000E9 RID: 233
		public char[] scrambledChars;

		// Token: 0x040000EA RID: 234
		internal int startValueStrippedLength;

		// Token: 0x040000EB RID: 235
		internal int changeValueStrippedLength;
	}
}
