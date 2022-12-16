using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BattleEncounterData", fileName = "BattleEncounterData")]
public class BattleEncounterNodeData: EncounterNodeData
{

    public Scenes levelIndex;
    public EnemyArmyData EnemyArmyData;

    public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
    {
        return new BattleEncounterNode(levelIndex, EnemyArmyData, parents, depth, childIndex, label,description, sprite);
    }
}