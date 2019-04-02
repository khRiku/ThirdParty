using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.CustomPlugins;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000014 RID: 20
	public static class ShortcutExtensions
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00004524 File Offset: 0x00002724
		public static Tweener DOAspect(this Camera target, float endValue, float duration)
		{
			return DOTween.To(() => target.aspect, delegate(float x)
			{
				target.aspect = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004568 File Offset: 0x00002768
		public static Tweener DOColor(this Camera target, Color endValue, float duration)
		{
			return DOTween.To(() => target.backgroundColor, delegate(Color x)
			{
				target.backgroundColor = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000045AC File Offset: 0x000027AC
		public static Tweener DOFarClipPlane(this Camera target, float endValue, float duration)
		{
			return DOTween.To(() => target.farClipPlane, delegate(float x)
			{
				target.farClipPlane = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000045F0 File Offset: 0x000027F0
		public static Tweener DOFieldOfView(this Camera target, float endValue, float duration)
		{
			return DOTween.To(() => target.fieldOfView, delegate(float x)
			{
				target.fieldOfView = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004634 File Offset: 0x00002834
		public static Tweener DONearClipPlane(this Camera target, float endValue, float duration)
		{
			return DOTween.To(() => target.nearClipPlane, delegate(float x)
			{
				target.nearClipPlane = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004678 File Offset: 0x00002878
		public static Tweener DOOrthoSize(this Camera target, float endValue, float duration)
		{
			return DOTween.To(() => target.orthographicSize, delegate(float x)
			{
				target.orthographicSize = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000046BC File Offset: 0x000028BC
		public static Tweener DOPixelRect(this Camera target, Rect endValue, float duration)
		{
			return DOTween.To(() => target.pixelRect, delegate(Rect x)
			{
				target.pixelRect = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004700 File Offset: 0x00002900
		public static Tweener DORect(this Camera target, Rect endValue, float duration)
		{
			return DOTween.To(() => target.rect, delegate(Rect x)
			{
				target.rect = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004744 File Offset: 0x00002944
		public static Tweener DOShakePosition(this Camera target, float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localPosition, delegate(Vector3 x)
			{
				target.transform.localPosition = x;
			}, duration, strength, vibrato, randomness, true, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetCameraShakePosition);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000047B0 File Offset: 0x000029B0
		public static Tweener DOShakePosition(this Camera target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localPosition, delegate(Vector3 x)
			{
				target.transform.localPosition = x;
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetCameraShakePosition);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000481C File Offset: 0x00002A1C
		public static Tweener DOShakeRotation(this Camera target, float duration, float strength = 90f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localEulerAngles, delegate(Vector3 x)
			{
				target.transform.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004888 File Offset: 0x00002A88
		public static Tweener DOShakeRotation(this Camera target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.transform.localEulerAngles, delegate(Vector3 x)
			{
				target.transform.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000048F4 File Offset: 0x00002AF4
		public static Tweener DOColor(this Light target, Color endValue, float duration)
		{
			return DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004938 File Offset: 0x00002B38
		public static Tweener DOIntensity(this Light target, float endValue, float duration)
		{
			return DOTween.To(() => target.intensity, delegate(float x)
			{
				target.intensity = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000497C File Offset: 0x00002B7C
		public static Tweener DOShadowStrength(this Light target, float endValue, float duration)
		{
			return DOTween.To(() => target.shadowStrength, delegate(float x)
			{
				target.shadowStrength = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000049C0 File Offset: 0x00002BC0
		public static Tweener DOColor(this LineRenderer target, Color2 startValue, Color2 endValue, float duration)
		{
			return DOTween.To(() => startValue, delegate(Color2 x)
			{
				target.SetColors(x.ca, x.cb);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004A0C File Offset: 0x00002C0C
		public static Tweener DOColor(this Material target, Color endValue, float duration)
		{
			return DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004A50 File Offset: 0x00002C50
		public static Tweener DOColor(this Material target, Color endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			return DOTween.To(() => target.GetColor(property), delegate(Color x)
			{
				target.SetColor(property, x);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004AC4 File Offset: 0x00002CC4
		public static Tweener DOFade(this Material target, float endValue, float duration)
		{
			return DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004B08 File Offset: 0x00002D08
		public static Tweener DOFade(this Material target, float endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			return DOTween.ToAlpha(() => target.GetColor(property), delegate(Color x)
			{
				target.SetColor(property, x);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004B7C File Offset: 0x00002D7C
		public static Tweener DOFloat(this Material target, float endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			return DOTween.To(() => target.GetFloat(property), delegate(float x)
			{
				target.SetFloat(property, x);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004BF0 File Offset: 0x00002DF0
		public static Tweener DOOffset(this Material target, Vector2 endValue, float duration)
		{
			return DOTween.To(() => target.mainTextureOffset, delegate(Vector2 x)
			{
				target.mainTextureOffset = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004C34 File Offset: 0x00002E34
		public static Tweener DOOffset(this Material target, Vector2 endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			return DOTween.To(() => target.GetTextureOffset(property), delegate(Vector2 x)
			{
				target.SetTextureOffset(property, x);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004CA8 File Offset: 0x00002EA8
		public static Tweener DOTiling(this Material target, Vector2 endValue, float duration)
		{
			return DOTween.To(() => target.mainTextureScale, delegate(Vector2 x)
			{
				target.mainTextureScale = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004CEC File Offset: 0x00002EEC
		public static Tweener DOTiling(this Material target, Vector2 endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			return DOTween.To(() => target.GetTextureScale(property), delegate(Vector2 x)
			{
				target.SetTextureScale(property, x);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004D60 File Offset: 0x00002F60
		public static Tweener DOVector(this Material target, Vector4 endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			return DOTween.To(() => target.GetVector(property), delegate(Vector4 x)
			{
				target.SetVector(property, x);
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public static Tweener DOResize(this TrailRenderer target, float toStartWidth, float toEndWidth, float duration)
		{
			return DOTween.To(() => new Vector2(target.startWidth, target.endWidth), delegate(Vector2 x)
			{
				target.startWidth = x.x;
				target.endWidth = x.y;
			}, new Vector2(toStartWidth, toEndWidth), duration).SetTarget(target);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004E20 File Offset: 0x00003020
		public static Tweener DOTime(this TrailRenderer target, float endValue, float duration)
		{
			return DOTween.To(() => target.time, delegate(float x)
			{
				target.time = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004E64 File Offset: 0x00003064
		public static Tweener DOMove(this Transform target, Vector3 endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, endValue, duration).SetOptions(snapping).SetTarget(target);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004EB0 File Offset: 0x000030B0
		public static Tweener DOMoveX(this Transform target, float endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(endValue, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetTarget(target);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004F0C File Offset: 0x0000310C
		public static Tweener DOMoveY(this Transform target, float endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, endValue, 0f), duration).SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004F68 File Offset: 0x00003168
		public static Tweener DOMoveZ(this Transform target, float endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, 0f, endValue), duration).SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004FC4 File Offset: 0x000031C4
		public static Tweener DOLocalMove(this Transform target, Vector3 endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, endValue, duration).SetOptions(snapping).SetTarget(target);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005010 File Offset: 0x00003210
		public static Tweener DOLocalMoveX(this Transform target, float endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(endValue, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetTarget(target);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000506C File Offset: 0x0000326C
		public static Tweener DOLocalMoveY(this Transform target, float endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, endValue, 0f), duration).SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000050C8 File Offset: 0x000032C8
		public static Tweener DOLocalMoveZ(this Transform target, float endValue, float duration, bool snapping = false)
		{
			return DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, 0f, endValue), duration).SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005124 File Offset: 0x00003324
		public static Tweener DORotate(this Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, delegate(Quaternion x)
			{
				target.rotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005178 File Offset: 0x00003378
		public static Tweener DORotateQuaternion(this Transform target, Quaternion endValue, float duration)
		{
			TweenerCore<Quaternion, Quaternion, NoOptions> tweenerCore = DOTween.To<Quaternion, Quaternion, NoOptions>(PureQuaternionPlugin.Plug(), () => target.rotation, delegate(Quaternion x)
			{
				target.rotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000051C4 File Offset: 0x000033C4
		public static Tweener DOLocalRotate(this Transform target, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.localRotation, delegate(Quaternion x)
			{
				target.localRotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005218 File Offset: 0x00003418
		public static Tweener DOLocalRotateQuaternion(this Transform target, Quaternion endValue, float duration)
		{
			TweenerCore<Quaternion, Quaternion, NoOptions> tweenerCore = DOTween.To<Quaternion, Quaternion, NoOptions>(PureQuaternionPlugin.Plug(), () => target.localRotation, delegate(Quaternion x)
			{
				target.localRotation = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005264 File Offset: 0x00003464
		public static Tweener DOScale(this Transform target, Vector3 endValue, float duration)
		{
			return DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000052A8 File Offset: 0x000034A8
		public static Tweener DOScale(this Transform target, float endValue, float duration)
		{
			Vector3 endValue2 = new Vector3(endValue, endValue, endValue);
			return DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, endValue2, duration).SetTarget(target);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000052F8 File Offset: 0x000034F8
		public static Tweener DOScaleX(this Transform target, float endValue, float duration)
		{
			return DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, new Vector3(endValue, 0f, 0f), duration).SetOptions(AxisConstraint.X, false).SetTarget(target);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005354 File Offset: 0x00003554
		public static Tweener DOScaleY(this Transform target, float endValue, float duration)
		{
			return DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, new Vector3(0f, endValue, 0f), duration).SetOptions(AxisConstraint.Y, false).SetTarget(target);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000053B0 File Offset: 0x000035B0
		public static Tweener DOScaleZ(this Transform target, float endValue, float duration)
		{
			return DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, new Vector3(0f, 0f, endValue), duration).SetOptions(AxisConstraint.Z, false).SetTarget(target);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000540C File Offset: 0x0000360C
		public static Tweener DOLookAt(this Transform target, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None, Vector3? up = null)
		{
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => target.rotation, delegate(Quaternion x)
			{
				target.rotation = x;
			}, towards, duration).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetLookAt);
			tweenerCore.plugOptions.axisConstraint = axisConstraint;
			tweenerCore.plugOptions.up = ((up == null) ? Vector3.up : up.Value);
			return tweenerCore;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005484 File Offset: 0x00003684
		public static Tweener DOPunchPosition(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f, bool snapping = false)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOPunchPosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, punch, duration, vibrato, elasticity).SetTarget(target).SetOptions(snapping);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000054F0 File Offset: 0x000036F0
		public static Tweener DOPunchScale(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOPunchScale: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, punch, duration, vibrato, elasticity).SetTarget(target);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005554 File Offset: 0x00003754
		public static Tweener DOPunchRotation(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOPunchRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => target.localEulerAngles, delegate(Vector3 x)
			{
				target.localRotation = Quaternion.Euler(x);
			}, punch, duration, vibrato, elasticity).SetTarget(target);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000055B8 File Offset: 0x000037B8
		public static Tweener DOShakePosition(this Transform target, float duration, float strength = 1f, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000562C File Offset: 0x0000382C
		public static Tweener DOShakePosition(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakePosition: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000056A0 File Offset: 0x000038A0
		public static Tweener DOShakeRotation(this Transform target, float duration, float strength = 90f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localEulerAngles, delegate(Vector3 x)
			{
				target.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000570C File Offset: 0x0000390C
		public static Tweener DOShakeRotation(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localEulerAngles, delegate(Vector3 x)
			{
				target.localRotation = Quaternion.Euler(x);
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005778 File Offset: 0x00003978
		public static Tweener DOShakeScale(this Transform target, float duration, float strength = 1f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				Debug.Log(Debugger.logPriority);
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeScale: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, duration, strength, vibrato, randomness, false, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000057F4 File Offset: 0x000039F4
		public static Tweener DOShakeScale(this Transform target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOShakeScale: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, duration, strength, vibrato, randomness, fadeOut).SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005860 File Offset: 0x00003A60
		public static Sequence DOJump(this Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = 0f;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			Tween yTween = DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative<Tweener>().SetLoops(numJumps * 2, LoopType.Yoyo).OnStart(delegate
			{
				startPosY = target.position.y;
			});
			s.Append(DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(endValue.x, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Vector3(0f, 0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)).Join(yTween).SetTarget(target).SetEase(DOTween.defaultEaseType);
			yTween.OnUpdate(delegate
			{
				if (!offsetYSet)
				{
					offsetYSet = true;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 position = target.position;
				position.y += DOVirtual.EasedValue(0f, offsetY, yTween.ElapsedPercentage(true), Ease.OutQuad);
				target.position = position;
			});
			return s;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000059F4 File Offset: 0x00003BF4
		public static Sequence DOLocalJump(this Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
		{
			if (numJumps < 1)
			{
				numJumps = 1;
			}
			float startPosY = target.localPosition.y;
			float offsetY = -1f;
			bool offsetYSet = false;
			Sequence s = DOTween.Sequence();
			s.Append(DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(endValue.x, 0f, 0f), duration).SetOptions(AxisConstraint.X, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, 0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping).SetEase(Ease.Linear)).Join(DOTween.To(() => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Vector3(0f, jumpPower, 0f), duration / (float)(numJumps * 2)).SetOptions(AxisConstraint.Y, snapping).SetEase(Ease.OutQuad).SetRelative<Tweener>().SetLoops(numJumps * 2, LoopType.Yoyo)).SetTarget(target).SetEase(DOTween.defaultEaseType).OnUpdate(delegate
			{
				if (!offsetYSet)
				{
					offsetYSet = false;
					offsetY = (s.isRelative ? endValue.y : (endValue.y - startPosY));
				}
				Vector3 localPosition = target.localPosition;
				localPosition.y += DOVirtual.EasedValue(0f, offsetY, s.ElapsedDirectionalPercentage(), Ease.OutQuad);
				target.localPosition = localPosition;
			});
			return s;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005B70 File Offset: 0x00003D70
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, new Path(pathType, path, resolution, gizmoColor), duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005BD8 File Offset: 0x00003DD8
		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null)
		{
			if (resolution < 1)
			{
				resolution = 1;
			}
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, new Path(pathType, path, resolution, gizmoColor), duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005C4C File Offset: 0x00003E4C
		public static TweenerCore<Vector3, Path, PathOptions> DOPath(this Transform target, Path path, float duration, PathMode pathMode = PathMode.Full3D)
		{
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.position, delegate(Vector3 x)
			{
				target.position = x;
			}, path, duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			return tweenerCore;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public static TweenerCore<Vector3, Path, PathOptions> DOLocalPath(this Transform target, Path path, float duration, PathMode pathMode = PathMode.Full3D)
		{
			TweenerCore<Vector3, Path, PathOptions> tweenerCore = DOTween.To<Vector3, Path, PathOptions>(PathPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, path, duration).SetTarget(target);
			tweenerCore.plugOptions.mode = pathMode;
			tweenerCore.plugOptions.useLocalPosition = true;
			return tweenerCore;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005D08 File Offset: 0x00003F08
		public static Tweener DOTimeScale(this Tween target, float endValue, float duration)
		{
			return DOTween.To(() => target.timeScale, delegate(float x)
			{
				target.timeScale = x;
			}, endValue, duration).SetTarget(target);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005D4C File Offset: 0x00003F4C
		public static Tweener DOBlendableColor(this Light target, Color endValue, float duration)
		{
			endValue -= target.color;
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color color = x - to;
				to = x;
				target.color += color;
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public static Tweener DOBlendableColor(this Material target, Color endValue, float duration)
		{
			endValue -= target.color;
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color color = x - to;
				to = x;
				target.color += color;
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005E44 File Offset: 0x00004044
		public static Tweener DOBlendableColor(this Material target, Color endValue, string property, float duration)
		{
			if (!target.HasProperty(property))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(property);
				}
				return null;
			}
			endValue -= target.GetColor(property);
			Color to = new Color(0f, 0f, 0f, 0f);
			return DOTween.To(() => to, delegate(Color x)
			{
				Color color = x - to;
				to = x;
				target.SetColor(property, target.GetColor(property) + color);
			}, endValue, duration).Blendable<Color, Color, ColorOptions>().SetTarget(target);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005EF4 File Offset: 0x000040F4
		public static Tweener DOBlendableMoveBy(this Transform target, Vector3 byValue, float duration, bool snapping = false)
		{
			Vector3 to = Vector3.zero;
			return DOTween.To(() => to, delegate(Vector3 x)
			{
				Vector3 vector = x - to;
				to = x;
				target.position += vector;
			}, byValue, duration).Blendable<Vector3, Vector3, VectorOptions>().SetOptions(snapping).SetTarget(target);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005F50 File Offset: 0x00004150
		public static Tweener DOBlendableLocalMoveBy(this Transform target, Vector3 byValue, float duration, bool snapping = false)
		{
			Vector3 to = Vector3.zero;
			return DOTween.To(() => to, delegate(Vector3 x)
			{
				Vector3 vector = x - to;
				to = x;
				target.localPosition += vector;
			}, byValue, duration).Blendable<Vector3, Vector3, VectorOptions>().SetOptions(snapping).SetTarget(target);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005FAC File Offset: 0x000041AC
		public static Tweener DOBlendableRotateBy(this Transform target, Vector3 byValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			Quaternion to = Quaternion.identity;
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => to, delegate(Quaternion x)
			{
				Quaternion quaternion = x * Quaternion.Inverse(to);
				to = x;
				target.rotation = target.rotation * Quaternion.Inverse(target.rotation) * quaternion * target.rotation;
			}, byValue, duration).Blendable<Quaternion, Vector3, QuaternionOptions>().SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000600C File Offset: 0x0000420C
		public static Tweener DOBlendableLocalRotateBy(this Transform target, Vector3 byValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			Quaternion to = Quaternion.identity;
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => to, delegate(Quaternion x)
			{
				Quaternion quaternion = x * Quaternion.Inverse(to);
				to = x;
				target.localRotation = target.localRotation * Quaternion.Inverse(target.localRotation) * quaternion * target.localRotation;
			}, byValue, duration).Blendable<Quaternion, Vector3, QuaternionOptions>().SetTarget(target);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000606C File Offset: 0x0000426C
		public static Tweener DOBlendablePunchRotation(this Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("DOBlendablePunchRotation: duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			Vector3 to = Vector3.zero;
			return DOTween.Punch(() => to, delegate(Vector3 v)
			{
				Quaternion quaternion = Quaternion.Euler(to.x, to.y, to.z);
				Quaternion quaternion2 = Quaternion.Euler(v.x, v.y, v.z) * Quaternion.Inverse(quaternion);
				to = v;
				target.rotation = target.rotation * Quaternion.Inverse(target.rotation) * quaternion2 * target.rotation;
			}, punch, duration, vibrato, elasticity).Blendable<Vector3, Vector3[], Vector3ArrayOptions>().SetTarget(target);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000060E0 File Offset: 0x000042E0
		public static Tweener DOBlendableScaleBy(this Transform target, Vector3 byValue, float duration)
		{
			Vector3 to = Vector3.zero;
			return DOTween.To(() => to, delegate(Vector3 x)
			{
				Vector3 vector = x - to;
				to = x;
				target.localScale += vector;
			}, byValue, duration).Blendable<Vector3, Vector3, VectorOptions>().SetTarget(target);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006134 File Offset: 0x00004334
		public static int DOComplete(this Component target, bool withCallbacks = false)
		{
			return DOTween.Complete(target, withCallbacks);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006134 File Offset: 0x00004334
		public static int DOComplete(this Material target, bool withCallbacks = false)
		{
			return DOTween.Complete(target, withCallbacks);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000613D File Offset: 0x0000433D
		public static int DOKill(this Component target, bool complete = false)
		{
			return DOTween.Kill(target, complete);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000613D File Offset: 0x0000433D
		public static int DOKill(this Material target, bool complete = false)
		{
			return DOTween.Kill(target, complete);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006146 File Offset: 0x00004346
		public static int DOFlip(this Component target)
		{
			return DOTween.Flip(target);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006146 File Offset: 0x00004346
		public static int DOFlip(this Material target)
		{
			return DOTween.Flip(target);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000614E File Offset: 0x0000434E
		public static int DOGoto(this Component target, float to, bool andPlay = false)
		{
			return DOTween.Goto(target, to, andPlay);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000614E File Offset: 0x0000434E
		public static int DOGoto(this Material target, float to, bool andPlay = false)
		{
			return DOTween.Goto(target, to, andPlay);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006158 File Offset: 0x00004358
		public static int DOPause(this Component target)
		{
			return DOTween.Pause(target);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006158 File Offset: 0x00004358
		public static int DOPause(this Material target)
		{
			return DOTween.Pause(target);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006160 File Offset: 0x00004360
		public static int DOPlay(this Component target)
		{
			return DOTween.Play(target);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006160 File Offset: 0x00004360
		public static int DOPlay(this Material target)
		{
			return DOTween.Play(target);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006168 File Offset: 0x00004368
		public static int DOPlayBackwards(this Component target)
		{
			return DOTween.PlayBackwards(target);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006168 File Offset: 0x00004368
		public static int DOPlayBackwards(this Material target)
		{
			return DOTween.PlayBackwards(target);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006170 File Offset: 0x00004370
		public static int DOPlayForward(this Component target)
		{
			return DOTween.PlayForward(target);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006170 File Offset: 0x00004370
		public static int DOPlayForward(this Material target)
		{
			return DOTween.PlayForward(target);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006178 File Offset: 0x00004378
		public static int DORestart(this Component target, bool includeDelay = true)
		{
			return DOTween.Restart(target, includeDelay, -1f);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006178 File Offset: 0x00004378
		public static int DORestart(this Material target, bool includeDelay = true)
		{
			return DOTween.Restart(target, includeDelay, -1f);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006186 File Offset: 0x00004386
		public static int DORewind(this Component target, bool includeDelay = true)
		{
			return DOTween.Rewind(target, includeDelay);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006186 File Offset: 0x00004386
		public static int DORewind(this Material target, bool includeDelay = true)
		{
			return DOTween.Rewind(target, includeDelay);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000618F File Offset: 0x0000438F
		public static int DOSmoothRewind(this Component target)
		{
			return DOTween.SmoothRewind(target);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000618F File Offset: 0x0000438F
		public static int DOSmoothRewind(this Material target)
		{
			return DOTween.SmoothRewind(target);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006197 File Offset: 0x00004397
		public static int DOTogglePause(this Component target)
		{
			return DOTween.TogglePause(target);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006197 File Offset: 0x00004397
		public static int DOTogglePause(this Material target)
		{
			return DOTween.TogglePause(target);
		}
	}
}
