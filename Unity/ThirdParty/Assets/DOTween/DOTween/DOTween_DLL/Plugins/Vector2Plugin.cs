using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000028 RID: 40
	public class Vector2Plugin : ABSTweenPlugin<Vector2, Vector2, VectorOptions>
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x000080A0 File Offset: 0x000062A0
		public override void Reset(TweenerCore<Vector2, Vector2, VectorOptions> t)
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A734 File Offset: 0x00008934
		public override void SetFrom(TweenerCore<Vector2, Vector2, VectorOptions> t, bool isRelative)
		{
			Vector2 endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			Vector2 vector = t.endValue;
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			if (axisConstraint != AxisConstraint.X)
			{
				if (axisConstraint != AxisConstraint.Y)
				{
					vector = t.startValue;
				}
				else
				{
					vector.y = t.startValue.y;
				}
			}
			else
			{
				vector.x = t.startValue.x;
			}
			if (t.plugOptions.snapping)
			{
				vector.x = (float)Math.Round((double)vector.x);
				vector.y = (float)Math.Round((double)vector.y);
			}
			t.setter(vector);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000817D File Offset: 0x0000637D
		public override Vector2 ConvertToStartValue(TweenerCore<Vector2, Vector2, VectorOptions> t, Vector2 value)
		{
			return value;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A7FE File Offset: 0x000089FE
		public override void SetRelativeEndValue(TweenerCore<Vector2, Vector2, VectorOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A818 File Offset: 0x00008A18
		public override void SetChangeValue(TweenerCore<Vector2, Vector2, VectorOptions> t)
		{
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			if (axisConstraint == AxisConstraint.X)
			{
				t.changeValue = new Vector2(t.endValue.x - t.startValue.x, 0f);
				return;
			}
			if (axisConstraint != AxisConstraint.Y)
			{
				t.changeValue = t.endValue - t.startValue;
				return;
			}
			t.changeValue = new Vector2(0f, t.endValue.y - t.startValue.y);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A8A2 File Offset: 0x00008AA2
		public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector2 changeValue)
		{
			return changeValue.magnitude / unitsXSecond;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000A8B0 File Offset: 0x00008AB0
		public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector2> getter, DOSetter<Vector2> setter, float elapsed, Vector2 startValue, Vector2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			AxisConstraint axisConstraint = options.axisConstraint;
			if (axisConstraint == AxisConstraint.X)
			{
				Vector2 vector = getter();
				vector.x = startValue.x + changeValue.x * num;
				if (options.snapping)
				{
					vector.x = (float)Math.Round((double)vector.x);
				}
				setter(vector);
				return;
			}
			if (axisConstraint != AxisConstraint.Y)
			{
				startValue.x += changeValue.x * num;
				startValue.y += changeValue.y * num;
				if (options.snapping)
				{
					startValue.x = (float)Math.Round((double)startValue.x);
					startValue.y = (float)Math.Round((double)startValue.y);
				}
				setter(startValue);
				return;
			}
			Vector2 vector2 = getter();
			vector2.y = startValue.y + changeValue.y * num;
			if (options.snapping)
			{
				vector2.y = (float)Math.Round((double)vector2.y);
			}
			setter(vector2);
		}
	}
}
