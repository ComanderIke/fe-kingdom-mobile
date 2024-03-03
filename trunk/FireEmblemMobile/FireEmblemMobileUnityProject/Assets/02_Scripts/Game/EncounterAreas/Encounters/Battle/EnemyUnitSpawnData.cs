using System;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.EncounterAreas.Encounters.Battle
{
    [Serializable]
    public class EnemyUnitSpawnData
    {
        [FormerlySerializedAs("unit")] public UnitBP unitBp;
        public Vector2 spawnPosition;
    }
}