using UnityEngine;
using Assets.Scripts.Engine;
using System.Collections.Generic;
using System;

public class CameraSystem : MonoBehaviour, EngineSystem {

    public delegate void MoveToFinishedEvent();
    public static MoveToFinishedEvent moveToFinishedEvent;

	public float speed;
    private Vector3 lastPosition;
    private Vector3 dragOrigin;
    private Vector3 oldPos;
   // private Vector3 targetPosition;
    private Transform target;
    private List<CameraMixin> mixins;

    void Start () {
        mixins = new List<CameraMixin>();
    }

    public T AddMixin<T>()
    {

        if (!typeof(CameraMixin).IsAssignableFrom(typeof(T)))
        {
            throw new Exception("Parameter was not of type CameraMixin");
        }
        CameraMixin mixin = (CameraMixin)gameObject.AddComponent(typeof(T));
        mixins.Add(mixin);
        return (T)Convert.ChangeType(mixin, typeof(T));
    }
    public void RemoveMixin<T>()
    {
        CameraMixin mixin = (CameraMixin)GetComponent(typeof(T));
        mixins.Remove(mixin);
        Destroy(GetComponent(mixin.GetType()));
    }
	public void DeactivateOtherMixins(CameraMixin mixin)
    {
        foreach(CameraMixin m in mixins)
        {

            if (m == mixin||m.IsLocked())
            {
                continue;
            }
            else
            {
                m.enabled = false;
            }
        }
    }
    public void ActivateMixins()
    {
        foreach (CameraMixin m in mixins)
        {
                m.enabled = true;
        }
    }
}
