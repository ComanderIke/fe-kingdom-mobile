using System;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnemyUnitSpawnData
{
    [FormerlySerializedAs("unit")] public UnitBP unitBp;
    public Vector2 spawnPosition;
}