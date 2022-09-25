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
            unit.BattleGO.GetExpRenderer().onAllParticlesArrived -= AllFinished;
            unit.BattleGO.GetExpRenderer().onAllParticlesArrived += AllFinished;
        }
        else
        {
            //Do Grid Map Version
            go = Instantiate(expParticlePrefab, transform);
            unit.visuals.UnitCharacterCircleUI.GetExpRenderer().onAllParticlesArrived -= AllFinished;
            unit.visuals.UnitCharacterCircleUI.GetExpRenderer().onAllParticlesArrived += AllFinished;
        }
        var controller=go.GetComponent<DeathParticleController>();
        controller.Play(unit, startPos, exp, uiCamera);
    }

    public event Action OnFinished;

    private void AllFinished()
    {
        OnFinished?.Invoke();
    }

  
}
