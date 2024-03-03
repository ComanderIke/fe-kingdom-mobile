using System.Collections.Generic;
using Game.DataAndReferences.Data;
using Game.Dialog.DialogSystem;
using Game.EncounterAreas.Model;
using Game.GUI.EncounterUI.Event;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Event
{
    public class EventEncounterNode : EncounterNode
    {
        public LGEventDialogSO randomEvent;
   
        public EventEncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
        {
            randomEvent = GenerateEvent();
        }

        LGEventDialogSO  GenerateEvent()
        {
            return GameBPData.Instance.GetEventData().GetRandomEvent();
        }
        public EventEncounterNode(string prefabName, List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
        {
            randomEvent = GameBPData.Instance.GetEventData().GetEventById(prefabName);
            if(randomEvent==null)
                randomEvent = GenerateEvent();
        }

        public override void Activate(Party party)
        {
            MyDebug.LogLogic("Visiting Event");
            base.Activate(party);
            int cnt = 0;
            while (party.HasVisitedEvent(randomEvent)&& cnt<100)
            {
                randomEvent = GenerateEvent();
                cnt++;
            }
            GameObject.FindObjectOfType<UIEventController>().Show(this,party);
            party.VisitedEvents.Add(randomEvent);
        
        
        }
    }
}