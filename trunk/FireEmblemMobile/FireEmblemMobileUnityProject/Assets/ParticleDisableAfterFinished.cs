using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisableAfterFinished : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
    }

   
}
