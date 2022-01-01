using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Areas/EnemyArmyData", fileName="EnemyArmyData")]
public class EnemyArmyData : ScriptableObject
{
    public List<EnemyUnitSpawnData> units;
}