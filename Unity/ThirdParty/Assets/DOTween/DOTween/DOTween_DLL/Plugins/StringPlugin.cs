// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.StringPlugin
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DG.Tweening.Plugins
{
  public class StringPlugin : ABSTweenPlugin<string, string, StringOptions>
  {
    private static readonly StringBuilder _Buffer = new StringBuilder();
    private static readonly List<char> _OpenedTags = new List<char>();

    public override void SetFrom(TweenerCore<string, string, StringOptions> t, bool isRelative)
    {
      string endValue = t.endValue;
      t.endValue = t.getter();
      t.startValue = endValue;
      t.setter(t.startValue);
    }

    public override void Reset(TweenerCore<string, string, StringOptions> t)
    {
      t.startValue = t.endValue = t.changeValue = (string) null;
    }

    public override string ConvertToStartValue(TweenerCore<string, string, StringOptions> t, string value)
    {
      return value;
    }

    public override void SetRelativeEndValue(TweenerCore<string, string, StringOptions> t)
    {
    }

    public override void SetChangeValue(TweenerCore<string, string, StringOptions> t)
    {
      t.changeValue = t.endValue;
      t.plugOptions.startValueStrippedLength = Regex.Replace(t.startValue, "<[^>]*>", "").Length;
      t.plugOptions.changeValueStrippedLength = Regex.Replace(t.changeValue, "<[^>]*>", "").Length;
    }

    public override float GetSpeedBasedDuration(StringOptions options, float unitsXSecond, string changeValue)
    {
      float num = (options.richTextEnabled ? (float) options.changeValueStrippedLength : (float) changeValue.Length) / unitsXSecond;
      if ((double) num < 0.0)
        num = -num;
      return num;
    }

    public override void EvaluateAndApply(StringOptions options, Tween t, bool isRelative, DOGetter<string> getter, DOSetter<string> setter, float elapsed, string startValue, string changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
      StringPlugin._Buffer.Remove(0, StringPlugin._Buffer.Length);
      if (isRelative && t.loopType == LoopType.Incremental)
      {
        int num = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
        if (num > 0)
        {
          StringPlugin._Buffer.Append(startValue);
          for (int index = 0; index < num; ++index)
            StringPlugin._Buffer.Append(changeValue);
          startValue = StringPlugin._Buffer.ToString();
          StringPlugin._Buffer.Remove(0, StringPlugin._Buffer.Length);
        }
      }
      int num1 = options.richTextEnabled ? options.startValueStrippedLength : startValue.Length;
      int num2 = options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length;
      int num3 = (int) Math.Round((double) num2 * (double) EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
      if (num3 > num2)
        num3 = num2;
      else if (num3 < 0)
        num3 = 0;
      if (isRelative)
      {
        StringPlugin._Buffer.Append(startValue);
        if (options.scrambleMode != ScrambleMode.None)
          setter(this.Append(changeValue, 0, num3, options.richTextEnabled).AppendScrambledChars(num2 - num3, this.ScrambledCharsToUse(options)).ToString());
        else
          setter(this.Append(changeValue, 0, num3, options.richTextEnabled).ToString());
      }
      else if (options.scrambleMode != ScrambleMode.None)
      {
        setter(this.Append(changeValue, 0, num3, options.richTextEnabled).AppendScrambledChars(num2 - num3, this.ScrambledCharsToUse(options)).ToString());
      }
      else
      {
        int num4 = num1 - num2;
        int num5 = num1;
        int num6 = 0;
        int num7;
        if (num4 > num6)
        {
          float num8 = (float) num3 / (float) num2;
          num7 = num5 - (int) ((double) num5 * (double) num8);
        }
        else
          num7 = num5 - num3;
        this.Append(changeValue, 0, num3, options.richTextEnabled);
        if (num3 < num2 && num3 < num1)
          this.Append(startValue, num3, options.richTextEnabled ? num3 + num7 : num7, options.richTextEnabled);
        setter(StringPlugin._Buffer.ToString());
      }
    }

    private StringBuilder Append(string value, int startIndex, int length, bool richTextEnabled)
    {
      if (!richTextEnabled)
      {
        StringPlugin._Buffer.Append(value, startIndex, length);
        return StringPlugin._Buffer;
      }
      StringPlugin._OpenedTags.Clear();
      bool flag1 = false;
      int length1 = value.Length;
      int startIndex1;
      for (startIndex1 = 0; startIndex1 < length; ++startIndex1)
      {
        char ch1 = value[startIndex1];
        if ((int) ch1 == 60)
        {
          bool flag2 = flag1;
          char ch2 = value[startIndex1 + 1];
          flag1 = startIndex1 >= length1 - 1 || (int) ch2 != 47;
          if (flag1)
            StringPlugin._OpenedTags.Add((int) ch2 == 35 ? 'c' : ch2);
          else
            StringPlugin._OpenedTags.RemoveAt(StringPlugin._OpenedTags.Count - 1);
          Match match = Regex.Match(value.Substring(startIndex1), "<.*?(>)");
          if (match.Success)
          {
            if (!flag1 && !flag2)
            {
              char ch3 = value[startIndex1 + 1];
              char[] array;
              if ((int) ch3 == 99)
                array = new char[2]{ '#', 'c' };
              else
                array = new char[1]{ ch3 };
              for (int startIndex2 = startIndex1 - 1; startIndex2 > -1; --startIndex2)
              {
                if ((int) value[startIndex2] == 60 && (int) value[startIndex2 + 1] != 47 && Array.IndexOf<char>(array, value[startIndex2 + 2]) != -1)
                {
                  StringPlugin._Buffer.Insert(0, value.Substring(startIndex2, value.IndexOf('>', startIndex2) + 1 - startIndex2));
                  break;
                }
              }
            }
            StringPlugin._Buffer.Append(match.Value);
            int num = match.Groups[1].Index + 1;
            length += num;
            startIndex += num;
            startIndex1 += num - 1;
          }
        }
        else if (startIndex1 >= startIndex)
          StringPlugin._Buffer.Append(ch1);
      }
      if (StringPlugin._OpenedTags.Count > 0 && startIndex1 < length1 - 1)
      {
        while (StringPlugin._OpenedTags.Count > 0 && startIndex1 < length1 - 1)
        {
          Match match = Regex.Match(value.Substring(startIndex1), "(</).*?>");
          if (match.Success)
          {
            if ((int) match.Value[2] == (int) StringPlugin._OpenedTags[StringPlugin._OpenedTags.Count - 1])
            {
              StringPlugin._Buffer.Append(match.Value);
              StringPlugin._OpenedTags.RemoveAt(StringPlugin._OpenedTags.Count - 1);
            }
            startIndex1 += match.Value.Length;
          }
          else
            break;
        }
      }
      return StringPlugin._Buffer;
    }

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
  }
}
