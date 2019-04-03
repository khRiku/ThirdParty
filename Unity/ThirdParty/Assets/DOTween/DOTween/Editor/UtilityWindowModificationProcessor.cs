// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.UtilityWindowModificationProcessor
// Assembly: DOTweenEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5921EEE1-1EAD-4AE3-AEC3-8051606D5E53
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\Editor\DOTweenEditor.dll

using System.IO;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
  public class UtilityWindowModificationProcessor : AssetModificationProcessor
  {
    private static AssetDeleteResult OnWillDeleteAsset(string asset, RemoveAssetOptions options)
    {
      string fullPath = EditorUtils.ADBPathToFullPath(asset);
      if (!Directory.Exists(fullPath))
        return AssetDeleteResult.DidNotDelete;
      string[] files = Directory.GetFiles(fullPath, "DOTween.dll", SearchOption.AllDirectories);
      int length = files.Length;
      bool flag = false;
      for (int index = 0; index < length; ++index)
      {
        if (files[index].EndsWith("DOTween.dll"))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return AssetDeleteResult.DidNotDelete;
      Debug.Log((object) "::: DOTween deleted");
      EditorPrefs.DeleteKey(Application.dataPath + "DOTweenVersion");
      EditorPrefs.DeleteKey(Application.dataPath + "DOTweenProVersion");
      return AssetDeleteResult.DidNotDelete;
    }
  }
}
