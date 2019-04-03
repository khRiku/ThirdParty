// Decompiled with JetBrains decompiler
// Type: DG.DOTweenEditor.DelayedCall
// Assembly: DOTweenEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5921EEE1-1EAD-4AE3-AEC3-8051606D5E53
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\Editor\DOTweenEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
  public class DelayedCall
  {
    public float delay;
    public Action callback;
    private float _startupTime;

    public DelayedCall(float delay, Action callback)
    {
      this.delay = delay;
      this.callback = callback;
      this._startupTime = Time.realtimeSinceStartup;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
    }

    private void Update()
    {
      if ((double) Time.realtimeSinceStartup - (double) this._startupTime < (double) this.delay)
        return;
      if (EditorApplication.update != null)
        EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      if (this.callback == null)
        return;
      this.callback();
    }
  }
}
