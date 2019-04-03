// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.DOTweenEditorPreview
// Assembly: DOTweenEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5921EEE1-1EAD-4AE3-AEC3-8051606D5E53
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\Editor\DOTweenEditor.dll

using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
  public static class DOTweenEditorPreview
  {
    private static bool _isPreviewing;
    private static double _previewTime;
    private static Action _onPreviewUpdated;
    private static GameObject _previewObj;

    static DOTweenEditorPreview()
    {
      DOTweenEditorPreview.Clear();
    }

    /// <summary>
    /// Starts the update loop of tween in the editor. Has no effect during playMode.
    /// </summary>
    /// <param name="onPreviewUpdated">Eventual callback to call after every update</param>
    public static void Start(Action onPreviewUpdated = null)
    {
      if (DOTweenEditorPreview._isPreviewing || EditorApplication.isPlayingOrWillChangePlaymode)
        return;
      DOTweenEditorPreview._isPreviewing = true;
      DOTweenEditorPreview._onPreviewUpdated = onPreviewUpdated;
      DOTweenEditorPreview._previewTime = EditorApplication.timeSinceStartup;
      EditorApplication.update += new EditorApplication.CallbackFunction(DOTweenEditorPreview.PreviewUpdate);
      DOTweenEditorPreview._previewObj = new GameObject("-[ DOTween Preview ► ]-", new System.Type[1]
      {
        typeof (DOTweenEditorPreview.PreviewComponent)
      });
    }

    /// <summary>Stops the update loop and clears any callback.</summary>
    public static void Stop()
    {
      DOTweenEditorPreview._isPreviewing = false;
      EditorApplication.update -= new EditorApplication.CallbackFunction(DOTweenEditorPreview.PreviewUpdate);
      DOTweenEditorPreview._onPreviewUpdated = (Action) null;
      DOTweenEditorPreview.Clear();
    }

    /// <summary>
    /// Readies the tween for editor preview by setting its UpdateType to Manual plus eventual extra settings.
    /// </summary>
    /// <param name="t">The tween to ready</param>
    /// <param name="clearCallbacks">If TRUE (recommended) removes all callbacks (OnComplete/Rewind/etc)</param>
    /// <param name="preventAutoKill">If TRUE prevents the tween from being auto-killed at completion</param>
    /// <param name="andPlay">If TRUE starts playing the tween immediately</param>
    public static void PrepareTweenForPreview(Tween t, bool clearCallbacks = true, bool preventAutoKill = true, bool andPlay = true)
    {
      t.SetUpdate<Tween>(UpdateType.Manual);
      if (preventAutoKill)
        t.SetAutoKill<Tween>(false);
      if (clearCallbacks)
        t.OnComplete<Tween>((TweenCallback) null).OnStart<Tween>((TweenCallback) null).OnPlay<Tween>((TweenCallback) null).OnPause<Tween>((TweenCallback) null).OnUpdate<Tween>((TweenCallback) null).OnWaypointChange<Tween>((TweenCallback<int>) null).OnStepComplete<Tween>((TweenCallback) null).OnRewind<Tween>((TweenCallback) null).OnKill<Tween>((TweenCallback) null);
      if (!andPlay)
        return;
      t.Play<Tween>();
    }

    private static void Clear()
    {
      DOTweenEditorPreview._previewObj = (GameObject) null;
      foreach (Component component in UnityEngine.Object.FindObjectsOfType<DOTweenEditorPreview.PreviewComponent>())
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component.gameObject);
    }

    private static void PreviewUpdate()
    {
      double previewTime = DOTweenEditorPreview._previewTime;
      DOTweenEditorPreview._previewTime = EditorApplication.timeSinceStartup;
      double num = DOTweenEditorPreview._previewTime - previewTime;
      DOTween.ManualUpdate((float) num, (float) num);
      if ((UnityEngine.Object) DOTweenEditorPreview._previewObj != (UnityEngine.Object) null)
        EditorUtility.SetDirty((UnityEngine.Object) DOTweenEditorPreview._previewObj);
      if (DOTweenEditorPreview._onPreviewUpdated == null)
        return;
      DOTweenEditorPreview._onPreviewUpdated();
    }

    private class PreviewComponent : MonoBehaviour
    {
    }
  }
}
