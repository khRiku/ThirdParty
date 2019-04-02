using System;
using DG.Tweening.Core;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public class Path
	{
		// Token: 0x06000232 RID: 562 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
		{
			this.type = type;
			this.subdivisionsXSegment = subdivisionsXSegment;
			if (gizmoColor != null)
			{
				this.gizmoColor = gizmoColor.Value;
			}
			this.AssignWaypoints(waypoints, true);
			this.AssignDecoder(type);
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Add(new TweenCallback(this.Draw));
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C961 File Offset: 0x0000AB61
		public Path()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C990 File Offset: 0x0000AB90
		public void FinalizePath(bool isClosedPath, AxisConstraint lockPositionAxes, Vector3 currTargetVal)
		{
			if (lockPositionAxes != AxisConstraint.None)
			{
				bool flag = (lockPositionAxes & AxisConstraint.X) == AxisConstraint.X;
				bool flag2 = (lockPositionAxes & AxisConstraint.Y) == AxisConstraint.Y;
				bool flag3 = (lockPositionAxes & AxisConstraint.Z) == AxisConstraint.Z;
				for (int i = 0; i < this.wps.Length; i++)
				{
					Vector3 vector = this.wps[i];
					this.wps[i] = new Vector3(flag ? currTargetVal.x : vector.x, flag2 ? currTargetVal.y : vector.y, flag3 ? currTargetVal.z : vector.z);
				}
			}
			this._decoder.FinalizePath(this, this.wps, isClosedPath);
			this.isFinalized = true;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CA37 File Offset: 0x0000AC37
		public Vector3 GetPoint(float perc, bool convertToConstantPerc = false)
		{
			if (convertToConstantPerc)
			{
				perc = this.ConvertToConstantPathPerc(perc);
			}
			return this._decoder.GetPoint(perc, this.wps, this, this.controlPoints);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CA60 File Offset: 0x0000AC60
		public float ConvertToConstantPathPerc(float perc)
		{
			if (this.type == PathType.Linear)
			{
				return perc;
			}
			if (perc > 0f && perc < 1f)
			{
				float num = this.length * perc;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				int num6 = this.lengthsTable.Length;
				int i = 0;
				while (i < num6)
				{
					if (this.lengthsTable[i] > num)
					{
						num4 = this.timesTable[i];
						num5 = this.lengthsTable[i];
						if (i > 0)
						{
							num3 = this.lengthsTable[i - 1];
							break;
						}
						break;
					}
					else
					{
						num2 = this.timesTable[i];
						i++;
					}
				}
				perc = num2 + (num - num3) / (num5 - num3) * (num4 - num2);
			}
			if (perc > 1f)
			{
				perc = 1f;
			}
			else if (perc < 0f)
			{
				perc = 0f;
			}
			return perc;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public int GetWaypointIndexFromPerc(float perc, bool isMovingForward)
		{
			if (perc >= 1f)
			{
				return this.wps.Length - 1;
			}
			if (perc <= 0f)
			{
				return 0;
			}
			float num = this.length * perc;
			float num2 = 0f;
			int i = 0;
			int num3 = this.wpLengths.Length;
			while (i < num3)
			{
				num2 += this.wpLengths[i];
				if (i == num3 - 1)
				{
					if (!isMovingForward)
					{
						return i;
					}
					return i - 1;
				}
				else if (num2 >= num)
				{
					if (num2 <= num)
					{
						return i;
					}
					if (!isMovingForward)
					{
						return i;
					}
					return i - 1;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
		public static Vector3[] GetDrawPoints(Path p, int drawSubdivisionsXSegment)
		{
			int num = p.wps.Length;
			if (p.type == PathType.Linear)
			{
				return p.wps;
			}
			int num2 = num * drawSubdivisionsXSegment;
			Vector3[] array = new Vector3[num2 + 1];
			for (int i = 0; i <= num2; i++)
			{
				float perc = (float)i / (float)num2;
				Vector3 point = p.GetPoint(perc, false);
				array[i] = point;
			}
			return array;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000CC14 File Offset: 0x0000AE14
		public static void RefreshNonLinearDrawWps(Path p)
		{
			int num = p.wps.Length * 10;
			if (p.nonLinearDrawWps == null || p.nonLinearDrawWps.Length != num + 1)
			{
				p.nonLinearDrawWps = new Vector3[num + 1];
			}
			for (int i = 0; i <= num; i++)
			{
				float perc = (float)i / (float)num;
				Vector3 point = p.GetPoint(perc, false);
				p.nonLinearDrawWps[i] = point;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000CC78 File Offset: 0x0000AE78
		public void Destroy()
		{
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Remove(new TweenCallback(this.Draw));
			}
			this.wps = null;
			this.wpLengths = (this.timesTable = (this.lengthsTable = null));
			this.nonLinearDrawWps = null;
			this.isFinalized = false;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000CCD4 File Offset: 0x0000AED4
		public Path CloneIncremental(int loopIncrement)
		{
			if (this._incrementalClone != null)
			{
				if (this._incrementalIndex == loopIncrement)
				{
					return this._incrementalClone;
				}
				this._incrementalClone.Destroy();
			}
			int num = this.wps.Length;
			Vector3 vector = this.wps[num - 1] - this.wps[0];
			Vector3[] array = new Vector3[this.wps.Length];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.wps[i] + vector * (float)loopIncrement;
			}
			int num2 = this.controlPoints.Length;
			ControlPoint[] array2 = new ControlPoint[num2];
			for (int j = 0; j < num2; j++)
			{
				array2[j] = this.controlPoints[j] + vector * (float)loopIncrement;
			}
			Vector3[] array3 = null;
			if (this.nonLinearDrawWps != null)
			{
				int num3 = this.nonLinearDrawWps.Length;
				array3 = new Vector3[num3];
				for (int k = 0; k < num3; k++)
				{
					array3[k] = this.nonLinearDrawWps[k] + vector * (float)loopIncrement;
				}
			}
			this._incrementalClone = new Path();
			this._incrementalIndex = loopIncrement;
			this._incrementalClone.type = this.type;
			this._incrementalClone.subdivisionsXSegment = this.subdivisionsXSegment;
			this._incrementalClone.subdivisions = this.subdivisions;
			this._incrementalClone.wps = array;
			this._incrementalClone.controlPoints = array2;
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Add(new TweenCallback(this._incrementalClone.Draw));
			}
			this._incrementalClone.length = this.length;
			this._incrementalClone.wpLengths = this.wpLengths;
			this._incrementalClone.timesTable = this.timesTable;
			this._incrementalClone.lengthsTable = this.lengthsTable;
			this._incrementalClone._decoder = this._decoder;
			this._incrementalClone.nonLinearDrawWps = array3;
			this._incrementalClone.targetPosition = this.targetPosition;
			this._incrementalClone.lookAtPosition = this.lookAtPosition;
			this._incrementalClone.isFinalized = true;
			return this._incrementalClone;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000CF1C File Offset: 0x0000B11C
		public void AssignWaypoints(Vector3[] newWps, bool cloneWps = false)
		{
			if (cloneWps)
			{
				int num = newWps.Length;
				this.wps = new Vector3[num];
				for (int i = 0; i < num; i++)
				{
					this.wps[i] = newWps[i];
				}
				return;
			}
			this.wps = newWps;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000CF64 File Offset: 0x0000B164
		public void AssignDecoder(PathType pathType)
		{
			this.type = pathType;
			if (pathType == PathType.Linear)
			{
				if (Path._linearDecoder == null)
				{
					Path._linearDecoder = new LinearDecoder();
				}
				this._decoder = Path._linearDecoder;
				return;
			}
			if (Path._catmullRomDecoder == null)
			{
				Path._catmullRomDecoder = new CatmullRomDecoder();
			}
			this._decoder = Path._catmullRomDecoder;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		public void Draw()
		{
			Path.Draw(this);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		private static void Draw(Path p)
		{
			if (p.timesTable == null)
			{
				return;
			}
			Color color = p.gizmoColor;
			color.a *= 0.5f;
			Gizmos.color = p.gizmoColor;
			int num = p.wps.Length;
			if (p._changed || (p.type != PathType.Linear && p.nonLinearDrawWps == null))
			{
				p._changed = false;
				if (p.type != PathType.Linear)
				{
					Path.RefreshNonLinearDrawWps(p);
				}
			}
			if (p.type == PathType.Linear)
			{
				Vector3 vector = p.wps[0];
				for (int i = 0; i < num; i++)
				{
					Vector3 vector2 = p.wps[i];
					Gizmos.DrawLine(vector2, vector);
					vector = vector2;
				}
			}
			else
			{
				Vector3 vector = p.nonLinearDrawWps[0];
				int num2 = p.nonLinearDrawWps.Length;
				for (int j = 1; j < num2; j++)
				{
					Vector3 vector3 = p.nonLinearDrawWps[j];
					Gizmos.DrawLine(vector3, vector);
					vector = vector3;
				}
			}
			Gizmos.color = color;
			for (int k = 0; k < num; k++)
			{
				Gizmos.DrawSphere(p.wps[k], 0.075f);
			}
			if (p.lookAtPosition != null)
			{
				Vector3 value = p.lookAtPosition.Value;
				Gizmos.DrawLine(p.targetPosition, value);
				Gizmos.DrawWireSphere(value, 0.075f);
			}
		}

		// Token: 0x04000102 RID: 258
		private static CatmullRomDecoder _catmullRomDecoder;

		// Token: 0x04000103 RID: 259
		private static LinearDecoder _linearDecoder;

		// Token: 0x04000104 RID: 260
		public float[] wpLengths;

		// Token: 0x04000105 RID: 261
		[SerializeField]
		public PathType type;

		// Token: 0x04000106 RID: 262
		[SerializeField]
		public int subdivisionsXSegment;

		// Token: 0x04000107 RID: 263
		[SerializeField]
		public int subdivisions;

		// Token: 0x04000108 RID: 264
		[SerializeField]
		public Vector3[] wps;

		// Token: 0x04000109 RID: 265
		[SerializeField]
		public ControlPoint[] controlPoints;

		// Token: 0x0400010A RID: 266
		[SerializeField]
		public float length;

		// Token: 0x0400010B RID: 267
		[SerializeField]
		public bool isFinalized;

		// Token: 0x0400010C RID: 268
		[SerializeField]
		public float[] timesTable;

		// Token: 0x0400010D RID: 269
		[SerializeField]
		public float[] lengthsTable;

		// Token: 0x0400010E RID: 270
		public int linearWPIndex = -1;

		// Token: 0x0400010F RID: 271
		private Path _incrementalClone;

		// Token: 0x04000110 RID: 272
		private int _incrementalIndex;

		// Token: 0x04000111 RID: 273
		private ABSPathDecoder _decoder;

		// Token: 0x04000112 RID: 274
		private bool _changed;

		// Token: 0x04000113 RID: 275
		public Vector3[] nonLinearDrawWps;

		// Token: 0x04000114 RID: 276
		public Vector3 targetPosition;

		// Token: 0x04000115 RID: 277
		public Vector3? lookAtPosition;

		// Token: 0x04000116 RID: 278
		public Color gizmoColor = new Color(1f, 1f, 1f, 0.7f);
	}
}
