using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200002A RID: 42
	public class StringPlugin : ABSTweenPlugin<string, string, StringOptions>
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x0000AFB8 File Offset: 0x000091B8
		public override void SetFrom(TweenerCore<string, string, StringOptions> t, bool isRelative)
		{
			string endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = endValue;
			t.setter(t.startValue);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000AFF8 File Offset: 0x000091F8
		public override void Reset(TweenerCore<string, string, StringOptions> t)
		{
			t.startValue = (t.endValue = (t.changeValue = null));
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000817D File Offset: 0x0000637D
		public override string ConvertToStartValue(TweenerCore<string, string, StringOptions> t, string value)
		{
			return value;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void SetRelativeEndValue(TweenerCore<string, string, StringOptions> t)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B020 File Offset: 0x00009220
		public override void SetChangeValue(TweenerCore<string, string, StringOptions> t)
		{
			t.changeValue = t.endValue;
			t.plugOptions.startValueStrippedLength = Regex.Replace(t.startValue, "<[^>]*>", "").Length;
			t.plugOptions.changeValueStrippedLength = Regex.Replace(t.changeValue, "<[^>]*>", "").Length;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000B084 File Offset: 0x00009284
		public override float GetSpeedBasedDuration(StringOptions options, float unitsXSecond, string changeValue)
		{
			float num = (float)(options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length) / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000B0B8 File Offset: 0x000092B8
		public override void EvaluateAndApply(StringOptions options, Tween t, bool isRelative, DOGetter<string> getter, DOSetter<string> setter, float elapsed, string startValue, string changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			StringPlugin._Buffer.Remove(0, StringPlugin._Buffer.Length);
			if (isRelative && t.loopType == LoopType.Incremental)
			{
				int num = t.isComplete ? (t.completedLoops - 1) : t.completedLoops;
				if (num > 0)
				{
					StringPlugin._Buffer.Append(startValue);
					for (int i = 0; i < num; i++)
					{
						StringPlugin._Buffer.Append(changeValue);
					}
					startValue = StringPlugin._Buffer.ToString();
					StringPlugin._Buffer.Remove(0, StringPlugin._Buffer.Length);
				}
			}
			int num2 = options.richTextEnabled ? options.startValueStrippedLength : startValue.Length;
			int num3 = options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length;
			int num4 = (int)Math.Round((double)((float)num3 * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
			if (num4 > num3)
			{
				num4 = num3;
			}
			else if (num4 < 0)
			{
				num4 = 0;
			}
			if (isRelative)
			{
				StringPlugin._Buffer.Append(startValue);
				if (options.scrambleMode != ScrambleMode.None)
				{
					setter(this.Append(changeValue, 0, num4, options.richTextEnabled).AppendScrambledChars(num3 - num4, this.ScrambledCharsToUse(options)).ToString());
					return;
				}
				setter(this.Append(changeValue, 0, num4, options.richTextEnabled).ToString());
				return;
			}
			else
			{
				if (options.scrambleMode != ScrambleMode.None)
				{
					setter(this.Append(changeValue, 0, num4, options.richTextEnabled).AppendScrambledChars(num3 - num4, this.ScrambledCharsToUse(options)).ToString());
					return;
				}
				int num5 = num2 - num3;
				int num6 = num2;
				if (num5 > 0)
				{
					float num7 = (float)num4 / (float)num3;
					num6 -= (int)((float)num6 * num7);
				}
				else
				{
					num6 -= num4;
				}
				this.Append(changeValue, 0, num4, options.richTextEnabled);
				if (num4 < num3 && num4 < num2)
				{
					this.Append(startValue, num4, options.richTextEnabled ? (num4 + num6) : num6, options.richTextEnabled);
				}
				setter(StringPlugin._Buffer.ToString());
				return;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000B2B8 File Offset: 0x000094B8
		private StringBuilder Append(string value, int startIndex, int length, bool richTextEnabled)
		{
			if (!richTextEnabled)
			{
				StringPlugin._Buffer.Append(value, startIndex, length);
				return StringPlugin._Buffer;
			}
			StringPlugin._OpenedTags.Clear();
			bool flag = false;
			int length2 = value.Length;
			int i;
			for (i = 0; i < length; i++)
			{
				char c = value[i];
				if (c == '<')
				{
					bool flag2 = flag;
					char c2 = value[i + 1];
					flag = (i >= length2 - 1 || c2 != '/');
					if (flag)
					{
						StringPlugin._OpenedTags.Add((c2 == '#') ? 'c' : c2);
					}
					else
					{
						StringPlugin._OpenedTags.RemoveAt(StringPlugin._OpenedTags.Count - 1);
					}
					Match match = Regex.Match(value.Substring(i), "<.*?(>)");
					if (match.Success)
					{
						if (!flag && !flag2)
						{
							char c3 = value[i + 1];
							char[] array;
							if (c3 == 'c')
							{
								array = new char[]
								{
									'#',
									'c'
								};
							}
							else
							{
								array = new char[]
								{
									c3
								};
							}
							for (int j = i - 1; j > -1; j--)
							{
								if (value[j] == '<' && value[j + 1] != '/' && Array.IndexOf<char>(array, value[j + 2]) != -1)
								{
									StringPlugin._Buffer.Insert(0, value.Substring(j, value.IndexOf('>', j) + 1 - j));
									break;
								}
							}
						}
						StringPlugin._Buffer.Append(match.Value);
						int num = match.Groups[1].Index + 1;
						length += num;
						startIndex += num;
						i += num - 1;
					}
				}
				else if (i >= startIndex)
				{
					StringPlugin._Buffer.Append(c);
				}
			}
			if (StringPlugin._OpenedTags.Count > 0 && i < length2 - 1)
			{
				while (StringPlugin._OpenedTags.Count > 0 && i < length2 - 1)
				{
					Match match2 = Regex.Match(value.Substring(i), "(</).*?>");
					if (!match2.Success)
					{
						break;
					}
					if (match2.Value[2] == StringPlugin._OpenedTags[StringPlugin._OpenedTags.Count - 1])
					{
						StringPlugin._Buffer.Append(match2.Value);
						StringPlugin._OpenedTags.RemoveAt(StringPlugin._OpenedTags.Count - 1);
					}
					i += match2.Value.Length;
				}
			}
			return StringPlugin._Buffer;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000B524 File Offset: 0x00009724
		private char[] ScrambledCharsToUse(StringOptions options)
		{
			switch (options.scrambleMode)
			{
			case ScrambleMode.Uppercase:
				return StringPluginExtensions.ScrambledCharsUppercase;
			case ScrambleMode.Lowercase:
				return StringPluginExtensions.ScrambledCharsLowercase;
			case ScrambleMode.Numerals:
				return StringPluginExtensions.ScrambledCharsNumerals;
			case ScrambleMode.Custom:
				return options.scrambledChars;
			default:
				return StringPluginExtensions.ScrambledCharsAll;
			}
		}

		// Token: 0x040000C2 RID: 194
		private static readonly StringBuilder _Buffer = new StringBuilder();

		// Token: 0x040000C3 RID: 195
		private static readonly List<char> _OpenedTags = new List<char>();
	}
}
