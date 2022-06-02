using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GUI;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour {
     
    [SerializeField]
    private RectTransform _attractorTransform;
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[100];
    public GameObject CubePrefab;

    private ExpBarController expController;
    public static event Action onParticleArrived;

    private Camera uicamera;
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
            Vector3 pos = _attractorTransform.position;
            Vector3 attractorPosition = uicamera.ScreenToWorldPoint(pos);
            attractorPosition.z = 0;
          //  Debug.Log(attractorPosition+" "+pos+" "+_attractorTransform.transform.position);
          
            for (int i=0; i < length; i++) {
                _particles [i].position = _particles [i].position + (attractorPosition - _particles [i].position) / (_particles [i].remainingLifetime) * Time.deltaTime;
                if (_particles[i].position.x+offset >= attractorPosition.x && _particles[i].position.x -offset<=attractorPosition.x
                                                                           &&_particles[i].position.y+offset >= attractorPosition.y&& _particles[i].position.y-offset <=attractorPosition.y)
                {
                   // Debug.Log("PosReached!");
                    //Debug.Log(_particles[i].position.x+" "+attractorPosition.x+ " "+_particles [i].remainingLifetime);
                    _particles[i].remainingLifetime = 0;
                    expController.ParticleArrived();

                }
            
               
            }
            
            
            _particleSystem.SetParticles (_particles, length);
        }
 
    }

    // private void OnDrawGizmos()
    // {
    //     if (_attractorTransform.transform.position != null)
    //     {
    //         
    //         Gizmos.DrawCube(_attractorTransform.transform.position, new Vector3(0.2f,0.2f,0.2f));
    //         Gizmos.DrawCube(uicamera.ScreenToWorldPoint(_attractorTransform.position), new Vector3(0.2f,0.2f,0.2f));
    //     }
    //        
    // }

    public void SetAttractorUnit(Unit unit, Camera uiCamera)
    {
        this.uicamera = uiCamera;
        IParticleAttractorTransformProvider provider =
            FindObjectsOfType<MonoBehaviour>().OfType<IParticleAttractorTransformProvider>().First();
        expController = unit.visuals.UnitCharacterCircleUI.GetExpRenderer();
        _attractorTransform = provider.GetUnitParticleAttractorTransform(unit);
        // var go = Instantiate(CubePrefab);
        // go.name = "AttractorPosition!";
        // Vector3 pos = uicamera.ScreenToWorldPoint(_attractorTransform.transform.position);
        // pos.z = 0;
        // go.transform.position =pos;
    }
}