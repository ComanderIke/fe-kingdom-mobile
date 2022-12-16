using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/InnEncounterData", fileName = "InnEncounterData")]
public class InnEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
    {
        return new InnEncounterNode(parents, depth, childIndex,  label,description, sprite);
    }
}