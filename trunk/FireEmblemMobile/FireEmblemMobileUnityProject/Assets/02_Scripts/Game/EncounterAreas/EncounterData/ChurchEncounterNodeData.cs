using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/ChurchEncounterData", fileName = "ChurchEncounterData")]
public class ChurchEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
    {
        return new ChurchEncounterNode(parents, depth, childIndex, label,description, sprite);
    }
}