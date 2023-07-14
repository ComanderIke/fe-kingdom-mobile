using System;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units.Skills.Passive;
using Game.GameInput;
using Game.Mechanics;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units
{
    public interface IEncounterEventListener
    {
        
    }
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

    public class BattleComponent
    {
        public BattleStats BattleStats { get;  set; }

        private readonly IBattleActor owner;
        public event Action<IBattleActor> onAttack;
        private Dictionary<BattleEvent, List<IBattleEventListener>> battleEvents;

        public BattleComponent(IBattleActor actor)
        {
             BattleStats = new BattleStats(actor);
             owner = actor;
             battleEvents = new Dictionary<BattleEvent, List<IBattleEventListener>>();
        }

      
        // public int InflictDamage(int dmg,bool magic, bool crit,bool eff, IBattleActor damageDealer)
        // {
        //     //crit = Random.Range(0, 100) <= 50;
        //     if(BattleActor is Unit unit)
        //         Unit.OnUnitDamaged?.Invoke(unit, dmg,magic, crit, eff);
        //     BattleActor.Hp -= dmg;
        //     return dmg;
        // }

        public bool IsEffective(EffectiveAgainstType effectiveAgainst)
        {
            return owner.GetEquippedWeapon().IsEffective(effectiveAgainst);
        }
        public float GetEffectiveCoefficient(EffectiveAgainstType effectiveAgainst)
        {
            return owner.GetEquippedWeapon().GetEffectiveCoefficient(effectiveAgainst);
        }
        

        public bool HasAdvantage(EffectiveAgainstType type)
        {
            return owner.GetEquippedWeapon().HasAdvantage(type);
        }

       
        public void AddListener(BattleEvent battleEvent, IBattleEventListener listener)
        {
            if (!battleEvents.ContainsKey(battleEvent))
            {
                battleEvents.Add(battleEvent, new List<IBattleEventListener>(){listener});
            }
            else
            {
                battleEvents[battleEvent].Add(listener);
            }
           
        }

        public void RemoveListener(BattleEvent battleEvent, IBattleEventListener listener)
        {
            if (battleEvents.ContainsKey(battleEvent))
            {
                battleEvents[battleEvent].Remove(listener);
            }
            
        }
    }
}