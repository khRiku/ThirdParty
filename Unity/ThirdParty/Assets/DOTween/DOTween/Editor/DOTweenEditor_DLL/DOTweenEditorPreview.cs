using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
	// Token: 0x02000003 RID: 3
	public static class DOTweenEditorPreview
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020F7 File Offset: 0x000002F7
		static DOTweenEditorPreview()
		{
			DOTweenEditorPreview.Clear();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
		public static void Start(Action onPreviewUpdated = null)
		{
			if (DOTweenEditorPreview._isPreviewing || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}
			DOTweenEditorPreview._isPreviewing = true;
			DOTweenEditorPreview._onPreviewUpdated = onPreviewUpdated;
			DOTweenEditorPreview._previewTime = EditorApplication.timeSinceStartup;
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(DOTweenEditorPreview.PreviewUpdate));
			DOTweenEditorPreview._previewObj = new GameObject("-[ DOTween Preview ► ]-", new Type[]
			{
				typeof(DOTweenEditorPreview.PreviewComponent)
			});
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002174 File Offset: 0x00000374
		public static void Stop()
		{
			DOTweenEditorPreview._isPreviewing = false;
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(DOTweenEditorPreview.PreviewUpdate));
			DOTweenEditorPreview._onPreviewUpdated = null;
			DOTweenEditorPreview.Clear();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021A8 File Offset: 0x000003A8
		public static void PrepareTweenForPreview(Tween t, bool clearCallbacks = true, bool preventAutoKill = true, bool andPlay = true)
		{
			t.SetUpdate(UpdateType.Manual);
			if (preventAutoKill)
			{
				t.SetAutoKill(false);
			}
			if (clearCallbacks)
			{
				t.OnComplete(null).OnStart(null).OnPlay(null).OnPause(null).OnUpdate(null).OnWaypointChange(null).OnStepComplete(null).OnRewind(null).OnKill(null);
			}
			if (andPlay)
			{
				t.Play<Tween>();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002210 File Offset: 0x00000410
		private static void Clear()
		{
			DOTweenEditorPreview._previewObj = null;
			DOTweenEditorPreview.PreviewComponent[] array = UnityEngine.Object.FindObjectsOfType<DOTweenEditorPreview.PreviewComponent>();
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(array[i].gameObject);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002244 File Offset: 0x00000444
		private static void PreviewUpdate()
		{
			double previewTime = DOTweenEditorPreview._previewTime;
			DOTweenEditorPreview._previewTime = EditorApplication.timeSinceStartup;
			float num = (float)(DOTweenEditorPreview._previewTime - previewTime);
			DOTween.ManualUpdate(num, num);
			if (DOTweenEditorPreview._previewObj != null)
			{
				EditorUtility.SetDirty(DOTweenEditorPreview._previewObj);
			}
			if (DOTweenEditorPreview._onPreviewUpdated != null)
			{
				DOTweenEditorPreview._onPreviewUpdated();
			}
		}

		// Token: 0x04000004 RID: 4
		private static bool _isPreviewing;

		// Token: 0x04000005 RID: 5
		private static double _previewTime;

		// Token: 0x04000006 RID: 6
		private static Action _onPreviewUpdated;

		// Token: 0x04000007 RID: 7
		private static GameObject _previewObj;

		// Token: 0x0200000E RID: 14
		private class PreviewComponent : MonoBehaviour
		{
		}
	}
}
