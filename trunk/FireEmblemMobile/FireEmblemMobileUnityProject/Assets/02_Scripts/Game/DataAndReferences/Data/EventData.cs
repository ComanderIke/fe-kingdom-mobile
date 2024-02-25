using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Players;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;
using Random = System.Random;

namespace Game.GameResources
{
    [Serializable]
    public class EventData:IEventData
    {
        [SerializeField] List<LGEventDialogSO> testEvents;
        [SerializeField] List<LGEventDialogSO> Area1OnlyEvents;
        [SerializeField] List<LGEventDialogSO> Area2OnlyEvents;
        [SerializeField] private LGEventDialogSO[] allEvents;
        [SerializeField] private LGEventDialogSO[] allEventsFiltered;
        [SerializeField] private List<LGEventDialogSO> allEventsExceptAreaOnlyEvents;
        private Random rng = new Random();
        public LGEventDialogSO GetRandomEvent()
        {
            if(rng==null)
                rng = new Random();
            if (GameConfig.Instance.ConfigProfile.overWriteEvents)
            {
                var eventPool = new List<LGEventDialogSO>(allEventsExceptAreaOnlyEvents);
                switch (Player.Instance.Party.AreaIndex)
                {
                    case 0: 
                        eventPool.AddRange(Area1OnlyEvents);
                        break;
                    case 1:
                        eventPool.AddRange(Area2OnlyEvents);break;
                }
                return eventPool[rng.Next(0, eventPool.Count)];
            }
                
            return testEvents[rng.Next(0, testEvents.Count)];
           
        }
       
#if UNITY_EDITOR
        public void OnValidate()
        {
            if (GameConfig.Instance.ConfigProfile.overWriteEvents)
            {
                var demoEvents = GameConfig.Instance.ConfigProfile.OverwritenEvents;
                allEventsFiltered = GameBPData.GetAllInstances<LGEventDialogSO>().Intersect(demoEvents).ToArray();
            }
            else
            {
                allEventsFiltered = GameBPData.GetAllInstances<LGEventDialogSO>();
               
            }
            allEvents = GameBPData.GetAllInstances<LGEventDialogSO>();
            allEventsExceptAreaOnlyEvents = allEventsFiltered.Except(Area1OnlyEvents).ToList();
            allEventsExceptAreaOnlyEvents = allEventsExceptAreaOnlyEvents.Except(Area2OnlyEvents).ToList();
        }
        #endif
       

        public LGEventDialogSO GetEventById(string prefabName)
        {
             Debug.Log("Get Event by ID: "+prefabName);
            return allEvents.First(a => a.name == prefabName);
        }
    }
}