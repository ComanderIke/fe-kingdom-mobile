using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GUI;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour {
    
    
    [SerializeField] private RectTransform _attractorTransform;
    [SerializeField] private bool CalculateVersion2 = false;
    [SerializeField] float offset = 0.1f;
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[100];

    public event Action ParticleArrived;
    private Camera uicamera;
    public void Start ()
    {
        _particleSystem = GetComponent<ParticleSystem> ();
    }

    public void LateUpdate()
    {
        if (_particleSystem.isPlaying) {
            int length = _particleSystem.GetParticles (_particles);
            Vector3 pos = _attractorTransform.position;
            Vector3 attractorPosition;
            
            if (CalculateVersion2)
                attractorPosition = _attractorTransform.transform.position;
            else
            {
                attractorPosition = uicamera.ScreenToWorldPoint(pos);
                attractorPosition.z = 0;
            }
           // Debug.Log(attractorPosition+" "+pos+" "+_attractorTransform.transform.position);
            for (int i=0; i < length; i++) {
                _particles [i].position = _particles [i].position + (attractorPosition - _particles [i].position) / (_particles [i].remainingLifetime) * Time.deltaTime;
                if (_particles[i].position.x+offset >= attractorPosition.x && _particles[i].position.x -offset<=attractorPosition.x
                                                                           &&_particles[i].position.y+offset >= attractorPosition.y&& _particles[i].position.y-offset <=attractorPosition.y)
                {
                    _particles[i].remainingLifetime = 0;
                    ParticleArrived?.Invoke();
                    
                }
            }
            _particleSystem.SetParticles (_particles, length);
        }
 
    }

    public void SetAttractorUnit(Unit unit, Camera uiCamera)
    {
        uicamera = uiCamera;
        // IParticleAttractorTransformProvider provider =
        //     FindObjectsOfType<MonoBehaviour>().OfType<IParticleAttractorTransformProvider>().First();
        //expController = unit.visuals.UnitCharacterCircleUI.GetExpRenderer();
     
        Debug.Log("TODO MapBattleAnimations Do it the old way?");
        _attractorTransform =unit.BattleGO.GetAttractorTransform();// provider.GetUnitParticleAttractorTransform(unit);

    }
}