using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;
using Random = System.Random;

namespace Game.GameResources
{
    [Serializable]
    public class EventData:IEventData
    {
        [SerializeField] List<LGEventDialogSO> tier0Events;
        [SerializeField] private LGEventDialogSO[] allEvents;
        private Random rng = new Random();
        public LGEventDialogSO GetRandomEvent()
        {
            if(rng==null)
                rng = new Random();
            if(GameConfig.Instance.ConfigProfile.overWriteEvents)
                return allEvents[rng.Next(0, allEvents.Length)];
            return tier0Events[rng.Next(0, tier0Events.Count)];
           
        }
       
#if UNITY_EDITOR
        public void OnValidate()
        {
            if (GameConfig.Instance.ConfigProfile.overWriteEvents)
            {
                var demoEvents = GameConfig.Instance.ConfigProfile.OverwritenEvents;
                allEvents = GameBPData.GetAllInstances<LGEventDialogSO>().Intersect(demoEvents).ToArray();
            }
            else
            {
                allEvents = GameBPData.GetAllInstances<LGEventDialogSO>();
            }
        }
        #endif
       

        public LGEventDialogSO GetEventById(string prefabName)
        {
            Debug.Log("Get Event by ID: "+prefabName);
            return allEvents.First(a => a.name == prefabName);
        }
    }
}