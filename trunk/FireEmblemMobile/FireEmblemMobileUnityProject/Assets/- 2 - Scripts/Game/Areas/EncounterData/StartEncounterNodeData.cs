using UnityEngine;

[CreateAssetMenu(menuName = "GameData/StartEncounterData", fileName = "StartEncounterData")]
public class StartEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent)
    {
        return new StartEncounterNode();
    }
}