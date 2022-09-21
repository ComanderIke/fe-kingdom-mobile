using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SmithyEncounterData", fileName = "SmithyEncounterData")]
public class SmithyEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent,int depth, int childIndex)
    {
        return new SmithyEncounterNode(parent, depth, childIndex,  label,description, sprite);
    }
}