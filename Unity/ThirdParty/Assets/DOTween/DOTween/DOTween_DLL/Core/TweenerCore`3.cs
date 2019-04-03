// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Core.TweenerCore`3
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;

namespace DG.Tweening.Core
{
  public class TweenerCore<T1, T2, TPlugOptions> : Tweener where TPlugOptions : struct, IPlugOptions
  {
    public T2 startValue;
    public T2 endValue;
    public T2 changeValue;
    public TPlugOptions plugOptions;
    public DOGetter<T1> getter;
    public DOSetter<T1> setter;
    public ABSTweenPlugin<T1, T2, TPlugOptions> tweenPlugin;
    private const string _TxtCantChangeSequencedValues = "You cannot change the values of a tween contained inside a Sequence";

    public TweenerCore()
    {
      this.typeofT1 = typeof (T1);
      this.typeofT2 = typeof (T2);
      this.typeofTPlugOptions = typeof (TPlugOptions);
      this.tweenType = TweenType.Tweener;
      this.Reset();
    }

    public override Tweener ChangeStartValue(object newStartValue, float newDuration = -1f)
    {
      if (this.isSequenced)
      {
        if (Debugger.logPriority >= 1)
          Debugger.LogWarning((object) "You cannot change the values of a tween contained inside a Sequence");
        return (Tweener) this;
      }
      Type type = newStartValue.GetType();
      if (type == this.typeofT2)
        return Tweener.DoChangeStartValue<T1, T2, TPlugOptions>(this, (T2) newStartValue, newDuration);
      if (Debugger.logPriority >= 1)
        Debugger.LogWarning((object) ("ChangeStartValue: incorrect newStartValue type (is " + (object) type + ", should be " + (object) this.typeofT2 + ")"));
      return (Tweener) this;
    }

    public override Tweener ChangeEndValue(object newEndValue, bool snapStartValue)
    {
      return this.ChangeEndValue(newEndValue, -1f, snapStartValue);
    }

    public override Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false)
    {
      if (this.isSequenced)
      {
        if (Debugger.logPriority >= 1)
          Debugger.LogWarning((object) "You cannot change the values of a tween contained inside a Sequence");
        return (Tweener) this;
      }
      Type type = newEndValue.GetType();
      if (type == this.typeofT2)
        return Tweener.DoChangeEndValue<T1, T2, TPlugOptions>(this, (T2) newEndValue, newDuration, snapStartValue);
      if (Debugger.logPriority >= 1)
        Debugger.LogWarning((object) ("ChangeEndValue: incorrect newEndValue type (is " + (object) type + ", should be " + (object) this.typeofT2 + ")"));
      return (Tweener) this;
    }

    public override Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f)
    {
      if (this.isSequenced)
      {
        if (Debugger.logPriority >= 1)
          Debugger.LogWarning((object) "You cannot change the values of a tween contained inside a Sequence");
        return (Tweener) this;
      }
      Type type1 = newStartValue.GetType();
      Type type2 = newEndValue.GetType();
      if (type1 != this.typeofT2)
      {
        if (Debugger.logPriority >= 1)
          Debugger.LogWarning((object) ("ChangeValues: incorrect value type (is " + (object) type1 + ", should be " + (object) this.typeofT2 + ")"));
        return (Tweener) this;
      }
      if (type2 == this.typeofT2)
        return Tweener.DoChangeValues<T1, T2, TPlugOptions>(this, (T2) newStartValue, (T2) newEndValue, newDuration);
      if (Debugger.logPriority >= 1)
        Debugger.LogWarning((object) ("ChangeValues: incorrect value type (is " + (object) type2 + ", should be " + (object) this.typeofT2 + ")"));
      return (Tweener) this;
    }

    public override Tweener SetFrom(bool relative)
    {
      this.tweenPlugin.SetFrom(this, relative);
      this.hasManuallySetStartValue = true;
      return (Tweener) this;
    }

    public override sealed void Reset()
    {
      base.Reset();
      if (this.tweenPlugin != null)
        this.tweenPlugin.Reset(this);
      this.plugOptions.Reset();
      this.getter = (DOGetter<T1>) null;
      this.setter = (DOSetter<T1>) null;
      this.hasManuallySetStartValue = false;
      this.isFromAllowed = true;
    }

    public override bool Validate()
    {
      try
      {
        T1 obj = this.getter();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public override float UpdateDelay(float elapsed)
    {
      return Tweener.DoUpdateDelay<T1, T2, TPlugOptions>(this, elapsed);
    }

    public override bool Startup()
    {
      return Tweener.DoStartup<T1, T2, TPlugOptions>(this);
    }

    public override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
    {
      float elapsed = useInversePosition ? this.duration - this.position : this.position;
      if (DOTween.useSafeMode)
      {
        try
        {
          this.tweenPlugin.EvaluateAndApply(this.plugOptions, (Tween) this, this.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
        }
        catch
        {
          return true;
        }
      }
      else
        this.tweenPlugin.EvaluateAndApply(this.plugOptions, (Tween) this, this.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
      return false;
    }
  }
}
