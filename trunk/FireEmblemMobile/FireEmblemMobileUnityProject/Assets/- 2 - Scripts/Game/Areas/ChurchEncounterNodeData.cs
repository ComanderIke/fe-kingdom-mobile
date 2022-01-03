using UnityEngine;

[CreateAssetMenu(menuName = "GameData/ChurchEncounterData", fileName = "ChurchEncounterData")]
public class ChurchEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent)
    {
        return new ChurchEncounterNode(parent);
    }
}