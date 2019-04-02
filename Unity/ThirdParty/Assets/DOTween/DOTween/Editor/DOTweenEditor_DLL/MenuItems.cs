using System;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
	// Token: 0x02000004 RID: 4
	internal static class MenuItems
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002298 File Offset: 0x00000498
		[MenuItem("GameObject/Demigiant/DOTween Manager", false, 20)]
		private static void CreateDOTweenComponent(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("[DOTween]");
			gameObject.AddComponent<DOTweenComponent>();
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
			Selection.activeObject = gameObject;
		}
	}
}
