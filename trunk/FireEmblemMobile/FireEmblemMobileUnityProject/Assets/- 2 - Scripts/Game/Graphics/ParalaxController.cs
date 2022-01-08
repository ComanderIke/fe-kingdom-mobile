using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public new Transform camera;
    private Vector3 lastCameraPos;
    private Vector3 cameraStartPos;
    private Vector3 transformStartPos;
    public float drag = 0.5f;

    void Start()
    {
        camera = GameObject.FindWithTag("BattleCamera").transform;
        lastCameraPos = camera.position;
        cameraStartPos = lastCameraPos;
        transformStartPos = transform.position;
    }

    // Update is called once per frame
    private Vector3 newPos;
    private Vector3 sumnewPos;
    private float velo;
    

    void LateUpdate()
     {
         Vector3 deltaPos = camera.position - lastCameraPos;
         sumnewPos += deltaPos;

         newPos = sumnewPos*speed;
        // Debug.Log("NewPos. "+newPos);
         //velo = Time.deltaTime;
         transform.position=new Vector3(Mathf.SmoothDamp(transform.position.x, transformStartPos.x+newPos.x,ref velo, drag),transform.position.y,transform.position.z);
        // Debug.Log(velo);
         lastCameraPos = camera.position;
     }
}
