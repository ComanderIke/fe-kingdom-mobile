using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Areas/EnemyArmyDataMapPool", fileName="EnemyArmyDataMapPool")]
public class EnemyArmyDataMapPool : EnemyArmyData
{
    public BattleMapPool battleMapPool;
    public override BattleMap GetBattleMap()
    {
        return battleMapPool.GetRandomMap();
    }
}