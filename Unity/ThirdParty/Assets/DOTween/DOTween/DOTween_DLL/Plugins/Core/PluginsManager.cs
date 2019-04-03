// Decompiled with JetBrains decompiler
// Type: DG.Tweening.Plugins.Core.PluginsManager
// Assembly: DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D19E8A38-5444-4F3D-A5A4-C530527191EF
// Assembly location: F:\Project\github\ThirdParty\Unity\ThirdParty\Assets\Demigiant\DOTween\DOTween.dll

using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
  internal static class PluginsManager
  {
    private static ITweenPlugin _floatPlugin;
    private static ITweenPlugin _doublePlugin;
    private static ITweenPlugin _intPlugin;
    private static ITweenPlugin _uintPlugin;
    private static ITweenPlugin _longPlugin;
    private static ITweenPlugin _ulongPlugin;
    private static ITweenPlugin _vector2Plugin;
    private static ITweenPlugin _vector3Plugin;
    private static ITweenPlugin _vector4Plugin;
    private static ITweenPlugin _quaternionPlugin;
    private static ITweenPlugin _colorPlugin;
    private static ITweenPlugin _rectPlugin;
    private static ITweenPlugin _rectOffsetPlugin;
    private static ITweenPlugin _stringPlugin;
    private static ITweenPlugin _vector3ArrayPlugin;
    private static ITweenPlugin _color2Plugin;
    private const int _MaxCustomPlugins = 20;
    private static Dictionary<System.Type, ITweenPlugin> _customPlugins;

    internal static ABSTweenPlugin<T1, T2, TPlugOptions> GetDefaultPlugin<T1, T2, TPlugOptions>() where TPlugOptions : struct, IPlugOptions
    {
      System.Type type1 = typeof (T1);
      System.Type type2 = typeof (T2);
      ITweenPlugin tweenPlugin = (ITweenPlugin) null;
      if (type1 == typeof (Vector3) && type1 == type2)
      {
        if (PluginsManager._vector3Plugin == null)
          PluginsManager._vector3Plugin = (ITweenPlugin) new Vector3Plugin();
        tweenPlugin = PluginsManager._vector3Plugin;
      }
      else if (type1 == typeof (Vector3) && type2 == typeof (Vector3[]))
      {
        if (PluginsManager._vector3ArrayPlugin == null)
          PluginsManager._vector3ArrayPlugin = (ITweenPlugin) new Vector3ArrayPlugin();
        tweenPlugin = PluginsManager._vector3ArrayPlugin;
      }
      else if (type1 == typeof (Quaternion))
      {
        if (type2 == typeof (Quaternion))
        {
          Debugger.LogError((object) "Quaternion tweens require a Vector3 endValue");
        }
        else
        {
          if (PluginsManager._quaternionPlugin == null)
            PluginsManager._quaternionPlugin = (ITweenPlugin) new QuaternionPlugin();
          tweenPlugin = PluginsManager._quaternionPlugin;
        }
      }
      else if (type1 == typeof (Vector2))
      {
        if (PluginsManager._vector2Plugin == null)
          PluginsManager._vector2Plugin = (ITweenPlugin) new Vector2Plugin();
        tweenPlugin = PluginsManager._vector2Plugin;
      }
      else if (type1 == typeof (float))
      {
        if (PluginsManager._floatPlugin == null)
          PluginsManager._floatPlugin = (ITweenPlugin) new FloatPlugin();
        tweenPlugin = PluginsManager._floatPlugin;
      }
      else if (type1 == typeof (Color))
      {
        if (PluginsManager._colorPlugin == null)
          PluginsManager._colorPlugin = (ITweenPlugin) new ColorPlugin();
        tweenPlugin = PluginsManager._colorPlugin;
      }
      else if (type1 == typeof (int))
      {
        if (PluginsManager._intPlugin == null)
          PluginsManager._intPlugin = (ITweenPlugin) new IntPlugin();
        tweenPlugin = PluginsManager._intPlugin;
      }
      else if (type1 == typeof (Vector4))
      {
        if (PluginsManager._vector4Plugin == null)
          PluginsManager._vector4Plugin = (ITweenPlugin) new Vector4Plugin();
        tweenPlugin = PluginsManager._vector4Plugin;
      }
      else if (type1 == typeof (Rect))
      {
        if (PluginsManager._rectPlugin == null)
          PluginsManager._rectPlugin = (ITweenPlugin) new RectPlugin();
        tweenPlugin = PluginsManager._rectPlugin;
      }
      else if (type1 == typeof (RectOffset))
      {
        if (PluginsManager._rectOffsetPlugin == null)
          PluginsManager._rectOffsetPlugin = (ITweenPlugin) new RectOffsetPlugin();
        tweenPlugin = PluginsManager._rectOffsetPlugin;
      }
      else if (type1 == typeof (uint))
      {
        if (PluginsManager._uintPlugin == null)
          PluginsManager._uintPlugin = (ITweenPlugin) new UintPlugin();
        tweenPlugin = PluginsManager._uintPlugin;
      }
      else if (type1 == typeof (string))
      {
        if (PluginsManager._stringPlugin == null)
          PluginsManager._stringPlugin = (ITweenPlugin) new StringPlugin();
        tweenPlugin = PluginsManager._stringPlugin;
      }
      else if (type1 == typeof (Color2))
      {
        if (PluginsManager._color2Plugin == null)
          PluginsManager._color2Plugin = (ITweenPlugin) new Color2Plugin();
        tweenPlugin = PluginsManager._color2Plugin;
      }
      else if (type1 == typeof (long))
      {
        if (PluginsManager._longPlugin == null)
          PluginsManager._longPlugin = (ITweenPlugin) new LongPlugin();
        tweenPlugin = PluginsManager._longPlugin;
      }
      else if (type1 == typeof (ulong))
      {
        if (PluginsManager._ulongPlugin == null)
          PluginsManager._ulongPlugin = (ITweenPlugin) new UlongPlugin();
        tweenPlugin = PluginsManager._ulongPlugin;
      }
      else if (type1 == typeof (double))
      {
        if (PluginsManager._doublePlugin == null)
          PluginsManager._doublePlugin = (ITweenPlugin) new DoublePlugin();
        tweenPlugin = PluginsManager._doublePlugin;
      }
      if (tweenPlugin != null)
        return tweenPlugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
      return (ABSTweenPlugin<T1, T2, TPlugOptions>) null;
    }

    public static ABSTweenPlugin<T1, T2, TPlugOptions> GetCustomPlugin<TPlugin, T1, T2, TPlugOptions>() where TPlugin : ITweenPlugin, new() where TPlugOptions : struct, IPlugOptions
    {
      System.Type key = typeof (TPlugin);
      ITweenPlugin instance;
      if (PluginsManager._customPlugins == null)
        PluginsManager._customPlugins = new Dictionary<System.Type, ITweenPlugin>(20);
      else if (PluginsManager._customPlugins.TryGetValue(key, out instance))
        return instance as ABSTweenPlugin<T1, T2, TPlugOptions>;
      instance = (ITweenPlugin) Activator.CreateInstance<TPlugin>();
      PluginsManager._customPlugins.Add(key, instance);
      return instance as ABSTweenPlugin<T1, T2, TPlugOptions>;
    }

    internal static void PurgeAll()
    {
      PluginsManager._floatPlugin = (ITweenPlugin) null;
      PluginsManager._intPlugin = (ITweenPlugin) null;
      PluginsManager._uintPlugin = (ITweenPlugin) null;
      PluginsManager._longPlugin = (ITweenPlugin) null;
      PluginsManager._ulongPlugin = (ITweenPlugin) null;
      PluginsManager._vector2Plugin = (ITweenPlugin) null;
      PluginsManager._vector3Plugin = (ITweenPlugin) null;
      PluginsManager._vector4Plugin = (ITweenPlugin) null;
      PluginsManager._quaternionPlugin = (ITweenPlugin) null;
      PluginsManager._colorPlugin = (ITweenPlugin) null;
      PluginsManager._rectPlugin = (ITweenPlugin) null;
      PluginsManager._rectOffsetPlugin = (ITweenPlugin) null;
      PluginsManager._stringPlugin = (ITweenPlugin) null;
      PluginsManager._vector3ArrayPlugin = (ITweenPlugin) null;
      PluginsManager._color2Plugin = (ITweenPlugin) null;
      if (PluginsManager._customPlugins == null)
        return;
      PluginsManager._customPlugins.Clear();
    }
  }
}
