using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour {
     
    [SerializeField]
    private RectTransform _attractorTransform;
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[100];

    public static event Action onParticleArrived;

    public Camera uicamera;
    //private float speed = 1;
 
    public void Start ()
    {
        _particleSystem = GetComponent<ParticleSystem> ();
      
    }

    public float offset = 0.1f;
    public void LateUpdate()
    {
        if (_particleSystem.isPlaying) {
            int length = _particleSystem.GetParticles (_particles);
            Vector3 pos = _attractorTransform.transform.position;
            pos = new Vector3(pos.x, pos.y, 0);
            Vector3 attractorPosition = pos;//uicamera.ScreenToWorldPoint(pos);
           // Debug.Log(attractorPosition+" "+pos+" "+_attractorTransform.transform.position);
          
            for (int i=0; i < length; i++) {
                _particles [i].position = _particles [i].position + (attractorPosition - _particles [i].position) / (_particles [i].remainingLifetime) * Time.deltaTime;
                if (_particles[i].position.x+offset >= attractorPosition.x && _particles[i].position.x -offset<=attractorPosition.x
                                                                           &&_particles[i].position.y+offset >= attractorPosition.y&& _particles[i].position.y-offset <=attractorPosition.y)
                {
                    //Debug.Log("PosReached!");
                    //Debug.Log(_particles[i].position.x+" "+attractorPosition.x+ " "+_particles [i].remainingLifetime);
                    _particles[i].remainingLifetime = 0;
                    onParticleArrived?.Invoke();
                    
                }

               
            }

            
            _particleSystem.SetParticles (_particles, length);
        }
 
    }
}