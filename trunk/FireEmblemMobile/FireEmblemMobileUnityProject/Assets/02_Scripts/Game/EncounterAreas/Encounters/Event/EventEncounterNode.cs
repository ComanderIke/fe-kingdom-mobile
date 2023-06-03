using System.Collections.Generic;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;


public class EventEncounterNode : EncounterNode
{
    public LGEventDialogSO randomEvent;
   
    public EventEncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
    {
        randomEvent = GenerateEvent();
    }

    LGEventDialogSO  GenerateEvent()
    {
        return GameBPData.Instance.GetEventData().GetRandomEvent(2);
    }
    public EventEncounterNode(string prefabName, List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
    {
        randomEvent = GameBPData.Instance.GetEventData().GetEventById(prefabName);
        if(randomEvent==null)
            randomEvent = GenerateEvent();
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIEventController>().Show(this,party);
        
        
    }
}