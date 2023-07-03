using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;
using Random = System.Random;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
   
    public class OnAttackEffect:PassiveSkill
    {
        public AttackEffects attackEffects;
        public float procChance;
        public OnAttackEffect(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr, float procChance, AttackEffects attackEffects) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
            this.procChance = procChance;
            this.attackEffects = attackEffects;
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Activation rate: ", (procChance*100f)+"%"));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            // unit.BattleComponent.OnAttack += ReactToAttack;
      
        }
        public override void UnbindSkill(Unit unit)
        {
            // unit.BattleComponent.OnAttack -= ReactToAttack;
           
        }
        private void ReactToAttack(Unit unit)
        {

                if(UnityEngine.Random.value<=procChance)
                    unit.BattleComponent.BattleStats.BonusAttackStats.AttackEffects.ApplyPositives(attackEffects);
        }
       
    }
}