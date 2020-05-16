using UnityEngine;
using UnityEditor;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class PreJitAttribute : Attribute
{
    public PreJitAttribute()
    {

    }
}