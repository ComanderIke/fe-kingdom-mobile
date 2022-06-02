using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.States;
using GameEngine;
using UnityEngine;
using Utility;

public class ExpParticleSystem : MonoBehaviour, IAnimation
{
    public GameObject ExpParticlePrefab;

    public Camera uiCamera;
    public void Play(Unit unit, Vector3 startPos, int exp)
    {
        Debug.Log("play Exp Particly");
        var go = Instantiate(ExpParticlePrefab, transform);
        var controller=go.GetComponent<DeathParticleController>();
        controller.Play(unit, startPos, exp, uiCamera);
    }

    public void AllFinished()
    {
        
        AnimationQueue.OnAnimationEnded?.Invoke();
    }

    public void Play()
    {
        
    }
}
