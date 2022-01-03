using UnityEngine;

[CreateAssetMenu(menuName = "GameData/EventEncounterData", fileName = "EventEncounterData")]
public class EventEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent)
    {
        return new EventEncounterNode(parent);
    }
}