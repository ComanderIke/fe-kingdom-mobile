using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SmithyEncounterData", fileName = "SmithyEncounterData")]
public class SmithyEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
    {
        return new SmithyEncounterNode(parents, depth, childIndex,  label,description, sprite);
    }
}