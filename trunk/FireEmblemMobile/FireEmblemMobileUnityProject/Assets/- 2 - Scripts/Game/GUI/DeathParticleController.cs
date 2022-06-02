using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

public class DeathParticleController : MonoBehaviour
{
    public ParticleSystem system;
    public ParticleAttractor attractor;
    public float interval = 0.2f;
    // Start is called before the first frame update
    private void Awake()
    {
        system.Stop();
    }

    public void Play(Unit unit, Vector3 startPos, int exp, Camera uiCamera)
    {
        attractor.SetAttractorUnit(unit, uiCamera);
        transform.position = startPos;
        Debug.Log("expParticles:" +exp+ "AttractorUnit: "+unit);
        
        //Debug.Log("CycleCount: "+(int)(exp/2f));
        var em = system.emission;
        em.enabled = true;
        em.SetBursts(new[] { new ParticleSystem.Burst(0.0f, (short)exp,(short)exp,1,interval)});
        // system.emission.SetBurst(0,new ParticleSystem.Burst(0.0f, 1,1,exp,0.010f));
        // system.emission.
        system.Play();
        Debug.Log("Play P-System!");
        Destroy(this.gameObject, 2.0f);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
