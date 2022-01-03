using Game.WorldMapStuff.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BattleEncounterData", fileName = "BattleEncounterData")]
public class BattleEncounterNodeData: EncounterNodeData
{

    public Scenes levelIndex;
    public EnemyArmyData EnemyArmyData;

    public override EncounterNode CreateNode(EncounterNode parent)
    {
        return new BattleEncounterNode(levelIndex, EnemyArmyData, parent);
    }
}