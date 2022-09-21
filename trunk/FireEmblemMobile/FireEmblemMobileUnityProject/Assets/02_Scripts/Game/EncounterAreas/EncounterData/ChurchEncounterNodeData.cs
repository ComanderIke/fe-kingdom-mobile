using UnityEngine;

[CreateAssetMenu(menuName = "GameData/ChurchEncounterData", fileName = "ChurchEncounterData")]
public class ChurchEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent,int depth, int childIndex)
    {
        return new ChurchEncounterNode(parent, depth, childIndex, label,description, sprite);
    }
}