using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Event
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Event", fileName = "Event1")]
    public class RandomEvent :ScriptableObject
    {
        public string headline;
        public List<EventScene> scenes;
   

        public RandomEvent(List<EventScene> scenes)
        {
            this.scenes = scenes;
        }
  
    }
}