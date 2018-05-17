using Assets.Scripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DragCameraMixin : CameraMixin
{
    const float DRAG_SPEED = 0.2f;
    private Vector3 dragOrigin;
    private bool drag = false;
    private Vector3 lastPosition;


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider != null)
                if (hit.collider.gameObject.tag == "Grid")
                {
                    lastPosition = Input.mousePosition;
                    drag = true;
                    cameraSystem.DeactivateOtherMixins(this);
                }
        }

        if (Input.GetMouseButton(0) && drag)
        {

            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * Time.deltaTime * DRAG_SPEED, -delta.y * Time.deltaTime * DRAG_SPEED, 0);
            lastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //lerpTime = 0;
            cameraSystem.ActivateMixins();
            drag = false;
        }
    }


}
