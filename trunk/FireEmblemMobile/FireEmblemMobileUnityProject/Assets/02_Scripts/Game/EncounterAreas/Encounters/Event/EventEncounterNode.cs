using System.Collections.Generic;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;


public class EventEncounterNode : EncounterNode
{
    public RandomEvent randomEvent;
   
    public EventEncounterNode(EncounterNode parent,int depth, int childIndex, string label, string description, Sprite sprite) : base(parent, depth, childIndex, label, description, sprite)
    {
        randomEvent = GameData.Instance.GetEventData().GetRandomEvent(1);
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIEventController>().Show(this,party);
        Debug.Log("Activate EventEncounterNode");
        
    }
}