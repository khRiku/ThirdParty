// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Options.StringOptions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

namespace DG.Tweening.Plugins.Options
{
  public struct StringOptions : IPlugOptions
  {
    public bool richTextEnabled;
    public ScrambleMode scrambleMode;
    public char[] scrambledChars;
    internal int startValueStrippedLength;
    internal int changeValueStrippedLength;

    public void Reset()
    {
      this.richTextEnabled = false;
      this.scrambleMode = ScrambleMode.None;
      this.scrambledChars = (char[]) null;
      this.startValueStrippedLength = this.changeValueStrippedLength = 0;
    }
  }
}
