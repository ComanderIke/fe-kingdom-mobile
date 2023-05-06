using Game.WorldMapStuff.Model;
using UnityEngine;

public class StartEncounterNode : EncounterNode
{
    public StartEncounterNode() : base(null,0,0, "Home base","", null)
    {

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<AreaGameManager>().Continue();
       
    }
}