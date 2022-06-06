using Game.WorldMapStuff.Model;
using UnityEngine;

public class StartEncounterNode : EncounterNode
{
    public StartEncounterNode() : base(null,0,0, "", null)
    {

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<AreaGameManager>().Continue();
        Debug.Log("Activate StartEncounterNode");
    }
}