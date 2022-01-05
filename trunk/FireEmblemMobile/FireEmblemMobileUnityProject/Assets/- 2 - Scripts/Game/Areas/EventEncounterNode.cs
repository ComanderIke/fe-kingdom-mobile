using UnityEngine;

public class EventEncounterNode : EncounterNode
{
    public EventEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        AreaGameManager.Instance.Continue();
        Debug.Log("Activate EventEncounterNode");
    }
}