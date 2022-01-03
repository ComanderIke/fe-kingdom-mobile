using UnityEngine;

[CreateAssetMenu(menuName = "GameData/InnEncounterData", fileName = "InnEncounterData")]
public class InnEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent)
    {
        return new InnEncounterNode(parent);
    }
}