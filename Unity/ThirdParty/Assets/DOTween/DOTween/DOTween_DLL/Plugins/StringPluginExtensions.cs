using System;
using System.Text;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200002B RID: 43
	internal static class StringPluginExtensions
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x0000B590 File Offset: 0x00009790
		static StringPluginExtensions()
		{
			StringPluginExtensions.ScrambledCharsAll.ScrambleChars();
			StringPluginExtensions.ScrambledCharsUppercase.ScrambleChars();
			StringPluginExtensions.ScrambledCharsLowercase.ScrambleChars();
			StringPluginExtensions.ScrambledCharsNumerals.ScrambleChars();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000B624 File Offset: 0x00009824
		internal static void ScrambleChars(this char[] chars)
		{
			int num = chars.Length;
			for (int i = 0; i < num; i++)
			{
				char c = chars[i];
				int num2 = Random.Range(i, num);
				chars[i] = chars[num2];
				chars[num2] = c;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B658 File Offset: 0x00009858
		internal static StringBuilder AppendScrambledChars(this StringBuilder buffer, int length, char[] chars)
		{
			if (length <= 0)
			{
				return buffer;
			}
			int num = chars.Length;
			int num2;
			for (num2 = StringPluginExtensions._lastRndSeed; num2 == StringPluginExtensions._lastRndSeed; num2 = Random.Range(0, num))
			{
			}
			StringPluginExtensions._lastRndSeed = num2;
			for (int i = 0; i < length; i++)
			{
				if (num2 >= num)
				{
					num2 = 0;
				}
				buffer.Append(chars[num2]);
				num2++;
			}
			return buffer;
		}

		// Token: 0x040000C4 RID: 196
		public static readonly char[] ScrambledCharsAll = new char[]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'X',
			'Y',
			'Z',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'x',
			'y',
			'z',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'0'
		};

		// Token: 0x040000C5 RID: 197
		public static readonly char[] ScrambledCharsUppercase = new char[]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'X',
			'Y',
			'Z'
		};

		// Token: 0x040000C6 RID: 198
		public static readonly char[] ScrambledCharsLowercase = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'x',
			'y',
			'z'
		};

		// Token: 0x040000C7 RID: 199
		public static readonly char[] ScrambledCharsNumerals = new char[]
		{
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'0'
		};

		// Token: 0x040000C8 RID: 200
		private static int _lastRndSeed;
	}
}
