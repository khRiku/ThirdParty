// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.StringPluginExtensions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using System.Text;
using UnityEngine;

namespace DG.Tweening.Plugins
{
  internal static class StringPluginExtensions
  {
    public static readonly char[] ScrambledCharsAll = new char[60]
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
    public static readonly char[] ScrambledCharsUppercase = new char[25]
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
    public static readonly char[] ScrambledCharsLowercase = new char[25]
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
    public static readonly char[] ScrambledCharsNumerals = new char[10]
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
    private static int _lastRndSeed;

    static StringPluginExtensions()
    {
      StringPluginExtensions.ScrambledCharsAll.ScrambleChars();
      StringPluginExtensions.ScrambledCharsUppercase.ScrambleChars();
      StringPluginExtensions.ScrambledCharsLowercase.ScrambleChars();
      StringPluginExtensions.ScrambledCharsNumerals.ScrambleChars();
    }

    internal static void ScrambleChars(this char[] chars)
    {
      int length = chars.Length;
      for (int min = 0; min < length; ++min)
      {
        char ch = chars[min];
        int index = Random.Range(min, length);
        chars[min] = chars[index];
        chars[index] = ch;
      }
    }

    internal static StringBuilder AppendScrambledChars(this StringBuilder buffer, int length, char[] chars)
    {
      if (length <= 0)
        return buffer;
      int length1 = chars.Length;
      int index1 = StringPluginExtensions._lastRndSeed;
      while (index1 == StringPluginExtensions._lastRndSeed)
        index1 = Random.Range(0, length1);
      StringPluginExtensions._lastRndSeed = index1;
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (index1 >= length1)
          index1 = 0;
        buffer.Append(chars[index1]);
        ++index1;
      }
      return buffer;
    }
  }
}
