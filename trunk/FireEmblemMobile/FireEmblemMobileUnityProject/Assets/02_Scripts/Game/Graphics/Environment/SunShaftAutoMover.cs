using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteInEditMode]
public class SunShaftAutoMover : MonoBehaviour
{
    public float moveSpeed;

    public Vector2 maxMovement;

  

    public new Light2D light;
    public GameObject leftGO;
    public GameObject rightGO;
    public Vector3 startPos;
   
    // Start is called before the first frame update
    void Start()
    {
     //   startPos = transform.position;
       
    }

    public float offset = 2;

    private void OnEnable()
    {
        if (leftGO != null && rightGO != null)
        {
            calculated = false;
            transform.localPosition = startPos;
            CalculateEdges();
            leftGO.transform.localPosition = new Vector3(-(distanceBetweenEdges.x - offset), 0, 0);
            rightGO.transform.localPosition = new Vector3(distanceBetweenEdges.x - offset, 0, 0);
        }
    }

    // Update is called once per frame
    private float time = 0;
    void Update()
    {
        if(!calculated)
            CalculateEdges();

        transform.position += Time.deltaTime * moveSpeed * Vector3.right;
        if (time >= 1)
        {
            time = 0;
        }
        if(PassedEdge())
        {
            MoveRightSpriteToOppositeSide();
        }
       
    }

    private void MoveRightSpriteToOppositeSide()
    {
        if (moveSpeed > 0)
        {
            transform.position -= distanceBetweenEdges-new Vector3(offset,0,0);
        }
        else
        {
            transform.position += distanceBetweenEdges-new Vector3(offset,0,0);
        }
    }

    private bool PassedEdge()
    {
        return moveSpeed > 0 && transform.position.x > rightEdge || moveSpeed < 0 && transform.position.x < leftEdge;
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawCube(new Vector3(transform.position.x,transform.position.y,0), new Vector3(1f,1f,1f));
        // Gizmos.DrawCube(new Vector3(rightEdge,transform.position.y,0), new Vector3(0.1f,2f,0.1f));
        // Gizmos.DrawCube(new Vector3(leftEdge,transform.position.y,0), new Vector3(0.1f,2f,0.1f));
    }

    private bool calculated = false;

    private float leftEdge;
    private float rightEdge;
    private Vector3 distanceBetweenEdges;
    private void CalculateEdges()
    {  
        if (light != null)
        {
            calculated = true;
      
            rightEdge = transform.position.x + light.lightCookieSprite.bounds.extents.x / 1f;
            leftEdge = transform.position.x - light.lightCookieSprite.bounds.extents.x / 1f;
            distanceBetweenEdges = new Vector3(rightEdge - leftEdge, 0, 0);
        }
    }
}
