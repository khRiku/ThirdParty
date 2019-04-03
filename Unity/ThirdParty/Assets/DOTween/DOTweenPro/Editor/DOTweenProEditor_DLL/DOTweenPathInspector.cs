// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.DOTweenPathInspector
// Assembly: DOTweenProEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1AF96003-A4AA-47A6-9D47-0CF90D290097
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTweenPro\Editor\DOTweenProEditor.dll

using DG.DemiEditor;
using DG.DOTweenEditor.Core;
using DG.DOTweenEditor.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DG.DOTweenEditor
{
  [CustomEditor(typeof (DOTweenPath))]
  public class DOTweenPathInspector : ABSAnimationInspector
  {
    private readonly Color _wpColor = Color.white;
    private readonly Color _arrowsColor = new Color(1f, 1f, 1f, 0.85f);
    private readonly Color _wpColorEnd = Color.red;
    private readonly List<WpHandle> _wpsByDepth = new List<WpHandle>();
    private int _selectedWpIndex = -1;
    private int _lastSelectedWpIndex = -1;
    private int _lastCreatedWpIndex = -1;
    private DOTweenPath _src;
    private int _minHandleControlId;
    private int _maxHandleControlId;
    private bool _changed;
    private Vector3 _lastSceneViewCamPosition;
    private Quaternion _lastSceneViewCamRotation;
    private bool _isDragging;
    private bool _reselectAfterDrag;
    private bool _sceneCamStored;
    private bool _refreshAfterEnable;
    private MethodInfo _miHasRigidbody;
    private Camera _fooSceneCam;
    private Transform _fooSceneCamTrans;
    private ReorderableList _wpsList;
    public bool updater;

    private bool _showAddManager
    {
      get
      {
        if (this._src.inspectorMode != DOTweenInspectorMode.Default)
          return this._src.inspectorMode == DOTweenInspectorMode.Developer;
        return true;
      }
    }

    private bool _showTweenSettings
    {
      get
      {
        if (this._src.inspectorMode != DOTweenInspectorMode.Default)
          return this._src.inspectorMode == DOTweenInspectorMode.Developer;
        return true;
      }
    }

    private Camera _sceneCam
    {
      get
      {
        if ((UnityEngine.Object) this._fooSceneCam == (UnityEngine.Object) null)
        {
          SceneView drawingSceneView = SceneView.currentDrawingSceneView;
          if ((UnityEngine.Object) drawingSceneView == (UnityEngine.Object) null)
            return (Camera) null;
          this._fooSceneCam = drawingSceneView.camera;
        }
        return this._fooSceneCam;
      }
    }

    private Transform _sceneCamTrans
    {
      get
      {
        if ((UnityEngine.Object) this._fooSceneCamTrans == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) this._sceneCam == (UnityEngine.Object) null)
            return (Transform) null;
          this._fooSceneCamTrans = this._sceneCam.transform;
        }
        return this._fooSceneCamTrans;
      }
    }

    private void OnEnable()
    {
      this._src = this.target as DOTweenPath;
      this.StoreSceneCamData();
      if (this._src.path == null)
        this.ResetPath(RepaintMode.None);
      this.onStartProperty = this.serializedObject.FindProperty("onStart");
      this.onPlayProperty = this.serializedObject.FindProperty("onPlay");
      this.onUpdateProperty = this.serializedObject.FindProperty("onUpdate");
      this.onStepCompleteProperty = this.serializedObject.FindProperty("onStepComplete");
      this.onCompleteProperty = this.serializedObject.FindProperty("onComplete");
      this.onRewindProperty = this.serializedObject.FindProperty("onRewind");
      this.onTweenCreatedProperty = this.serializedObject.FindProperty("onTweenCreated");
      this._refreshAfterEnable = true;
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      if (this._miHasRigidbody == null)
        this._miHasRigidbody = Utils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils+Physics").GetMethod("HasRigidbody", BindingFlags.Static | BindingFlags.Public);
      EditorGUIUtils.SetGUIStyles(new Vector2?());
      GUILayout.Space(3f);
      EditorGUIUtils.InspectorLogo();
      if (Application.isPlaying)
      {
        GUILayout.Space(8f);
        GUILayout.Label("Path Editor disabled while in play mode", EditorGUIUtils.wordWrapLabelStyle, new GUILayoutOption[0]);
        GUILayout.Space(10f);
      }
      else
      {
        if (this._refreshAfterEnable)
        {
          this._refreshAfterEnable = false;
          if (this._src.path == null)
            this.ResetPath(RepaintMode.None);
          else
            this.RefreshPath(RepaintMode.Scene, true);
          this._wpsList = new ReorderableList((IList) this._src.wps, typeof (Vector3), true, true, true, true);
          this._wpsList.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate) (rect => EditorGUI.LabelField(rect, "Path Waypoints"));
          this._wpsList.onReorderCallback = (ReorderableList.ReorderCallbackDelegate) (list => this.RefreshPath(RepaintMode.Scene, true));
          this._wpsList.drawElementCallback = (ReorderableList.ElementCallbackDelegate) ((rect, index, isActive, isFocused) =>
          {
            Rect position1 = new Rect(rect.xMin, rect.yMin, 23f, rect.height);
            Rect position2 = new Rect(position1.xMax, position1.yMin, rect.width - 23f, position1.height);
            GUI.Label(position1, (index + 1).ToString());
            this._src.wps[index] = EditorGUI.Vector3Field(position2, "", this._src.wps[index]);
          });
        }
        bool flag1 = false;
        Undo.RecordObject((UnityEngine.Object) this._src, "DOTween Path");
        if (this._src.inspectorMode != DOTweenInspectorMode.Default)
        {
          GUILayout.Label("Inspector Mode: <b>" + (object) this._src.inspectorMode + "</b>", ABSAnimationInspector.styles.custom.warningLabel, new GUILayoutOption[0]);
          GUILayout.Space(2f);
        }
        if (!((UnityEngine.Object) this._src.GetComponent<DOTweenVisualManager>() != (UnityEngine.Object) null) && this._showAddManager)
        {
          if (GUILayout.Button(new GUIContent("Add Manager", "Adds a manager component which allows you to choose additional options for this gameObject")))
            this._src.gameObject.AddComponent<DOTweenVisualManager>();
          GUILayout.Space(4f);
        }
        AnimationInspectorGUI.StickyTitle("Scene View Commands");
        DeGUILayout.BeginVBox(DeGUI.styles.box.stickyTop);
        GUILayout.Label("➲ SHIFT + " + (EditorUtils.isOSXEditor ? "CMD" : "CTRL") + ": add a waypoint\n➲ SHIFT + ALT: remove a waypoint");
        DeGUILayout.EndVBox();
        AnimationInspectorGUI.StickyTitle("Info");
        DeGUILayout.BeginVBox(DeGUI.styles.box.stickyTop);
        GUILayout.Label("Path Length: " + (this._src.path == null ? "-" : this._src.path.length.ToString()));
        DeGUILayout.EndVBox();
        if (this._showTweenSettings)
        {
          AnimationInspectorGUI.StickyTitle("Tween Options");
          GUILayout.BeginHorizontal();
          this._src.autoPlay = DeGUILayout.ToggleButton(this._src.autoPlay, new GUIContent("AutoPlay", "If selected, the tween will play automatically"), DeGUI.styles.button.tool, new GUILayoutOption[0]);
          this._src.autoKill = DeGUILayout.ToggleButton(this._src.autoKill, new GUIContent("AutoKill", "If selected, the tween will be killed when it completes, and won't be reusable"), DeGUI.styles.button.tool, new GUILayoutOption[0]);
          GUILayout.EndHorizontal();
          DeGUILayout.BeginVBox(DeGUI.styles.box.stickyTop);
          GUILayout.BeginHorizontal();
          this._src.duration = EditorGUILayout.FloatField("Duration", this._src.duration, new GUILayoutOption[0]);
          if ((double) this._src.duration < 0.0)
            this._src.duration = 0.0f;
          this._src.isSpeedBased = (DeGUILayout.ToggleButton((this._src.isSpeedBased ? 1 : 0) != 0, new GUIContent("SpeedBased", "If selected, the duration will count as units/degree x second"), DeGUI.styles.button.tool, new GUILayoutOption[1]
          {
            GUILayout.Width(75f)
          }) ? 1 : 0) != 0;
          GUILayout.EndHorizontal();
          this._src.delay = EditorGUILayout.FloatField("Delay", this._src.delay, new GUILayoutOption[0]);
          if ((double) this._src.delay < 0.0)
            this._src.delay = 0.0f;
          this._src.easeType = EditorGUIUtils.FilteredEasePopup(this._src.easeType);
          if (this._src.easeType == Ease.INTERNAL_Custom)
            this._src.easeCurve = EditorGUILayout.CurveField("   Ease Curve", this._src.easeCurve, new GUILayoutOption[0]);
          this._src.loops = EditorGUILayout.IntField(new GUIContent("Loops", "Set to -1 for infinite loops"), this._src.loops, new GUILayoutOption[0]);
          if (this._src.loops < -1)
            this._src.loops = -1;
          if (this._src.loops > 1 || this._src.loops == -1)
            this._src.loopType = (LoopType) EditorGUILayout.EnumPopup("   Loop Type", (Enum) this._src.loopType, new GUILayoutOption[0]);
          this._src.id = EditorGUILayout.TextField("ID", this._src.id, new GUILayoutOption[0]);
          this._src.updateType = (UpdateType) EditorGUILayout.EnumPopup("Update Type", (Enum) this._src.updateType, new GUILayoutOption[0]);
          if (this._src.inspectorMode == DOTweenInspectorMode.Developer)
          {
            GUILayout.BeginHorizontal();
            bool flag2 = (bool) this._miHasRigidbody.Invoke((object) null, new object[1]
            {
              (object) this._src
            });
            this._src.tweenRigidbody = EditorGUILayout.Toggle("Tween Rigidbody", flag2 && this._src.tweenRigidbody, new GUILayoutOption[0]);
            if (!flag2)
              GUILayout.Label("No rigidbody found", ABSAnimationInspector.styles.custom.warningLabel, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            if (this._src.tweenRigidbody)
              EditorGUILayout.HelpBox("Tweening a rigidbody works correctly only when it's kinematic", MessageType.Warning);
          }
          DeGUILayout.EndVBox();
          AnimationInspectorGUI.StickyTitle("Path Tween Options");
          DeGUILayout.BeginVBox(DeGUI.styles.box.stickyTop);
          PathType pathType = this._src.pathType;
          this._src.pathType = (PathType) EditorGUILayout.EnumPopup("Path Type", (Enum) this._src.pathType, new GUILayoutOption[0]);
          if (pathType != this._src.pathType)
            flag1 = true;
          if (this._src.pathType != PathType.Linear)
            this._src.pathResolution = EditorGUILayout.IntSlider("   Path resolution", this._src.pathResolution, 2, 20, new GUILayoutOption[0]);
          bool isClosedPath = this._src.isClosedPath;
          this._src.isClosedPath = EditorGUILayout.Toggle("Close Path", this._src.isClosedPath, new GUILayoutOption[0]);
          if (isClosedPath != this._src.isClosedPath)
            flag1 = true;
          this._src.isLocal = EditorGUILayout.Toggle(new GUIContent("Local Movement", "If checked, the path will tween the localPosition (instead than the position) of its target"), this._src.isLocal, new GUILayoutOption[0]);
          this._src.pathMode = (PathMode) EditorGUILayout.EnumPopup("Path Mode", (Enum) this._src.pathMode, new GUILayoutOption[0]);
          this._src.lockRotation = (AxisConstraint) EditorGUILayout.EnumPopup("Lock Rotation", (Enum) this._src.lockRotation, new GUILayoutOption[0]);
          this._src.orientType = (OrientType) EditorGUILayout.EnumPopup("Orientation", (Enum) this._src.orientType, new GUILayoutOption[0]);
          if (this._src.orientType != OrientType.None)
          {
            switch (this._src.orientType)
            {
              case OrientType.ToPath:
                this._src.lookAhead = EditorGUILayout.Slider("   LookAhead", this._src.lookAhead, 0.0f, 1f, new GUILayoutOption[0]);
                break;
              case OrientType.LookAtTransform:
                this._src.lookAtTransform = EditorGUILayout.ObjectField("   LookAt Target", (UnityEngine.Object) this._src.lookAtTransform, typeof (Transform), true, new GUILayoutOption[0]) as Transform;
                break;
              case OrientType.LookAtPosition:
                this._src.lookAtPosition = EditorGUILayout.Vector3Field("   LookAt Position", this._src.lookAtPosition);
                break;
            }
          }
          DeGUILayout.EndVBox();
        }
        AnimationInspectorGUI.StickyTitle("Path Editor Options");
        DeGUILayout.BeginVBox(DeGUI.styles.box.stickyTop);
        this._src.relative = EditorGUILayout.Toggle(new GUIContent("Relative", "If toggled, the whole path moves with the target"), this._src.relative, new GUILayoutOption[0]);
        this._src.pathColor = EditorGUILayout.ColorField("Color", this._src.pathColor, new GUILayoutOption[0]);
        this._src.showIndexes = EditorGUILayout.Toggle("Show Indexes", this._src.showIndexes, new GUILayoutOption[0]);
        this._src.showWpLength = EditorGUILayout.Toggle("Show WPs Lengths", this._src.showWpLength, new GUILayoutOption[0]);
        this._src.livePreview = EditorGUILayout.Toggle("Live Preview", this._src.livePreview, new GUILayoutOption[0]);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Handles Type/Mode", new GUILayoutOption[1]
        {
          GUILayout.Width(EditorGUIUtility.labelWidth - 11f)
        });
        this._src.handlesType = (HandlesType) EditorGUILayout.EnumPopup((Enum) this._src.handlesType);
        this._src.handlesDrawMode = (HandlesDrawMode) EditorGUILayout.EnumPopup((Enum) this._src.handlesDrawMode);
        GUILayout.EndHorizontal();
        if (this._src.handlesDrawMode == HandlesDrawMode.Perspective)
          this._src.perspectiveHandleSize = EditorGUILayout.FloatField("   Handle Size", this._src.perspectiveHandleSize, new GUILayoutOption[0]);
        DeGUILayout.EndVBox();
        if (this._showTweenSettings)
          AnimationInspectorGUI.AnimationEvents((ABSAnimationInspector) this, (ABSAnimationComponent) this._src);
        this.DrawExtras();
        GUILayout.Space(10f);
        DeGUILayout.BeginToolbar();
        this._src.wpsDropdown = DeGUILayout.ToolbarFoldoutButton(this._src.wpsDropdown, "Waypoints", false, false, new Color?());
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("Copy to clipboard", "Copies the current waypoints to clipboard, as an array ready to be pasted in your code"), DeGUI.styles.button.tool, new GUILayoutOption[0]))
          this.CopyWaypointsToClipboard();
        DeGUILayout.EndToolbar();
        if (this._src.wpsDropdown)
        {
          DeGUILayout.BeginVBox(DeGUI.styles.box.stickyTop);
          int num = GUI.changed ? 1 : 0;
          this._wpsList.DoLayoutList();
          if (num == 0 && GUI.changed)
            flag1 = true;
          DeGUILayout.EndVBox();
        }
        else
          GUILayout.Space(5f);
        if (flag1)
        {
          this.RefreshPath(RepaintMode.Scene, false);
        }
        else
        {
          if (!GUI.changed)
            return;
          EditorUtility.SetDirty((UnityEngine.Object) this._src);
          this.DORepaint(RepaintMode.Scene, false);
        }
      }
    }

    private void OnSceneGUI()
    {
      if (Application.isPlaying)
        return;
      this.StoreSceneCamData();
      if (!this._src.gameObject.activeInHierarchy || !this._sceneCamStored)
        return;
      if (this._wpsByDepth.Count != this._src.wps.Count)
        this.FillWpIndexByDepth();
      EditorGUIUtils.SetGUIStyles(new Vector2?());
      Event current = Event.current;
      Undo.RecordObject((UnityEngine.Object) this._src, "DOTween Path");
      if (current.type == EventType.MouseDown)
      {
        if (current.shift)
        {
          if (EditorGUI.actionKey)
          {
            Vector3 vector3 = this._lastCreatedWpIndex != -1 ? this._src.wps[this._lastCreatedWpIndex] : (this._selectedWpIndex != -1 ? this._src.wps[this._selectedWpIndex] : (this._lastSelectedWpIndex != -1 ? this._src.wps[this._lastSelectedWpIndex] : this._src.transform.position));
            Matrix4x4 worldToCameraMatrix = this._sceneCam.worldToCameraMatrix;
            float num1 = (float) -((double) worldToCameraMatrix.m20 * (double) vector3.x + (double) worldToCameraMatrix.m21 * (double) vector3.y + (double) worldToCameraMatrix.m22 * (double) vector3.z + (double) worldToCameraMatrix.m23);
            Camera sceneCam = this._sceneCam;
            double x = (double) current.mousePosition.x;
            Rect pixelRect = this._sceneCam.pixelRect;
            double width = (double) pixelRect.width;
            double num2 = x / width;
            double num3 = 1.0;
            double y = (double) current.mousePosition.y;
            pixelRect = this._sceneCam.pixelRect;
            double height = (double) pixelRect.height;
            double num4 = y / height;
            double num5 = num3 - num4;
            double num6 = (double) num1;
            Vector3 position = new Vector3((float) num2, (float) num5, (float) num6);
            Vector3 worldPoint = sceneCam.ViewportToWorldPoint(position);
            if (this._selectedWpIndex != -1 && this._selectedWpIndex < this._src.wps.Count - 1)
            {
              this._src.wps.Insert(this._selectedWpIndex + 1, worldPoint);
              this._lastCreatedWpIndex = this._selectedWpIndex + 1;
              this._selectedWpIndex = this._lastCreatedWpIndex;
            }
            else
            {
              this._src.wps.Add(worldPoint);
              this._lastCreatedWpIndex = this._src.wps.Count - 1;
              this._selectedWpIndex = this._lastCreatedWpIndex;
            }
            this.RefreshPath(RepaintMode.Scene, true);
            return;
          }
          if (current.alt && this._src.wps.Count > 1)
          {
            this.FindSelectedWaypointIndex();
            if (this._selectedWpIndex != -1)
            {
              this._src.wps.RemoveAt(this._selectedWpIndex);
              this.ResetIndexes();
              this.RefreshPath(RepaintMode.Scene, true);
              return;
            }
          }
        }
        this.FindSelectedWaypointIndex();
      }
      if (this._src.wps.Count < 1)
        return;
      if (current.type == EventType.MouseDrag)
      {
        this._isDragging = true;
        if (this._src.livePreview)
        {
          bool flag = this.CheckTargetMove();
          if (this._selectedWpIndex != -1)
            flag = true;
          if (flag)
            this.RefreshPath(RepaintMode.Scene, false);
        }
      }
      else if (this._isDragging && current.rawType == EventType.MouseUp)
      {
        if (this._isDragging && this._selectedWpIndex != -1)
          this._reselectAfterDrag = true;
        this._isDragging = false;
        if (this._selectedWpIndex != -1 || this.CheckTargetMove())
        {
          EditorUtility.SetDirty((UnityEngine.Object) this._src);
          this.RefreshPath(RepaintMode.Scene, true);
        }
      }
      else if (this.CheckTargetMove())
        this.RefreshPath(RepaintMode.Scene, false);
      if (this._changed && !this._isDragging)
      {
        this.FillWpIndexByDepth();
        this._changed = false;
      }
      int count = this._src.wps.Count;
      for (int index = 0; index < count; ++index)
      {
        WpHandle wpHandle = this._wpsByDepth[index];
        bool flag1 = wpHandle.wpIndex == this._selectedWpIndex;
        Vector3 wp = this._src.wps[wpHandle.wpIndex];
        float num1 = this._src.handlesDrawMode == HandlesDrawMode.Orthographic ? HandleUtility.GetHandleSize(wp) * 0.2f : this._src.perspectiveHandleSize;
        int num2 = wpHandle.wpIndex < 0 ? 0 : (wpHandle.wpIndex < (this._src.isClosedPath ? count : count - 1) ? 1 : 0);
        Vector3 arrowPointsAt = num2 != 0 ? (wpHandle.wpIndex >= count - 1 ? this._src.transform.position : this._src.wps[wpHandle.wpIndex + 1]) : Vector3.zero;
        bool flag2 = num2 != 0 && (double) Vector3.Distance(this._sceneCamTrans.position, wp) < (double) Vector3.Distance(this._sceneCamTrans.position, wp + Vector3.ClampMagnitude(arrowPointsAt - wp, num1 * 1.75f));
        Handles.color = !flag1 ? (wpHandle.wpIndex != count - 1 || this._src.isClosedPath ? this._wpColor : this._wpColorEnd) : Color.yellow;
        if ((num2 & (flag2 ? 1 : 0)) != 0)
          this.DrawArrowFor(wpHandle.wpIndex, num1, arrowPointsAt);
        int controlId1 = GUIUtility.GetControlID(FocusType.Passive);
        if (index == 0)
          this._minHandleControlId = controlId1;
        Vector3 position1 = this._src.handlesType != HandlesType.Free ? Handles.PositionHandle(wp, Quaternion.identity) : Handles.FreeMoveHandle(wp, Quaternion.identity, num1, Vector3.one, new Handles.DrawCapFunction(Handles.SphereCap));
        this._src.wps[wpHandle.wpIndex] = position1;
        int controlId2 = GUIUtility.GetControlID(FocusType.Passive);
        wpHandle.controlId = index == 0 ? controlId2 - 1 : controlId1 + 1;
        this._maxHandleControlId = controlId2;
        if (num2 != 0 && !flag2)
          this.DrawArrowFor(wpHandle.wpIndex, num1, arrowPointsAt);
        Vector3 position2 = this._sceneCamTrans.TransformPoint(this._sceneCamTrans.InverseTransformPoint(position1) + new Vector3(num1 * 0.75f, 0.1f, 0.0f));
        if (this._src.showIndexes || this._src.showWpLength)
        {
          string str;
          if (!this._src.showIndexes || !this._src.showWpLength)
            str = this._src.showIndexes ? (wpHandle.wpIndex + 1).ToString() : this._src.path.wpLengths[wpHandle.wpIndex + 1].ToString("N2");
          else
            str = (wpHandle.wpIndex + 1).ToString() + "(" + this._src.path.wpLengths[wpHandle.wpIndex + 1].ToString("N2") + ")";
          string text = str;
          Handles.Label(position2, text, flag1 ? EditorGUIUtils.handleSelectedLabelStyle : EditorGUIUtils.handlelabelStyle);
        }
      }
      Handles.color = this._src.pathColor;
      if (this._src.pathType == PathType.Linear)
        Handles.DrawPolyLine(this._src.path.wps);
      else if (this._src.path.nonLinearDrawWps != null)
        Handles.DrawPolyLine(this._src.path.nonLinearDrawWps);
      if (this._reselectAfterDrag && current.type == EventType.Repaint)
        this._reselectAfterDrag = false;
      if (!this._changed)
        this._changed = this.Changed();
      if (!this._changed)
        return;
      EditorUtility.SetDirty((UnityEngine.Object) this._src);
    }

    private void DORepaint(RepaintMode repaintMode, bool refreshWpIndexByDepth)
    {
      switch (repaintMode)
      {
        case RepaintMode.Scene:
          SceneView.RepaintAll();
          break;
        case RepaintMode.Inspector:
          EditorUtility.SetDirty((UnityEngine.Object) this._src);
          break;
        case RepaintMode.SceneAndInspector:
          EditorUtility.SetDirty((UnityEngine.Object) this._src);
          SceneView.RepaintAll();
          break;
      }
      if (!refreshWpIndexByDepth)
        return;
      this.FillWpIndexByDepth();
    }

    private bool Changed()
    {
      if (GUI.changed)
        return true;
      if (this._lastSelectedWpIndex != this._selectedWpIndex)
      {
        this._lastSelectedWpIndex = this._selectedWpIndex;
        return true;
      }
      if (this.CheckTargetMove())
        return true;
      if (!(this._sceneCamTrans.position != this._lastSceneViewCamPosition) && !(this._sceneCamTrans.rotation != this._lastSceneViewCamRotation))
        return false;
      this._lastSceneViewCamPosition = this._sceneCamTrans.position;
      this._lastSceneViewCamRotation = this._sceneCamTrans.rotation;
      return true;
    }

    private void DrawArrowFor(int wpIndex, float handleSize, Vector3 arrowPointsAt)
    {
      Color color = Handles.color;
      Handles.color = this._arrowsColor;
      Vector3 wp = this._src.wps[wpIndex];
      Vector3 vector3 = arrowPointsAt - wp;
      if ((double) vector3.magnitude >= (double) handleSize * 1.75)
        Handles.ConeCap(wpIndex, wp + Vector3.ClampMagnitude(vector3, handleSize), Quaternion.LookRotation(vector3), handleSize * 0.65f);
      Handles.color = color;
    }

    private void DrawExtras()
    {
      AnimationInspectorGUI.StickyTitle("Extras");
      DeGUILayout.BeginVBox(DeGUI.styles.box.sticky);
      if (GUILayout.Button("Reset Path"))
        this.ResetPath(RepaintMode.SceneAndInspector);
      DeGUILayout.EndVBox();
      GUILayout.Space(2f);
      GUILayout.BeginHorizontal(DeGUI.styles.box.stickyTop, new GUILayoutOption[0]);
      if (GUILayout.Button("Drop To Floor"))
        this.DropToFloor(this._src.dropToFloorOffset);
      GUILayout.Space(7f);
      GUILayout.Label("Offset Y", new GUILayoutOption[1]
      {
        GUILayout.Width(49f)
      });
      this._src.dropToFloorOffset = EditorGUILayout.FloatField(this._src.dropToFloorOffset, GUILayout.Width(40f));
      GUILayout.EndHorizontal();
    }

    private void StoreSceneCamData()
    {
      if ((UnityEngine.Object) this._sceneCam == (UnityEngine.Object) null)
      {
        this._sceneCamStored = false;
      }
      else
      {
        if (this._sceneCamStored || (UnityEngine.Object) this._sceneCam == (UnityEngine.Object) null)
          return;
        this._sceneCamStored = true;
        this._lastSceneViewCamPosition = this._sceneCamTrans.position;
        this._lastSceneViewCamRotation = this._sceneCamTrans.rotation;
      }
    }

    private void FillWpIndexByDepth()
    {
      if (!this._sceneCamStored)
        return;
      int count = this._src.wps.Count;
      if (count == 0)
        return;
      this._wpsByDepth.Clear();
      for (int wpIndex = 0; wpIndex < count; ++wpIndex)
        this._wpsByDepth.Add(new WpHandle(wpIndex));
      this._wpsByDepth.Sort((Comparison<WpHandle>) ((x, y) =>
      {
        float num1 = Vector3.Distance(this._sceneCamTrans.position, this._src.wps[x.wpIndex]);
        float num2 = Vector3.Distance(this._sceneCamTrans.position, this._src.wps[y.wpIndex]);
        if ((double) num1 > (double) num2)
          return -1;
        return (double) num1 < (double) num2 ? 1 : 0;
      }));
    }

    private void FindSelectedWaypointIndex()
    {
      this._lastSelectedWpIndex = this._selectedWpIndex;
      this._selectedWpIndex = -1;
      int count = this._src.wps.Count;
      if (count == 0)
        return;
      int nearestControl = HandleUtility.nearestControl;
      if (nearestControl == 0 || nearestControl < this._minHandleControlId || nearestControl > this._maxHandleControlId)
        return;
      int index1 = -1;
      for (int index2 = 0; index2 < count; ++index2)
      {
        int controlId = this._wpsByDepth[index2].controlId;
        switch (controlId)
        {
          case -1:
          case 0:
            continue;
          default:
            int wpIndex = this._wpsByDepth[index2].wpIndex;
            if (controlId > nearestControl)
            {
              this._selectedWpIndex = this._wpsByDepth[index1 == -1 ? index2 : index1].wpIndex;
              this._lastCreatedWpIndex = -1;
              return;
            }
            if (controlId == nearestControl)
            {
              this._selectedWpIndex = wpIndex;
              this._lastCreatedWpIndex = -1;
              return;
            }
            index1 = index2;
            continue;
        }
      }
      if (this._selectedWpIndex != -1)
        return;
      this._selectedWpIndex = this._wpsByDepth[index1].wpIndex;
      this._lastCreatedWpIndex = -1;
    }

    private void ResetPath(RepaintMode repaintMode)
    {
      this._src.wps.Clear();
      this._src.lastSrcPosition = this._src.transform.position;
      this._src.path = new Path(this._src.pathType, this._src.wps.ToArray(), 10, new Color?(this._src.pathColor));
      this._wpsByDepth.Clear();
      this.ResetIndexes();
      this.DORepaint(repaintMode, false);
    }

    private void ResetIndexes()
    {
      this._selectedWpIndex = this._lastSelectedWpIndex = this._lastCreatedWpIndex = -1;
    }

    private bool CheckTargetMove()
    {
      if (!(this._src.lastSrcPosition != this._src.transform.position))
        return false;
      if (this._src.relative)
      {
        Vector3 vector3 = this._src.transform.position - this._src.lastSrcPosition;
        int count = this._src.wps.Count;
        for (int index = 0; index < count; ++index)
          this._src.wps[index] += vector3;
      }
      this._src.lastSrcPosition = this._src.transform.position;
      return true;
    }

    private void RefreshPath(RepaintMode repaintMode, bool refreshWpIndexByDepth)
    {
      if (this._src.wps.Count < 1)
        return;
      this._src.path.AssignDecoder(this._src.pathType);
      this._src.path.AssignWaypoints(this._src.GetFullWps(), false);
      this._src.path.FinalizePath(this._src.isClosedPath, AxisConstraint.None, this._src.transform.position);
      if (this._src.pathType != PathType.Linear)
        Path.RefreshNonLinearDrawWps(this._src.path);
      this.DORepaint(repaintMode, refreshWpIndexByDepth);
    }

    private void DropToFloor(float offsetY)
    {
      bool flag = false;
      for (int index = 0; index < this._src.wps.Count; ++index)
      {
        Vector3 wp = this._src.wps[index];
        wp.y += 0.01f;
        RaycastHit hitInfo;
        if (Physics.Raycast(wp, Vector3.down, out hitInfo, float.PositiveInfinity))
        {
          flag = true;
          Vector3 point = hitInfo.point;
          point.y += offsetY;
          this._src.wps[index] = point;
          this.RefreshPath(RepaintMode.SceneAndInspector, true);
        }
      }
      if (!flag)
        EditorUtility.DisplayDialog("Drop To Floor", "No colliders to drop on.", "Ok");
      else
        EditorUtility.SetDirty((UnityEngine.Object) this._src);
    }

    private void CopyWaypointsToClipboard()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Vector3[] waypoints = new[] { ");
      for (int index = 0; index < this._src.wps.Count; ++index)
      {
        Vector3 wp = this._src.wps[index];
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append(string.Format("new Vector3({0}f,{1}f,{2}f)", (object) wp.x, (object) wp.y, (object) wp.z));
      }
      stringBuilder.Append(" };");
      EditorGUIUtility.systemCopyBuffer = stringBuilder.ToString();
    }
  }
}
