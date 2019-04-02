using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenUpgradeManager
{
	// Token: 0x02000002 RID: 2
	[InitializeOnLoad]
	internal static class Autorun
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		static Autorun()
		{
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(Autorun.OnUpdate));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002072 File Offset: 0x00000272
		public static void OnUpdate()
		{
			if (!Autorun.UpgradeWindowIsOpen())
			{
				Autorun.ApplyModulesSettings();
				UpgradeWindow.Open();
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002085 File Offset: 0x00000285
		private static bool UpgradeWindowIsOpen()
		{
			return Resources.FindObjectsOfTypeAll<UpgradeWindow>().Length != 0;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002090 File Offset: 0x00000290
		private static void ApplyModulesSettings()
		{
			Type type = Type.GetType("DG.DOTweenEditor.UI.DOTweenUtilityWindowModules, DOTweenEditor");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("ApplyModulesSettings", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}
	}
}
