// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.MenuItems
// Assembly: DOTweenEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5921EEE1-1EAD-4AE3-AEC3-8051606D5E53
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\Editor\DOTweenEditor.dll

using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
  internal static class MenuItems
  {
    [MenuItem("GameObject/Demigiant/DOTween Manager", false, 20)]
    private static void CreateDOTweenComponent(MenuCommand menuCommand)
    {
      GameObject child = new GameObject("[DOTween]");
      child.AddComponent<DOTweenComponent>();
      GameObjectUtility.SetParentAndAlign(child, menuCommand.context as GameObject);
      Undo.RegisterCreatedObjectUndo((Object) child, "Create " + child.name);
      Selection.activeObject = (Object) child;
    }
  }
}
