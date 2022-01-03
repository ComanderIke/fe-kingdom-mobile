using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SmithyEncounterData", fileName = "SmithyEncounterData")]
public class SmithyEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent)
    {
        return new SmithyEncounterNode(parent);
    }
}