using System;
using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Battle;
using Game.GameActors.Units;

namespace Game.EncounterAreas.Encounters.Event
{
    public enum EventSceneType
    {
        Normal,
        Memory,
        Fight,
        Merchant
    }
    [System.Serializable]
    public class EventFightData
    {
        public BattleMap battleMap;
        public UnitBP EnemyToFight;
        public bool lethalfight;
    }

    [Serializable]
    public class ResponseOption
    {
        public string Text;
        public int StatRequirement;
        public int StatIndex;
        public Reward reward;
    
        public EventFightData fightData;
        public EventSceneType type;
   
        public bool statcheck;
        public List<EventOutcome> outcomes;

        public ResponseOption(string text,int statRequirement, int statIndex, Reward reward)
        {
            this.Text = text;
            this.StatIndex = statIndex;
            this.StatRequirement = statRequirement;
            this.reward = reward;
        }

    
    }

    [Serializable]
    public class EventOutcome
    {
        public int nextSceneIndex=-1;
    }
}