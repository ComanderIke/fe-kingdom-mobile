using UnityEngine;

public class ChurchEncounterNode : EncounterNode
{
    public ChurchEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        AreaGameManager.Instance.Continue();
        Debug.Log("Activate ChurchEncounterNode");
    }
}