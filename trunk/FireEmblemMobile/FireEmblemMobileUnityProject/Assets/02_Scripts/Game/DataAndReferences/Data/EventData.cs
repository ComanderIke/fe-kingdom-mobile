using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.Dialog.DialogSystem;
using UnityEngine;
using Random = System.Random;

namespace Game.GameResources
{
    [Serializable]
    public class EventData:IEventData
    {
        [SerializeField] List<LGEventDialogSO> tier0Events;
        [SerializeField] List<LGEventDialogSO> tier1Events;
        [SerializeField] List<LGEventDialogSO> tier2Events;
        [SerializeField] List<LGEventDialogSO> specialEvents;

        public LGEventDialogSO GetRandomEvent(int tier)
        {
            var rng = new Random();
            
            switch (tier)
            {
                case 0: return tier0Events[rng.Next(0, tier0Events.Count)];
                case 1: return tier1Events[rng.Next(0, tier1Events.Count)];
                case 2: return tier2Events[rng.Next(0, tier2Events.Count)];
                default: return null;
            }
        }
        public LGEventDialogSO GetSpecialEvent(int index)
        {
            if (index < 0 || index > specialEvents.Count - 1)
                return null;

            return specialEvents[index];
        }
    }
}