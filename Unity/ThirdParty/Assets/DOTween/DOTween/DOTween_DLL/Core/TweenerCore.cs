using System;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Core
{
	// Token: 0x02000051 RID: 81
	public class TweenerCore<T1, T2, TPlugOptions> : Tweener where TPlugOptions : struct, IPlugOptions
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000F374 File Offset: 0x0000D574
		internal TweenerCore()
		{
			this.typeofT1 = typeof(T1);
			this.typeofT2 = typeof(T2);
			this.typeofTPlugOptions = typeof(TPlugOptions);
			this.tweenType = TweenType.Tweener;
			this.Reset();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		public override Tweener ChangeStartValue(object newStartValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("You cannot change the values of a tween contained inside a Sequence");
				}
				return this;
			}
			Type type = newStartValue.GetType();
			if (type != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeStartValue: incorrect newStartValue type (is ",
						type,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			return Tweener.DoChangeStartValue<T1, T2, TPlugOptions>(this, (T2)((object)newStartValue), newDuration);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000F449 File Offset: 0x0000D649
		public override Tweener ChangeEndValue(object newEndValue, bool snapStartValue)
		{
			return this.ChangeEndValue(newEndValue, -1f, snapStartValue);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000F458 File Offset: 0x0000D658
		public override Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false)
		{
			if (this.isSequenced)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("You cannot change the values of a tween contained inside a Sequence");
				}
				return this;
			}
			Type type = newEndValue.GetType();
			if (type != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeEndValue: incorrect newEndValue type (is ",
						type,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			return Tweener.DoChangeEndValue<T1, T2, TPlugOptions>(this, (T2)((object)newEndValue), newDuration, snapStartValue);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000F4E0 File Offset: 0x0000D6E0
		public override Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("You cannot change the values of a tween contained inside a Sequence");
				}
				return this;
			}
			Type type = newStartValue.GetType();
			Type type2 = newEndValue.GetType();
			if (type != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeValues: incorrect value type (is ",
						type,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			if (type2 != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeValues: incorrect value type (is ",
						type2,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			return Tweener.DoChangeValues<T1, T2, TPlugOptions>(this, (T2)((object)newStartValue), (T2)((object)newEndValue), newDuration);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000F5BA File Offset: 0x0000D7BA
		internal override Tweener SetFrom(bool relative)
		{
			this.tweenPlugin.SetFrom(this, relative);
			this.hasManuallySetStartValue = true;
			return this;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000F5D4 File Offset: 0x0000D7D4
		internal sealed override void Reset()
		{
			base.Reset();
			if (this.tweenPlugin != null)
			{
				this.tweenPlugin.Reset(this);
			}
			this.plugOptions.Reset();
			this.getter = null;
			this.setter = null;
			this.hasManuallySetStartValue = false;
			this.isFromAllowed = true;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000F628 File Offset: 0x0000D828
		internal override bool Validate()
		{
			try
			{
				this.getter();
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000F65C File Offset: 0x0000D85C
		internal override float UpdateDelay(float elapsed)
		{
			return Tweener.DoUpdateDelay<T1, T2, TPlugOptions>(this, elapsed);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F665 File Offset: 0x0000D865
		internal override bool Startup()
		{
			return Tweener.DoStartup<T1, T2, TPlugOptions>(this);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000F670 File Offset: 0x0000D870
		internal override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
		{
			float elapsed = useInversePosition ? (this.duration - base.position) : base.position;
			if (DOTween.useSafeMode)
			{
				try
				{
					this.tweenPlugin.EvaluateAndApply(this.plugOptions, this, base.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
					return false;
				}
				catch
				{
					return true;
				}
			}
			this.tweenPlugin.EvaluateAndApply(this.plugOptions, this, base.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
			return false;
		}

		// Token: 0x0400015F RID: 351
		public T2 startValue;

		// Token: 0x04000160 RID: 352
		public T2 endValue;

		// Token: 0x04000161 RID: 353
		public T2 changeValue;

		// Token: 0x04000162 RID: 354
		public TPlugOptions plugOptions;

		// Token: 0x04000163 RID: 355
		public DOGetter<T1> getter;

		// Token: 0x04000164 RID: 356
		public DOSetter<T1> setter;

		// Token: 0x04000165 RID: 357
		internal ABSTweenPlugin<T1, T2, TPlugOptions> tweenPlugin;

		// Token: 0x04000166 RID: 358
		private const string _TxtCantChangeSequencedValues = "You cannot change the values of a tween contained inside a Sequence";
	}
}
