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

    public void Init()
    {

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

    //public void MoveCameraTo( int x, int y)
    //{
    //    int deltaX = (int)transform.localPosition.x - x;
    //    int deltaY = (int)transform.localPosition.y - y;
    //    int targetX = (int)transform.localPosition.x;
    //    int targetY = (int)transform.localPosition.y;
    //    if (x > (int)transform.localPosition.x + 5 || x < (int)transform.localPosition.x)
    //    {
    //        targetX = -1 * (deltaX + 5);

    //    }
    //    if (y > (int)transform.localPosition.y + 7 || y < (int)transform.localPosition.y)
    //    {
    //        targetY = -1 * (deltaY + 7);
    //    }
    //    targetPos = new Vector2(targetX, targetY);
    //}


   
}
