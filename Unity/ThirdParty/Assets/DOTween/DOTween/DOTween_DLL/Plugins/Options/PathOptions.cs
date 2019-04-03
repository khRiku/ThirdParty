// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Options.PathOptions
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using UnityEngine;

namespace DG.Tweening.Plugins.Options
{
  public struct PathOptions : IPlugOptions
  {
    public PathMode mode;
    public OrientType orientType;
    public AxisConstraint lockPositionAxis;
    public AxisConstraint lockRotationAxis;
    public bool isClosedPath;
    public Vector3 lookAtPosition;
    public Transform lookAtTransform;
    public float lookAhead;
    public bool hasCustomForwardDirection;
    public Quaternion forward;
    public bool useLocalPosition;
    public Transform parent;
    public bool isRigidbody;
    internal Quaternion startupRot;
    internal float startupZRot;

    public void Reset()
    {
      this.mode = PathMode.Ignore;
      this.orientType = OrientType.None;
      this.lockPositionAxis = this.lockRotationAxis = AxisConstraint.None;
      this.isClosedPath = false;
      this.lookAtPosition = Vector3.zero;
      this.lookAtTransform = (Transform) null;
      this.lookAhead = 0.0f;
      this.hasCustomForwardDirection = false;
      this.forward = Quaternion.identity;
      this.useLocalPosition = false;
      this.parent = (Transform) null;
      this.startupRot = Quaternion.identity;
      this.startupZRot = 0.0f;
    }
  }
}
