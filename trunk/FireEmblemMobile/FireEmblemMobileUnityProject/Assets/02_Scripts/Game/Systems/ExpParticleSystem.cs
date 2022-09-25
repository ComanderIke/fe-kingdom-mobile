using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Mechanics;
using Game.States;
using GameEngine;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

public class ExpParticleSystem : MonoBehaviour, IExpRenderer
{
    [FormerlySerializedAs("ExpParticlePrefab")] public GameObject expParticlePrefab;
    [FormerlySerializedAs("ExpParticlePrefabBattleVersion")] public GameObject expParticlePrefabBattleVersion;

    public Camera uiCamera;

 

    public void Play(Unit unit, Vector3 startPos, int exp)
    {
        // var expRenderer = ((Unit)attacker).visuals.UnitCharacterCircleUI.GetExpRenderer();
        // expRenderer.UpdateAnimated(attacker.ExperienceManager.GetMaxEXP(exp));
        GameObject go;
        if (unit.BattleGO != null)
        {
            //Do Battle Animation Version
            go = Instantiate(expParticlePrefabBattleVersion, transform);
            var controller=go.GetComponent<DeathParticleController>();
            controller.OnParticleArrived -= unit.BattleGO.GetExpRenderer().ParticleArrived;
            controller.OnParticleArrived += unit.BattleGO.GetExpRenderer().ParticleArrived;
            controller.Play(unit, startPos, exp, uiCamera);
            ;
            unit.BattleGO.GetExpRenderer().onAllParticlesArrived -= AllFinished;
            unit.BattleGO.GetExpRenderer().onAllParticlesArrived += AllFinished;
        }
        else
        {
            //Do Grid Map Version
            go = Instantiate(expParticlePrefab, transform);
            var controller=go.GetComponent<DeathParticleController>();
            controller.Play(unit, startPos, exp, uiCamera);
            controller.OnParticleArrived -=  unit.visuals.UnitCharacterCircleUI.GetExpRenderer().ParticleArrived;
            controller.OnParticleArrived +=  unit.visuals.UnitCharacterCircleUI.GetExpRenderer().ParticleArrived;
            unit.visuals.UnitCharacterCircleUI.GetExpRenderer().onAllParticlesArrived -= AllFinished;
            unit.visuals.UnitCharacterCircleUI.GetExpRenderer().onAllParticlesArrived += AllFinished;
        }

    }
    
    public event Action OnFinished;

    private void AllFinished()
    {
        Debug.Log("all Particles Arrived!");
        OnFinished?.Invoke();
    }

  
}
