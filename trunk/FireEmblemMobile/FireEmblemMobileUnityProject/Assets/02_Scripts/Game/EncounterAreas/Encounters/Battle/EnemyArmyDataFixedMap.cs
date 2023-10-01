using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Areas/EnemyArmyDataFixedMap", fileName="EnemyArmyDataFixedMap")]
public class EnemyArmyDataFixedMap : EnemyArmyData
{
    public BattleMap battleMap;
    public override BattleMap GetBattleMap()
    {
        return battleMap;
    }
}