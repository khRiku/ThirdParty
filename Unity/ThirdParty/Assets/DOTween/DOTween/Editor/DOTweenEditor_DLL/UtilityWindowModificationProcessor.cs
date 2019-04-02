using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
	// Token: 0x02000006 RID: 6
	public class UtilityWindowModificationProcessor : AssetModificationProcessor
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002E0C File Offset: 0x0000100C
		private static AssetDeleteResult OnWillDeleteAsset(string asset, RemoveAssetOptions options)
		{
			string path = EditorUtils.ADBPathToFullPath(asset);
			if (!Directory.Exists(path))
			{
				return 0;
			}
			string[] files = Directory.GetFiles(path, "DOTween.dll", SearchOption.AllDirectories);
			int num = files.Length;
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (files[i].EndsWith("DOTween.dll"))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return 0;
			}
			Debug.Log("::: DOTween deleted");
			EditorPrefs.DeleteKey(Application.dataPath + "DOTweenVersion");
			EditorPrefs.DeleteKey(Application.dataPath + "DOTweenProVersion");
			return 0;
		}
	}
}
