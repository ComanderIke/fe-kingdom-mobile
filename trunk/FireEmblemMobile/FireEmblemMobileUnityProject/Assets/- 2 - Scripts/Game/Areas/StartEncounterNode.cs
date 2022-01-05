using UnityEngine;

public class StartEncounterNode : EncounterNode
{
    public StartEncounterNode() : base(null)
    {

    }

    public override void Activate()
    {
        AreaGameManager.Instance.Continue();
        Debug.Log("Activate StartEncounterNode");
    }
}