// Decompiled with JetBrains decompiler
// Type: DG.Tweening.DOTweenPath
// Assembly: DOTweenPro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0FFB13C2-E5DA-4737-82C8-2ACE533F01F7
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\DOTweenPro.dll

using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace DG.Tweening
{
  /// <summary>
  /// Attach this to a GameObject to create and assign a path to it
  /// </summary>
  [AddComponentMenu("DOTween/DOTween Path")]
  public class DOTweenPath : ABSAnimationComponent
  {
    public float duration = 1f;
    public Ease easeType = Ease.OutQuad;
    public AnimationCurve easeCurve = new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(1f, 1f)
    });
    public int loops = 1;
    public string id = "";
    public float lookAhead = 0.01f;
    public bool autoPlay = true;
    public bool autoKill = true;
    public int pathResolution = 10;
    public PathMode pathMode = PathMode.Full3D;
    public Vector3 forwardDirection = Vector3.forward;
    public Vector3 upDirection = Vector3.up;
    public List<Vector3> wps = new List<Vector3>();
    public List<Vector3> fullWps = new List<Vector3>();
    public bool livePreview = true;
    public float perspectiveHandleSize = 0.5f;
    public bool showIndexes = true;
    public Color pathColor = new Color(1f, 1f, 1f, 0.5f);
    public float delay;
    public LoopType loopType;
    public OrientType orientType;
    public Transform lookAtTransform;
    public Vector3 lookAtPosition;
    public bool relative;
    public bool isLocal;
    public bool isClosedPath;
    public AxisConstraint lockRotation;
    public bool assignForwardAndUp;
    public bool tweenRigidbody;
    public Path path;
    public DOTweenInspectorMode inspectorMode;
    public PathType pathType;
    public HandlesType handlesType;
    public HandlesDrawMode handlesDrawMode;
    public bool showWpLength;
    public Vector3 lastSrcPosition;
    public bool wpsDropdown;
    public float dropToFloorOffset;
    private static MethodInfo _miCreateTween;

    private void Awake()
    {
      if (this.path == null || this.wps.Count < 1 || this.inspectorMode == DOTweenInspectorMode.OnlyPath)
        return;
      if (DOTweenPath._miCreateTween == null)
        DOTweenPath._miCreateTween = Utils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils+Physics").GetMethod("CreateDOTweenPathTween", BindingFlags.Static | BindingFlags.Public);
      this.path.AssignDecoder(this.path.type);
      if (TweenManager.isUnityEditor)
      {
        DOTween.GizmosDelegates.Add(new TweenCallback(this.path.Draw));
        this.path.gizmoColor = this.pathColor;
      }
      if (this.isLocal)
      {
        Transform transform = this.transform;
        if ((Object) transform.parent != (Object) null)
        {
          Vector3 position = transform.parent.position;
          int length1 = this.path.wps.Length;
          for (int index = 0; index < length1; ++index)
            this.path.wps[index] = this.path.wps[index] - position;
          int length2 = this.path.controlPoints.Length;
          for (int index = 0; index < length2; ++index)
          {
            ControlPoint controlPoint = this.path.controlPoints[index];
            controlPoint.a -= position;
            controlPoint.b -= position;
            this.path.controlPoints[index] = controlPoint;
          }
        }
      }
      if (this.relative)
        this.ReEvaluateRelativeTween();
      if (this.pathMode == PathMode.Full3D && (Object) this.GetComponent<SpriteRenderer>() != (Object) null)
        this.pathMode = PathMode.TopDown2D;
      TweenerCore<Vector3, Path, PathOptions> t = (TweenerCore<Vector3, Path, PathOptions>) DOTweenPath._miCreateTween.Invoke((object) null, new object[6]
      {
        (object) this,
        (object) this.tweenRigidbody,
        (object) this.isLocal,
        (object) this.path,
        (object) this.duration,
        (object) this.pathMode
      });
      t.SetOptions(this.isClosedPath, AxisConstraint.None, this.lockRotation);
      switch (this.orientType)
      {
        case OrientType.ToPath:
          if (this.assignForwardAndUp)
          {
            t.SetLookAt(this.lookAhead, new Vector3?(this.forwardDirection), new Vector3?(this.upDirection));
            break;
          }
          t.SetLookAt(this.lookAhead, new Vector3?(), new Vector3?());
          break;
        case OrientType.LookAtTransform:
          if ((Object) this.lookAtTransform != (Object) null)
          {
            if (this.assignForwardAndUp)
            {
              t.SetLookAt(this.lookAtTransform, new Vector3?(this.forwardDirection), new Vector3?(this.upDirection));
              break;
            }
            t.SetLookAt(this.lookAtTransform, new Vector3?(), new Vector3?());
            break;
          }
          break;
        case OrientType.LookAtPosition:
          if (this.assignForwardAndUp)
          {
            t.SetLookAt(this.lookAtPosition, new Vector3?(this.forwardDirection), new Vector3?(this.upDirection));
            break;
          }
          t.SetLookAt(this.lookAtPosition, new Vector3?(), new Vector3?());
          break;
      }
      t.SetDelay<TweenerCore<Vector3, Path, PathOptions>>(this.delay).SetLoops<TweenerCore<Vector3, Path, PathOptions>>(this.loops, this.loopType).SetAutoKill<TweenerCore<Vector3, Path, PathOptions>>(this.autoKill).SetUpdate<TweenerCore<Vector3, Path, PathOptions>>(this.updateType).OnKill<TweenerCore<Vector3, Path, PathOptions>>((TweenCallback) (() => this.tween = (Tween) null));
      if (this.isSpeedBased)
        t.SetSpeedBased<TweenerCore<Vector3, Path, PathOptions>>();
      if (this.easeType == Ease.INTERNAL_Custom)
        t.SetEase<TweenerCore<Vector3, Path, PathOptions>>(this.easeCurve);
      else
        t.SetEase<TweenerCore<Vector3, Path, PathOptions>>(this.easeType);
      if (!string.IsNullOrEmpty(this.id))
        t.SetId<TweenerCore<Vector3, Path, PathOptions>>(this.id);
      if (this.hasOnStart)
      {
        if (this.onStart != null)
          t.OnStart<TweenerCore<Vector3, Path, PathOptions>>(new TweenCallback(this.onStart.Invoke));
      }
      else
        this.onStart = (UnityEvent) null;
      if (this.hasOnPlay)
      {
        if (this.onPlay != null)
          t.OnPlay<TweenerCore<Vector3, Path, PathOptions>>(new TweenCallback(this.onPlay.Invoke));
      }
      else
        this.onPlay = (UnityEvent) null;
      if (this.hasOnUpdate)
      {
        if (this.onUpdate != null)
          t.OnUpdate<TweenerCore<Vector3, Path, PathOptions>>(new TweenCallback(this.onUpdate.Invoke));
      }
      else
        this.onUpdate = (UnityEvent) null;
      if (this.hasOnStepComplete)
      {
        if (this.onStepComplete != null)
          t.OnStepComplete<TweenerCore<Vector3, Path, PathOptions>>(new TweenCallback(this.onStepComplete.Invoke));
      }
      else
        this.onStepComplete = (UnityEvent) null;
      if (this.hasOnComplete)
      {
        if (this.onComplete != null)
          t.OnComplete<TweenerCore<Vector3, Path, PathOptions>>(new TweenCallback(this.onComplete.Invoke));
      }
      else
        this.onComplete = (UnityEvent) null;
      if (this.hasOnRewind)
      {
        if (this.onRewind != null)
          t.OnRewind<TweenerCore<Vector3, Path, PathOptions>>(new TweenCallback(this.onRewind.Invoke));
      }
      else
        this.onRewind = (UnityEvent) null;
      if (this.autoPlay)
        t.Play<TweenerCore<Vector3, Path, PathOptions>>();
      else
        t.Pause<TweenerCore<Vector3, Path, PathOptions>>();
      this.tween = (Tween) t;
      if (!this.hasOnTweenCreated || this.onTweenCreated == null)
        return;
      this.onTweenCreated.Invoke();
    }

    private void Reset()
    {
      this.path = new Path(this.pathType, this.wps.ToArray(), 10, new Color?(this.pathColor));
    }

    private void OnDestroy()
    {
      if (this.tween != null && this.tween.active)
        this.tween.Kill(false);
      this.tween = (Tween) null;
    }

    public override void DOPlay()
    {
      this.tween.Play<Tween>();
    }

    public override void DOPlayBackwards()
    {
      this.tween.PlayBackwards();
    }

    public override void DOPlayForward()
    {
      this.tween.PlayForward();
    }

    public override void DOPause()
    {
      this.tween.Pause<Tween>();
    }

    public override void DOTogglePause()
    {
      this.tween.TogglePause();
    }

    public override void DORewind()
    {
      this.tween.Rewind(true);
    }

    /// <summary>Restarts the tween</summary>
    /// <param name="fromHere">If TRUE, re-evaluates the tween's start and end values from its current position.
    /// Set it to TRUE when spawning the same DOTweenPath in different positions (like when using a pooling system)</param>
    public override void DORestart(bool fromHere = false)
    {
      if (this.tween == null)
      {
        if (Debugger.logPriority <= 1)
          return;
        Debugger.LogNullTween(this.tween);
      }
      else
      {
        if (fromHere && this.relative && !this.isLocal)
          this.ReEvaluateRelativeTween();
        this.tween.Restart(true, -1f);
      }
    }

    public override void DOComplete()
    {
      this.tween.Complete();
    }

    public override void DOKill()
    {
      this.tween.Kill(false);
    }

    public Tween GetTween()
    {
      if (this.tween != null && this.tween.active)
        return this.tween;
      if (Debugger.logPriority > 1)
      {
        if (this.tween == null)
          Debugger.LogNullTween(this.tween);
        else
          Debugger.LogInvalidTween(this.tween);
      }
      return (Tween) null;
    }

    /// <summary>
    /// Returns a list of points that are used to draw the path inside the editor.
    /// </summary>
    public Vector3[] GetDrawPoints()
    {
      if (this.path.wps == null || this.path.nonLinearDrawWps == null)
      {
        Debugger.LogWarning((object) "Draw points not ready yet. Returning NULL");
        return (Vector3[]) null;
      }
      if (this.pathType == PathType.Linear)
        return this.path.wps;
      return this.path.nonLinearDrawWps;
    }

    public Vector3[] GetFullWps()
    {
      int count = this.wps.Count;
      int length = count + 1;
      if (this.isClosedPath)
        ++length;
      Vector3[] vector3Array = new Vector3[length];
      vector3Array[0] = this.transform.position;
      for (int index = 0; index < count; ++index)
        vector3Array[index + 1] = this.wps[index];
      if (this.isClosedPath)
        vector3Array[length - 1] = vector3Array[0];
      return vector3Array;
    }

    private void ReEvaluateRelativeTween()
    {
      Vector3 position = this.transform.position;
      if (position == this.lastSrcPosition)
        return;
      Vector3 vector3 = position - this.lastSrcPosition;
      int length1 = this.path.wps.Length;
      for (int index = 0; index < length1; ++index)
        this.path.wps[index] = this.path.wps[index] + vector3;
      int length2 = this.path.controlPoints.Length;
      for (int index = 0; index < length2; ++index)
      {
        ControlPoint controlPoint = this.path.controlPoints[index];
        controlPoint.a += vector3;
        controlPoint.b += vector3;
        this.path.controlPoints[index] = controlPoint;
      }
      this.lastSrcPosition = position;
    }
  }
}
