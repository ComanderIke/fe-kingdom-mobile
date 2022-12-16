using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/StartEncounterData", fileName = "StartEncounterData")]
public class StartEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex)
    {
        return new StartEncounterNode();
    }
}