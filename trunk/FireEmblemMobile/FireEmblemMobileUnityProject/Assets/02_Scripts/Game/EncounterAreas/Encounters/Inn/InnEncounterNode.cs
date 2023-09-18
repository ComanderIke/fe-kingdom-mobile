using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameResources;
using Game.GUI.EncounterUI.Inn;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class InnEncounterNode : EncounterNode
{
    

    public InnEncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
    {

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIInnController>().Show(this, party);
       
    }
}