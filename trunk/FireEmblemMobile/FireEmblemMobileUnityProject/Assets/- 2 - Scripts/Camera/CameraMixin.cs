using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(CameraSystem))]
public abstract class CameraMixin : MonoBehaviour
{
    protected CameraSystem cameraSystem;
    private bool locked;
    private void Start()
    {
        cameraSystem = MainScript.instance.GetSystem<CameraSystem>();
    }
    public bool IsLocked()
    {
        return locked;
    }
    public CameraMixin Locked(bool value)
    {
        locked = value;
        return this;
    }

}

