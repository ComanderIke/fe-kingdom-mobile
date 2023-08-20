using System;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Passive;
using Game.GameInput;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units
{
    public interface IEncounterEventListener
    {
        
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
             attackEffects = new List<AttackEffectContainer>();
        }
        public BattleComponent(BattleComponent battleComponent)
        {
            BattleStats = battleComponent.BattleStats;
            owner = battleComponent.owner;
            battleEvents = battleComponent.battleEvents;
            attackEffects = battleComponent.attackEffects;
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

        public struct AttackEffectContainer
        {
            public IOnAttackEffect attackEffect;
            public Skill skill;

            public AttackEffectContainer(Skill skill, IOnAttackEffect attackEffect)
            {
                this.skill = skill;
                this.attackEffect = attackEffect;
            }
        }
        public List<AttackEffectContainer> attackEffects;
        public void AddToAttackSkillList(Skill skill, IOnAttackEffect attackEffectMixin)
        {
            attackEffects.Add(new AttackEffectContainer(skill, attackEffectMixin));
        }
        public void RemoveFromAttackSkillList(Skill skill, IOnAttackEffect attackEffectMixin)
        {
            Debug.Log("TRY REMOVE FROM ATTACK SKILL LIST: ");
            Debug.Log(skill.Name);
            Debug.Log(attackEffectMixin);
            for (int i= attackEffects.Count-1; i>=0; i--)
            {
                if (attackEffects[i].skill == skill && attackEffects[i].attackEffect == attackEffectMixin)
                {
                    attackEffects.RemoveAt(i);
                }
            }
        }

        public void InitiatesBattle(IBattleActor opponent)
        {
            if (battleEvents.ContainsKey(BattleEvent.InitiateCombat))
            {
                foreach (var listener in battleEvents[BattleEvent.InitiateCombat])
                {
                    listener.Activate((Unit)owner,(Unit)opponent);
                }
            }
        }
        public void BattleEnded(IBattleActor opponent)
        {
            if (battleEvents.ContainsKey(BattleEvent.InitiateCombat))
            {
                foreach (var listener in battleEvents[BattleEvent.InitiateCombat])
                {
                    listener.Deactivate((Unit)owner,(Unit)opponent);
                }
            }
        }
    }
    public interface IOnAttackEffect
    {
        bool ReactToAttack(IBattleActor unit);
    }
}