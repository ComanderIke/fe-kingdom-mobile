using System.Collections.Generic;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;


public class EventEncounterNode : EncounterNode
{
    public RandomEvent randomEvent;
   
    public EventEncounterNode(EncounterNode parent,int depth, int childIndex, string description, Sprite sprite) : base(parent, depth, childIndex, description, sprite)
    {
        randomEvent = GameData.Instance.GetEventData().GetRandomEvent(0);
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIEventController>().Show(this,party);
        Debug.Log("Activate EventEncounterNode");
        
    }
}