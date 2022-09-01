using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Game.GameResources
{
    [Serializable]
    public class EventData:IEventData
    {
        [SerializeField] List<RandomEvent> tier0Events;
        [SerializeField] List<RandomEvent> tier1Events;
        [SerializeField] List<RandomEvent> tier2Events;
        [SerializeField] List<RandomEvent> specialEvents;

        public RandomEvent GetRandomEvent(int tier)
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
        public RandomEvent GetSpecialEvent(int index)
        {
            if (index < 0 || index > specialEvents.Count - 1)
                return null;

            return specialEvents[index];
        }
    }
}