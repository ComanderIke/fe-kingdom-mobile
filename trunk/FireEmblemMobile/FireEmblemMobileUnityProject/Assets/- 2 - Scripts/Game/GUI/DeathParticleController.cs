using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleController : MonoBehaviour
{
    public ParticleSystem system;

    public float interval = 0.2f;
    // Start is called before the first frame update
    private void Awake()
    {
        system.Stop();
    }

    public void Play(Vector3 startPos, int exp)
    {
        transform.position = startPos;
        //Debug.Log("expParticles:" +exp);
        
        //Debug.Log("CycleCount: "+(int)(exp/2f));
        var em = system.emission;
        em.enabled = true;
        em.SetBursts(new[] { new ParticleSystem.Burst(0.0f, 3,3,(int)(exp/3f),interval), 
            new ParticleSystem.Burst(0.1f, (short)(exp%3),(short)(exp%3),1,interval)});
        // system.emission.SetBurst(0,new ParticleSystem.Burst(0.0f, 1,1,exp,0.010f));
        // system.emission.
        system.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
