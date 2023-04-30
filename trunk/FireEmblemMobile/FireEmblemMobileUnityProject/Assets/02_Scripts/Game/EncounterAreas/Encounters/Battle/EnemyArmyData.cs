using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Areas/EnemyArmyData", fileName="EnemyArmyData")]
public class EnemyArmyData : ScriptableObject
{
    public BattleMapPool battleMapPool;
    public int level = 1;//Average Level of enemies
    public bool isBoss;
}