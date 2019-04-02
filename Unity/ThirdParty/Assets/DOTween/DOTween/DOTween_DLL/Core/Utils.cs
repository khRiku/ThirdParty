using System;
using System.Reflection;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000050 RID: 80
	public static class Utils
	{
		// Token: 0x0600029F RID: 671 RVA: 0x0000F214 File Offset: 0x0000D414
		internal static Vector3 Vector3FromAngle(float degrees, float magnitude)
		{
			float num = degrees * 0.0174532924f;
			return new Vector3(magnitude * Mathf.Cos(num), magnitude * Mathf.Sin(num), 0f);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000F244 File Offset: 0x0000D444
		internal static float Angle2D(Vector3 from, Vector3 to)
		{
			Vector2 right = Vector2.right;
			to -= from;
			float num = Vector2.Angle(right, to);
			if (Vector3.Cross(right, to).z > 0f)
			{
				num = 360f - num;
			}
			return num * -1f;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F294 File Offset: 0x0000D494
		internal static bool Vector3AreApproximatelyEqual(Vector3 a, Vector3 b)
		{
			return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000F2D0 File Offset: 0x0000D4D0
		public static Type GetLooseScriptType(string typeName)
		{
			for (int i = 0; i < Utils._defAssembliesToQuery.Length; i++)
			{
				Type type = Type.GetType(string.Format("{0}, {1}", typeName, Utils._defAssembliesToQuery[i]));
				if (type != null)
				{
					return type;
				}
			}
			if (Utils._loadedAssemblies == null)
			{
				Utils._loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			}
			for (int j = 0; j < Utils._loadedAssemblies.Length; j++)
			{
				Type type2 = Type.GetType(string.Format("{0}, {1}", typeName, Utils._loadedAssemblies[j].GetName()));
				if (type2 != null)
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x0400015D RID: 349
		private static Assembly[] _loadedAssemblies;

		// Token: 0x0400015E RID: 350
		private static readonly string[] _defAssembliesToQuery = new string[]
		{
			"Assembly-CSharp",
			"Assembly-CSharp-firstpass"
		};
	}
}
