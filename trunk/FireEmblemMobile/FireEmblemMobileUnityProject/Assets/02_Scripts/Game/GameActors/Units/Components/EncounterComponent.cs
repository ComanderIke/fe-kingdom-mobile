using System.Collections.Generic;
using LostGrace;

namespace Game.GameActors.Units
{
    public class EncounterComponent
    {
        private Dictionary<EncounterEvent, List<IEncounterEventListener>> encounterEvents;

        public EncounterComponent()
        {
            encounterEvents = new Dictionary<EncounterEvent, List<IEncounterEventListener>>();
        }
        public void AddListener(EncounterEvent encounterEvent, IEncounterEventListener listener)
        {
            if (!encounterEvents.ContainsKey(encounterEvent))
            {
                encounterEvents.Add(encounterEvent, new List<IEncounterEventListener>(){listener});
            }
            else
            {
                encounterEvents[encounterEvent].Add(listener);
            }
           
        }

        public void RemoveListener(EncounterEvent encounterEvent, IEncounterEventListener listener)
        {
            if (encounterEvents.ContainsKey(encounterEvent))
            {
                encounterEvents[encounterEvent].Remove(listener);
            }
            
        }
    }
}