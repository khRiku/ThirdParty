using System;
using DG.DOTweenEditor.UI;
using UnityEditor;

namespace DG.DOTweenEditor
{
	// Token: 0x02000007 RID: 7
	public class UtilityWindowPostProcessor : AssetPostprocessor
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002EA1 File Offset: 0x000010A1
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			if (UtilityWindowPostProcessor._setupDialogRequested)
			{
				return;
			}
			if (Array.FindAll<string>(importedAssets, (string name) => name.Contains("DOTween") && !name.EndsWith(".meta") && !name.EndsWith(".jpg") && !name.EndsWith(".png")).Length == 0)
			{
				return;
			}
			DOTweenUtilityWindowModules.ApplyModulesSettings();
		}

		// Token: 0x04000015 RID: 21
		private static bool _setupDialogRequested;
	}
}
