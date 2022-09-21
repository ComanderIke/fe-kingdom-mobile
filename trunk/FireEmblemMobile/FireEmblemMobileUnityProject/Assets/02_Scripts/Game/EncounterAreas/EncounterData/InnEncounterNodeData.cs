using UnityEngine;

[CreateAssetMenu(menuName = "GameData/InnEncounterData", fileName = "InnEncounterData")]
public class InnEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent,int depth, int childIndex)
    {
        return new InnEncounterNode(parent, depth, childIndex,  label,description, sprite);
    }
}