using UnityEngine;

[CreateAssetMenu(menuName = "GameData/EventEncounterData", fileName = "EventEncounterData")]
public class EventEncounterNodeData: EncounterNodeData
{
    public override EncounterNode CreateNode(EncounterNode parent,int depth, int childIndex)
    {
        return new EventEncounterNode(parent, depth, childIndex,  label,description, sprite);
    }
}