using UnityEngine;

public class SmithyEncounterNode : EncounterNode
{
    public SmithyEncounterNode(EncounterNode parent) : base(parent)
    {

    }

    public override void Activate()
    {
        AreaGameManager.Instance.Continue();
        Debug.Log("Activate SmithyEncounterNode");
    }
}