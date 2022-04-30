using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMirror : MonoBehaviour
{
    public GameObject mirrorGO;

    public bool flipRotation = false;

    private ParticleSystem ps;
    // Start is called before the first frame update
    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!mirrorGO)
            return;
        if (!flipRotation)
        {


            if (mirrorGO.transform.localScale.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale =
                    new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else if (mirrorGO.transform.localScale.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale =
                    new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            if (mirrorGO.transform.localScale.x < 0 && ps.main.flipRotation ==0)
            {
                ParticleSystem.MainModule mainModule = ps.main;
                mainModule.flipRotation = 1;
            }
            else if (mirrorGO.transform.localScale.x > 0 && ps.main.flipRotation== 1)
            {
                ParticleSystem.MainModule mainModule = ps.main;
                mainModule.flipRotation = 0;
            }
        }
    }
}
