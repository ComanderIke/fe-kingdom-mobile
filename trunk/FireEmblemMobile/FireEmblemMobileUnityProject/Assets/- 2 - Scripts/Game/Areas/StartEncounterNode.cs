using UnityEngine;

public class StartEncounterNode : EncounterNode
{
    public StartEncounterNode() : base(null)
    {

    }

    public override void Activate()
    {
        GameObject.FindObjectOfType<AreaGameManager>().Continue();
        Debug.Log("Activate StartEncounterNode");
    }
}